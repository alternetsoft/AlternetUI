using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a collection of items within a list box.
    /// </summary>
    /// <remarks>
    /// This class provides list-like behavior and supports multiple collection interfaces.
    /// It enables item management, enumeration, and indexing within a structured list.
    /// </remarks>
    /// <seealso cref="ICollection{T}"/>
    /// <seealso cref="IEnumerable{T}"/>
    /// <seealso cref="IList{T}"/>
    /// <seealso cref="IReadOnlyCollection{T}"/>
    /// <seealso cref="IReadOnlyList{T}"/>
    /// <seealso cref="ICollection"/>
    /// <seealso cref="IList"/>
    public class ListBoxItems : ICollection<object>, IEnumerable<object>,
        IEnumerable, IList<object>, IReadOnlyCollection<object>, IReadOnlyList<object>,
        ICollection, IList
    {
        private readonly BaseCollection<ListControlItem> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItems"/> class.
        /// </summary>
        public ListBoxItems()
            : this(new BaseCollection<ListControlItem>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItems"/> class
        /// to use live items from the specified collection.
        /// </summary>
        /// <param name="items">The source of the items.</param>
        public ListBoxItems(BaseCollection<ListControlItem> items)
        {
            this.items = items;

            items.PropertyChanged += Items_PropertyChanged;
            items.CollectionChanged += Items_CollectionChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItems"/> class.
        /// </summary>
        /// <param name="items"></param>
        public ListBoxItems(IEnumerable<object> items)
            : this(new BaseCollection<ListControlItem>())
        {
            AddRange(items);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ListBoxItems"/> class.
        /// </summary>
        ~ListBoxItems()
        {
            items.PropertyChanged -= Items_PropertyChanged;
            items.CollectionChanged -= Items_CollectionChanged;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Occurs when the collection's contents change.
        /// </summary>
        /// <remarks>
        /// This event is triggered whenever an item is added, removed, or the collection is reset.
        /// It allows subscribers to react to collection modifications dynamically.
        /// </remarks>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <inheritdoc/>
        public int Count => items.Count;

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool IsFixedSize => ((IList)items).IsFixedSize;

        /// <inheritdoc/>
        public bool IsSynchronized => ((ICollection)items).IsSynchronized;

        /// <inheritdoc/>
        public object SyncRoot => ((ICollection)items).SyncRoot;

        /// <inheritdoc/>
        public object this[int index]
        {
            get
            {
                return items[index].Value!;
            }

            set
            {
                items[index].Value = value;
            }
        }

        /// <summary>
        /// Implicitly converts an object collection to a ListBoxItems collection.
        /// </summary>
        /// <param name="items">The array of objects to convert.</param>
        public static implicit operator ListBoxItems(object[] items)
        {
            return new ListBoxItems(items);
        }

        /// <inheritdoc/>
        public void Add(object value)
        {
            ListControlItem item = new();
            item.Value = value;
            items.Add(item);
        }

        /// <summary>
        /// Adds multiple items to the collection.
        /// </summary>
        /// <param name="collection">The collection of objects to add.</param>
        /// <remarks>
        /// Each item in the provided collection is individually added.
        /// This method ensures bulk addition while maintaining integrity.
        /// </remarks>
        public void AddRange(IEnumerable<object> collection)
        {
            foreach (var item in collection)
                Add(item);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            items.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(object value)
        {
            foreach(var item in items)
            {
                if (item.Value == value)
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void Insert(int index, object value)
        {
            ListControlItem item = new();
            item.Value = value;
            items.Insert(index, item);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        /// <inheritdoc/>
        public int IndexOf(object item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Value == item)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <inheritdoc/>
        public bool Remove(object item)
        {
            var index = IndexOf(item);

            if(index >= 0)
            {
                items.RemoveAt(index);
                return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList.Add(object value)
        {
            ListControlItem item = new();
            item.Value = value;
            return ((IList)items).Add(item);
        }

        void IList.Remove(object value)
        {
            Remove(value);
        }

        /// <inheritdoc/>
        public void CopyTo(object[] array, int arrayIndex)
        {
            var itm = items.Select(x => x.Value).ToList();
            itm.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            var itm = items.Select(x => x.Value).ToList();
            ((ICollection)itm).CopyTo(array, index);
        }

        /// <inheritdoc/>
        public IEnumerator<object> GetEnumerator()
        {
            var itm = items.Select(x => x.Value!);
            return itm.GetEnumerator();
        }

        private void Items_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged is null)
                return;

            var action = e.Action;

            // Gets the list of new items involved in the change.
            var newItems = ToValues(e.NewItems);

            // Gets the index at which the change occurred.
            var newStartingIndex = e.NewStartingIndex;

            // Gets the list of items affected by a Replace, Remove, or Move action.
            var oldItems = ToValues(e.OldItems);

            // Gets the index at which a Move, Remove, or Replace action occurred.
            var oldStartingIndex = e.OldStartingIndex;

            IList ToValues(IList? items)
            {
                IList result = new List<object>();

                if (items is null)
                    return result;

                foreach(var item in items)
                {
                    result.Add(((ListControlItem)item).Value);
                }

                return result;
            }

            NotifyCollectionChangedEventArgs args;

            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    args = CollectionChangedHelper.OnInsertItem(newStartingIndex, newItems);
                    CollectionChanged(this, args);
                    break;
                case NotifyCollectionChangedAction.Move:
                    args = CollectionChangedHelper
                        .OnMoveItem(oldItems, oldStartingIndex, newStartingIndex);
                    CollectionChanged(this, args);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    args = CollectionChangedHelper.OnRemoveItem(oldStartingIndex, oldItems);
                    CollectionChanged(this, args);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    args = CollectionChangedHelper
                        .OnReplaceItem(oldItems, oldStartingIndex, newItems);
                    CollectionChanged(this, args);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    args = CollectionChangedHelper.OnResetItems();
                    CollectionChanged(this, args);
                    break;
            }
        }

        private static class CollectionChangedHelper
        {
            public static NotifyCollectionChangedEventArgs OnResetItems()
            {
                return new(NotifyCollectionChangedAction.Reset);
            }

            public static NotifyCollectionChangedEventArgs OnRemoveItem(int index, IList val)
            {
                return new (NotifyCollectionChangedAction.Remove, val, index);
            }

            public static NotifyCollectionChangedEventArgs OnInsertItem(int index, IList item)
            {
                return new (NotifyCollectionChangedAction.Add, item, index);
            }

            public static NotifyCollectionChangedEventArgs OnReplaceItem(
                IList oldItem,
                int index,
                IList newItem)
            {
                return new(NotifyCollectionChangedAction.Replace, oldItem, newItem, index);
            }

            public static NotifyCollectionChangedEventArgs OnMoveItem(
                IList val,
                int oldIndex,
                int newIndex)
            {
                return new(NotifyCollectionChangedAction.Move, val, newIndex, oldIndex);
            }
        }
    }
}