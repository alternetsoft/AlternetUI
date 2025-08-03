// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Source code for list and collection:
// https://github.com/microsoft/referencesource/blob/master/mscorlib/system/collections/generic/list.cs
// https://github.com/microsoft/referencesource/blob/master/mscorlib/system/collections/objectmodel/collection.cs

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Implementation of a dynamic data collection based on generic collection
    /// implementing <see cref="INotifyCollectionChanged"/> to notify listeners
    /// when items get added, removed or the whole list is refreshed.
    /// </summary>
    [Serializable]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    public class BaseObservableCollection<T>
        : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        // Lazily allocated only when a subclass calls BlockReentrancy() or during
        // serialization. Do not rename (binary serialization)
        private SimpleMonitor? monitor;

        [NonSerialized]
        private int blockReentrancyCount;

        [NonSerialized]
        private List<T> list;

        static BaseObservableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of collection that is empty and
        /// has default initial capacity.
        /// </summary>
        public BaseObservableCollection()
            : this(new List<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the collection
        /// as a wrapper for the specified list.
        /// </summary>
        /// <param name="list">The list that is wrapped by the new collection.</param>
        public BaseObservableCollection(List<T> list)
            : base(list)
        {
            this.list = list;
        }

        /// <summary>
        /// Occurs when the collection changes, either by adding or removing an item.
        /// </summary>
        [field: NonSerialized]
        public virtual event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        [field: NonSerialized]
        public virtual event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets access to the internal list.
        /// </summary>
        /// <remarks>
        /// When you call methods of the internal list, collection events are not called.
        /// </remarks>
        protected List<T> List => list;

        /// <summary>
        /// Sorts the elements in this collection.  Uses the default comparer.
        /// </summary>
        public virtual void Sort()
        {
            CheckReentrancy();

            if (Items is List<T> list)
            {
                list.Sort();
            }
            else
            {
                throw new NotImplementedException();
            }

            OnIndexerPropertyChanged();
            OnCollectionReset();
        }

        /// <summary>
        /// Sorts the elements in this collection. Uses the provided comparer.
        /// </summary>
        public virtual void Sort(IComparer<T>? comparer)
        {
            CheckReentrancy();

            if (Items is List<T> list)
            {
                list.Sort(comparer);
            }
            else
            {
                throw new NotImplementedException();
            }

            OnIndexerPropertyChanged();
            OnCollectionReset();
        }

        /// <summary>
        /// Sorts the elements in a section of this collection. The sort compares the
        /// elements to each other using the given IComparer interface. If
        /// comparer is null, the elements are compared to each other using
        /// the IComparable interface, which in that case must be implemented by all
        /// elements of the list.
        /// </summary>
        public virtual void Sort(int index, int count, IComparer<T>? comparer)
        {
            CheckReentrancy();

            if (Items is List<T> list)
            {
                list.Sort(index, count, comparer);
            }
            else
            {
                throw new NotImplementedException();
            }

            OnIndexerPropertyChanged();
            OnCollectionReset();
        }

        /// <summary>
        /// Sorts the elements in the entire collection using the specified comparison.
        /// </summary>
        public virtual void Sort(Comparison<T> comparison)
        {
            CheckReentrancy();

            if (Items is List<T> list)
            {
                list.Sort(comparison);
            }
            else
            {
                throw new NotImplementedException();
            }

            OnIndexerPropertyChanged();
            OnCollectionReset();
        }

        /// <summary>
        /// Sorts the items in descending order based on the specified comparison logic.
        /// </summary>
        /// <remarks>This method performs the sorting operation only if the collection contains items.
        /// The sorting is executed in descending order by reversing the result of the provided
        /// comparison logic.</remarks>
        /// <param name="comparison">A delegate that defines the comparison logic
        /// to apply to the items. The delegate
        /// should return a value less than zero if the first item is less than the second,
        /// zero if they are equal, or
        /// greater than zero if the first item is greater than the second.</param>
        public virtual void SortDescending(Comparison<T> comparison)
        {
            Sort(DescendingComparison);

            int DescendingComparison(T x, T y)
            {
                var result = comparison(x, y);
                return result < 0 ? 1 : result > 0 ? -1 : 0;
            }
        }

        /// <summary>
        /// Moves item from oldIndex to newIndex within the list.
        /// Raises a <see cref="CollectionChanged"/> event to any listeners.
        /// </summary>
        public virtual void Move(int oldIndex, int newIndex)
        {
            CheckReentrancy();

            T removedItem = this[oldIndex];

            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, removedItem);

            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
        }

        /// <summary> Check and assert for reentrant attempts to change this collection. </summary>
        /// <exception cref="InvalidOperationException"> raised when changing the collection
        /// while another collection change is still being notified to other listeners.</exception>
        protected void CheckReentrancy()
        {
            if (blockReentrancyCount > 0)
            {
                // we can allow changes if there's only one listener - the problem
                // only arises if reentrant changes make the original event args
                // invalid for later listeners.  This keeps existing code working
                // (e.g. Selector.SelectedItems).
                if (CollectionChanged?.GetInvocationList().Length > 1)
                    throw new InvalidOperationException("ObservableCollection reentrancy not allowed");
            }
        }

        /// <summary>
        /// Called by base class when the list is being cleared;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void ClearItems()
        {
            CheckReentrancy();
            base.ClearItems();
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionReset();
        }

        /// <summary>
        /// Called by base class when an item is removed from list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void RemoveItem(int index)
        {
            CheckReentrancy();
            T removedItem = this[index];

            base.RemoveItem(index);

            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
        }

        /// <summary>
        /// Called by base class when an item is added to list;
        /// raises a <see cref="CollectionChanged"/> event to any listeners.
        /// </summary>
        protected override void InsertItem(int index, T item)
        {
            CheckReentrancy();
            base.InsertItem(index, item);

            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>
        /// Called by base class when an item is set in list;
        /// raises a <see cref="CollectionChanged"/> event to any listeners.
        /// </summary>
        protected override void SetItem(int index, T item)
        {
            CheckReentrancy();
            T originalItem = this[index];
            base.SetItem(index, item);

            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, originalItem, item, index);
        }

        /// <summary>
        /// Raises a <see cref="PropertyChanged"/> event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raise <see cref="CollectionChanged"/> event to any listeners.
        /// Properties/methods modifying this collection will raise
        /// a collection changed event through this virtual method.
        /// </summary>
        /// <remarks>
        /// When overriding this method, either call its base implementation
        /// or call <see cref="BlockReentrancy"/> to guard against reentrant collection changes.
        /// </remarks>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler? handler = CollectionChanged;
            if (handler != null)
            {
                // Not calling BlockReentrancy() here to avoid the SimpleMonitor allocation.
                blockReentrancyCount++;
                try
                {
                    handler(this, e);
                }
                finally
                {
                    blockReentrancyCount--;
                }
            }
        }

        /// <summary>
        /// Helper to raise <see cref="CollectionChanged"/> event with action == Reset to any listeners.
        /// </summary>
        protected void OnCollectionReset()
        {
            OnCollectionChanged(EventArgsUtils.ResetCollectionChangedArgs);
        }

        /// <summary>
        /// Disallow reentrant attempts to change this collection. E.g. an event handler
        /// of the <see cref="CollectionChanged"/> event
        /// is not allowed to make changes to this collection.
        /// </summary>
        /// <remarks>
        /// typical usage is to wrap e.g. a OnCollectionChanged call with a using() scope:
        /// <code>
        ///         using (BlockReentrancy())
        ///         {
        ///             CollectionChanged(this, new NotifyCollectionChangedEventArgs(
        ///             action, item, index));
        ///         }
        /// </code>
        /// </remarks>
        protected IDisposable BlockReentrancy()
        {
            blockReentrancyCount++;
            return EnsureMonitorInitialized();
        }

        /// <summary>
        /// Helper to raise a <see cref="PropertyChanged"/> event for the Count property.
        /// </summary>
        protected void OnCountPropertyChanged()
            => OnPropertyChanged(EventArgsUtils.CountPropertyChangedArgs);

        /// <summary>
        /// Helper to raise a <see cref="PropertyChanged"/> event for the Indexer property.
        /// </summary>
        protected void OnIndexerPropertyChanged()
            => OnPropertyChanged(EventArgsUtils.IndexerPropertyChangedArgs);

        /// <summary>
        /// Helper to raise <see cref="CollectionChanged"/> event to any listeners.
        /// </summary>
        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object? item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        /// <summary>
        /// Helper to raise <see cref="CollectionChanged"/> event to any listeners.
        /// </summary>
        protected void OnCollectionChanged(
            NotifyCollectionChangedAction action,
            object? item,
            int index,
            int oldIndex)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        /// <summary>
        /// Helper to raise <see cref="CollectionChanged"/> event to any listeners.
        /// </summary>
        protected void OnCollectionChanged(
            NotifyCollectionChangedAction action,
            object? oldItem,
            object? newItem,
            int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        [Conditional("DEBUG")]
        private static void CallTests()
        {
            TestSort();
        }

        [Conditional("DEBUG")]
        private static void TestSort()
        {
            var numbers = new List<int> { 5, 2, 8, 1, 3 };
            var collection = new BaseObservableCollection<int>(numbers);
            collection.Sort();
        }

        private SimpleMonitor EnsureMonitorInitialized() => monitor ??= new SimpleMonitor(this);

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            EnsureMonitorInitialized();
            monitor!.busyCount = blockReentrancyCount;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (monitor != null)
            {
                blockReentrancyCount = monitor.busyCount;
                monitor.collection = this;
            }
        }

        internal sealed class CollectionDebugView<T1>
        {
            private readonly ICollection<T1> collection;

            public CollectionDebugView(ICollection<T1> collection)
            {
                if (collection is null)
                    throw new ArgumentNullException(nameof(collection));

                this.collection = collection;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public T1[] Items
            {
                get
                {
                    T1[] items = new T1[collection.Count];
                    collection.CopyTo(items, 0);
                    return items;
                }
            }
        }

        // this class helps prevent reentrant calls
        [Serializable]
        private sealed class SimpleMonitor : IDisposable
        {
            // Only used during (de)serialization to maintain compatibility
            // with desktop. Do not rename (binary serialization)
            internal int busyCount;

            [NonSerialized]
            internal BaseObservableCollection<T> collection;

            public SimpleMonitor(BaseObservableCollection<T> collection)
            {
                this.collection = collection;
            }

            public void Dispose() => collection.blockReentrancyCount--;
        }
    }
}