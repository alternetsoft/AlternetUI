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
    public class ListBoxItems : DisposableObject, IListSource<object?>, IList
    {
        private readonly ListSource<ListControlItem>? items;

        private Func<IListSource<ListControlItem>> provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItems"/> class.
        /// </summary>
#pragma warning disable
        public ListBoxItems(Func<IListSource<ListControlItem>> provider)
#pragma warning restore
        {
            SetProvider(provider);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxItems"/> class.
        /// </summary>
#pragma warning disable       
        public ListBoxItems()
#pragma warning restore       
        {
            items = new ListSource<ListControlItem>();
            SetProvider(() => items);
        }

        /// <summary>
        /// Occurs when an item is inserted.
        /// </summary>
        public event CollectionItemChangedHandler<object?>? ItemInserted;

        /// <summary>
        /// Occurs when an item is removed.
        /// </summary>
        public event CollectionItemChangedHandler<object?>? ItemRemoved;

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
        public IList AsList => this;

        bool IList.IsFixedSize => IsFixedSize;
        
        bool IList.IsReadOnly => IsReadOnly;

        int ICollection.Count => Count;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        object? IList.this[int index]
        {
            get { return this[index]; }
            set { this[index] = value; }
        }

        /// <inheritdoc/>
        public object? this[int index]
        {
            get
            {
                return provider().GetItem(index)?.Value;
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
        public void Add(object? value)
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
        public void AddRange(IEnumerable<object?> collection)
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
        public void CopyTo(object?[] array, int arrayIndex)
        {
            var itm = provider().Select(x => x.Value).ToList();
            itm.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<object?> GetEnumerator()
        {
            var itm = provider().Select(x => x.Value);
            return itm.GetEnumerator();
        }

        /// <inheritdoc/>
        public void SetCount(int count, Func<object?> fnCreateItem)
        {
            provider().SetCount(count, () =>
            {
                var item = new ListControlItem();
                item.Value = fnCreateItem();
                return item;
            });
        }

        /// <inheritdoc/>
        public void SetItem(int index, object? value)
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

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            provider().PropertyChanged -= OnItemsPropertyChanged;
            provider().CollectionChanged -= OnItemsCollectionChanged;
            provider().ItemInserted -= OnItemsItemInserted;
            provider().ItemRemoved -= OnItemsItemRemoved;
            base.DisposeManaged();
        }

        private void OnItemsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChangedEx(e);
        }

        private void SetProvider(Func<IListSource<ListControlItem>> provider)
        {
            this.provider = provider;

            provider().PropertyChanged += OnItemsPropertyChanged;
            provider().CollectionChanged += OnItemsCollectionChanged;
            provider().ItemInserted += OnItemsItemInserted;
            provider().ItemRemoved += OnItemsItemRemoved;
        }

        private void OnItemsCollectionChanged(object? sender, ListChangedEventArgs e)
        {
            if (CollectionChanged is null)
                return;

            ListChangedEventArgs newArgs = e.Clone(ToValues);

            IList? ToValues(IList? items)
            {
                if (items is null)
                    return null;

                IList result = new List<object>();

                foreach (var item in items)
                {
                    result.Add(((ListControlItem)item).Value);
                }

                return result;
            }

            CollectionChanged(this, newArgs);
        }

        private void OnItemsItemRemoved(object? sender, int index, ListControlItem item)
        {
            ItemRemoved?.Invoke(this, index, item.Value);
        }

        private void OnItemsItemInserted(object? sender, int index, ListControlItem item)
        {
            ItemInserted?.Invoke(this, index, item.Value);
        }

        int IList.Add(object? value)
        {
            Add(value);
            return Count - 1;
        }

        void IList.Clear()
        {
            Clear();
        }

        bool IList.Contains(object? value)
        {
            return IndexOf(value) >= 0;
        }

        int IList.IndexOf(object? value)
        {
            return IndexOf(value);
        }

        void IList.Insert(int index, object? value)
        {
            Insert(index, value);
        }

        void IList.Remove(object? value)
        {
            Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((object?[])array, index);
        }
    }
}