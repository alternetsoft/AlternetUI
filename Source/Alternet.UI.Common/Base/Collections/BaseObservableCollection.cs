// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
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

        /// <summary>
        /// Initializes a new instance of collection that is empty and
        /// has default initial capacity.
        /// </summary>
        public BaseObservableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the collection that contains
        /// elements copied from the specified collection and has sufficient capacity
        /// to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <remarks>
        /// The elements are copied onto the collection in the
        /// same order they are read by the enumerator of the collection.
        /// </remarks>
        /// <exception cref="ArgumentNullException"> collection is a null reference.</exception>
        public BaseObservableCollection(IEnumerable<T> collection)
            : base(new List<T>(collection ?? throw new ArgumentNullException(nameof(collection))))
        {
        }

        /// <summary>
        /// Initializes a new instance of the collection
        /// as a wrapper for the specified list.
        /// </summary>
        /// <param name="list">The list that is wrapped by the new collection.</param>
        /// <param name="createCopy">Whether to create copy of the list.</param>
        public BaseObservableCollection(IList<T> list, bool createCopy = false)
            : base(createCopy
                  ? new List<T>(list ?? throw new ArgumentNullException(nameof(list)))
                  : list)
        {
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
        /// Move item at oldIndex to newIndex.
        /// </summary>
        public void Move(int oldIndex, int newIndex) => MoveItem(oldIndex, newIndex);

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
        /// Called by base class when an item is
        /// to be moved within the list;
        /// raises a <see cref="CollectionChanged"/> event to any listeners.
        /// </summary>
        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            CheckReentrancy();

            T removedItem = this[oldIndex];

            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, removedItem);

            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
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
        /// Helper to raise a <see cref="PropertyChanged"/> event for the Count property.
        /// </summary>
        private void OnCountPropertyChanged()
            => OnPropertyChanged(EventArgsCache.CountPropertyChanged);

        /// <summary>
        /// Helper to raise a <see cref="PropertyChanged"/> event for the Indexer property.
        /// </summary>
        private void OnIndexerPropertyChanged()
            => OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);

        /// <summary>
        /// Helper to raise <see cref="CollectionChanged"/> event to any listeners.
        /// </summary>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object? item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        /// <summary>
        /// Helper to raise <see cref="CollectionChanged"/> event to any listeners.
        /// </summary>
        private void OnCollectionChanged(
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
        private void OnCollectionChanged(
            NotifyCollectionChangedAction action,
            object? oldItem,
            object? newItem,
            int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        /// <summary>
        /// Helper to raise <see cref="CollectionChanged"/> event with action == Reset to any listeners.
        /// </summary>
        private void OnCollectionReset() => OnCollectionChanged(EventArgsCache.ResetCollectionChanged);

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

        internal static class EventArgsCache
        {
            internal static readonly PropertyChangedEventArgs CountPropertyChanged
                = new ("Count");

            internal static readonly PropertyChangedEventArgs IndexerPropertyChanged
                = new ("Item[]");

            internal static readonly NotifyCollectionChangedEventArgs ResetCollectionChanged
                = new (NotifyCollectionChangedAction.Reset);
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