using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Alternet.UI;

namespace Alternet.Base.Collections
{
    /// <summary>
    /// Represents the method that will handle an event when collection is changed.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="index">The index where the change occurred.</param>
    /// <param name="item">The item affected by the change.</param>
    public delegate void CollectionItemChangedHandler<T>(object? sender, int index, T item);

    /// <summary>
    /// Represents the method that will handle an event when multiple item in the
    /// collection were changed.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="index">The index where the change occurred.</param>
    /// <param name="items">The items being affected.</param>
    public delegate void CollectionItemRangeChangedHandler<T>(
        object? sender,
        int index,
        IEnumerable<T> items);

    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items get added,
    /// removed, or when the whole list is refreshed.
    /// In addition to the functionality available in <see cref="ObservableCollection{T}"/>,
    /// provides <see cref="ItemInserted"/> and <see cref="ItemRemoved"/> events.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class Collection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Occurs when an item is inserted in the collection.
        /// </summary>
        public event CollectionItemChangedHandler<T>? ItemInserted;

        /// <summary>
        /// Occurs when an item is removed from the collection.
        /// </summary>
        public event CollectionItemChangedHandler<T>? ItemRemoved;

        /// <summary>
        /// Occurs when an item range addition is finished in the collection.
        /// </summary>
        public event CollectionItemRangeChangedHandler<T>? ItemRangeAdditionFinished;

        /// <summary>
        /// Returns <see langword="true"/> if <see cref="AddRange"/> is being executed at the moment.
        /// </summary>
        public bool RangeOpInProgress { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether an
        /// <see cref="ArgumentNullException"/> should be thrown
        /// on an attempt to add a <c>null</c> value to the collection.
        /// </summary>
        public bool ThrowOnNullAdd { get; set; }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[long index]
        {
            get => Items[(int)index];
            set => SetItem((int)index, value);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public new T this[int index]
        {
            get => base[index];
            set
            {
                SetItem(index, value);
            }
        }

        /// <summary>
        /// Adds the elements of the specified <see cref="IEnumerable{T}"/> to the end
        /// of the collection.
        /// </summary>
        /// <param name="collection">
        /// The <see cref="IEnumerable{T}"/> whose elements should be added to the end
        /// of the collection.
        /// The value itself cannot be <c>null</c>, but it can contain elements that are
        /// <c>null</c>, if type <c>T</c> is a reference type.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// <c>collection</c> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// The order of the elements in the <see cref="IEnumerable{T}"/> is preserved
        /// in the collection.
        /// </remarks>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            RangeOpInProgress = true;

            int index = Count;
            try
            {
                foreach (var item in collection)
                    Add(item);
            }
            finally
            {
                RangeOpInProgress = false;
                OnItemRangeAdditionFinished(index, collection);
            }
        }

        public new void SetItem(int index, T item)
        {
            if (Items.IsReadOnly)
                throw new NotSupportedException(ErrorMessages.Default.CannotChangeReadOnlyObject);

            if ((uint)index >= (uint)Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            base.SetItem(index, item);
        }

        /// <summary>
        /// Changes the number of elements in the collection.
        /// </summary>
        /// <param name="newCount">New </param>
        /// <param name="createItem">Function which creates new item.</param>
        /// <remarks>
        /// If collection has more items than specified in <paramref name="newCount"/>,
        /// these items are removed. If collection has less items, new items are created
        /// using <paramref name="createItem"/> function.
        /// </remarks>
        public void SetCount(int newCount, Func<T> createItem)
        {
            for (int i = Count - 1; i >= newCount; i--)
                RemoveAt(i);

            for (int i = Count; i < newCount; i++)
                Add(createItem());
        }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>item</c> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, T item)
        {
            if (ThrowOnNullAdd && item is null)
                throw new ArgumentNullException(nameof(item));

            base.InsertItem(index, item);
            OnItemInserted(index, item);
        }

        /// <summary>
        /// Removes the item at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);
            OnItemRemoved(index, item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        /// <remarks>The base class calls this method when the list is being cleared.</remarks>
        protected override void ClearItems()
        {
            for (int i = Count - 1; i >= 0; i--)
                OnItemRemoved(i, this[i]);

            base.ClearItems();
        }

        /// <summary>
        /// Raises the <see cref="ItemRangeAdditionFinished"/> event with the provided arguments.
        /// </summary>
        /// <param name="insertionIndex">The index to insert the <paramref name="items"/> at.</param>
        /// <param name="items">The items being inserted.</param>
        protected virtual void OnItemRangeAdditionFinished(int insertionIndex, IEnumerable<T> items)
        {
            ItemRangeAdditionFinished?.Invoke(this, insertionIndex, items);
        }

        /// <summary>
        /// Raises the <see cref="ItemInserted"/> event with the provided arguments.
        /// </summary>
        /// <param name="index">The index where the change occurred.</param>
        /// <param name="item">The item affected by the change.</param>
        protected virtual void OnItemInserted(int index, T item)
        {
            ItemInserted?.Invoke(this, index, item);
        }

        /// <summary>
        /// Raises the <see cref="ItemRemoved"/> event with the provided arguments.
        /// </summary>
        /// <param name="index">The index where the change occurred.</param>
        /// <param name="item">The item affected by the change.</param>
        protected virtual void OnItemRemoved(int index, T item)
        {
            ItemRemoved?.Invoke(this, index, item);
        }
    }
}