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
    public class ListBoxItems : IListSource<object>
    {
        private readonly ListSource<ListControlItem>? items;
        private readonly Func<IListSource<ListControlItem>> provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItems"/> class.
        /// </summary>
        public ListBoxItems(Func<IListSource<ListControlItem>> provider)
        {
            this.provider = provider;

            provider().PropertyChanged += Items_PropertyChanged;
            provider().CollectionChanged += Items_CollectionChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItems"/> class.
        /// </summary>
        public ListBoxItems()
        {
            items = new ListSource<ListControlItem>();
            provider = () => items;
            provider().PropertyChanged += Items_PropertyChanged;
            provider().CollectionChanged += Items_CollectionChanged;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ListBoxItems"/> class.
        /// </summary>
        ~ListBoxItems()
        {
            provider().PropertyChanged -= Items_PropertyChanged;
            provider().CollectionChanged -= Items_CollectionChanged;
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
        public event ListChangedEventHandler? CollectionChanged;

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                return provider().Count;
            }

            set
            {
                provider().Count = value;
            }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool IsFixedSize => false;

        /// <inheritdoc/>
        public IList AsList => provider().AsList;

        /// <inheritdoc/>
        public object this[int index]
        {
            get
            {
                return provider().GetItem(index)?.Value!;
            }

            set
            {
                var item = provider().GetItem(index);
                if (item != null)
                {
                    item.Value = value;
                }
            }
        }

        /// <summary>
        /// Implicitly converts an object collection to a ListBoxItems collection.
        /// </summary>
        /// <param name="items">The array of objects to convert.</param>
        public static implicit operator ListBoxItems(object[] items)
        {
            var result = new ListBoxItems();
            result.AddRange(items);
            return result;
        }

        /// <inheritdoc/>
        public void Add(object value)
        {
            ListControlItem item = new();
            item.Value = value;
            provider().Add(item);
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
            provider().Clear();
        }

        /// <inheritdoc/>
        public bool Contains(object? value)
        {
            for (int i = 0; i < provider().Count; i++)
            {
                var item = provider().GetItem(i);
                if (item != null && item.Value == value)
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void Insert(int index, object? value)
        {
            ListControlItem item = new();
            item.Value = value;
            provider().Insert(index, item);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            provider().RemoveAt(index);
        }

        /// <inheritdoc/>
        public int IndexOf(object? item)
        {
            for (int i = 0; i < provider().Count; i++)
            {
                var listItem = provider().GetItem(i);
                if (listItem != null && listItem.Value == item)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <inheritdoc/>
        public bool Remove(object? item)
        {
            var index = IndexOf(item);

            if (index >= 0)
            {
                provider().RemoveAt(index);
                return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public void CopyTo(object[] array, int arrayIndex)
        {
            var itm = provider().Select(x => x.Value).ToList();
            itm.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<object> GetEnumerator()
        {
            var itm = provider().Select(x => x.Value!);
            return itm.GetEnumerator();
        }

        private void Items_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private void Items_CollectionChanged(object? sender, ListChangedEventArgs e)
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

                foreach (var item in items)
                {
                    result.Add(((ListControlItem)item).Value);
                }

                return result;
            }

            ListChangedEventArgs args;

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

        /// <inheritdoc/>
        public void SetCount(int count, Func<object> fnCreateItem)
        {
            provider().SetCount(count, () =>
            {
                var item = new ListControlItem();
                item.Value = fnCreateItem();
                return item;
            });
        }

        /// <inheritdoc/>
        public void SetItem(int index, object value)
        {
            var item = provider().GetItem(index);
            if (item != null)
            {
                item.Value = value;
            }
        }

        /// <inheritdoc/>
        public void Sort()
        {
            provider().Sort();
        }

        /// <inheritdoc/>
        public object? GetItem(int index)
        {
            return provider().GetItem(index)?.Value;
        }

        /// <inheritdoc/>
        public void Sort(IComparer<object>? comparer)
        {
            provider().Sort(comparer);
        }

        /// <inheritdoc/>
        public void Sort(int index, int count, IComparer<object>? comparer)
        {
            provider().Sort(index, count, comparer);
        }

        /// <inheritdoc/>
        public void Sort(Comparison<object> comparison)
        {
            provider().Sort(comparison);
        }

        /// <inheritdoc/>
        public void SortDescending(Comparison<object> comparison)
        {
            provider().SortDescending(comparison);
        }

        private static class CollectionChangedHelper
        {
            public static ListChangedEventArgs OnResetItems()
            {
                return new(NotifyCollectionChangedAction.Reset);
            }

            public static ListChangedEventArgs OnRemoveItem(int index, IList val)
            {
                return new(NotifyCollectionChangedAction.Remove, val, index);
            }

            public static ListChangedEventArgs OnInsertItem(int index, IList item)
            {
                return new(NotifyCollectionChangedAction.Add, item, index);
            }

            public static ListChangedEventArgs OnReplaceItem(
                IList oldItem,
                int index,
                IList newItem)
            {
                return new(NotifyCollectionChangedAction.Replace, oldItem, newItem, index);
            }

            public static ListChangedEventArgs OnMoveItem(
                IList val,
                int oldIndex,
                int newIndex)
            {
                return new(NotifyCollectionChangedAction.Move, val, newIndex, oldIndex);
            }
        }
    }
}