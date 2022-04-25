using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Alternet.Base.Collections
{
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
        public event EventHandler<CollectionChangeEventArgs<T>>? ItemInserted;

        /// <summary>
        /// Occurs when an item is removed from the collection.
        /// </summary>
        public event EventHandler<CollectionChangeEventArgs<T>>? ItemRemoved;

        /// <summary>
        /// Occurs when an item range addition is finished in the collection.
        /// </summary>
        public event EventHandler<RangeAdditionFinishedEventArgs<T>>? ItemRangeAdditionFinished;

        /// <summary>
        /// Returns <see langword="true"/> if <see cref="AddRange"/> is being executed at the moment.
        /// </summary>
        public bool RangeOperationInProgress { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether an <see cref="ArgumentNullException"/> should be thrown
        /// on an attempt to add a <c>null</c> value to the collection.
        /// </summary>
        public bool ThrowOnNullItemAddition { get; set; }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>item</c> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, T item)
        {
            if (ThrowOnNullItemAddition && item is null)
                throw new ArgumentNullException(nameof(item), "Adding null to the collection is not allowed.");

            base.InsertItem(index, item);

            OnItemInserted(new CollectionChangeEventArgs<T>(index, item));
        }

        /// <summary>
        /// Adds the elements of the specified <see cref="IEnumerable{T}"/> to the end of the collection.
        /// </summary>
        /// <param name="collection">
        /// The <see cref="IEnumerable{T}"/> whose elements should be added to the end of the collection.
        /// The value itself cannot be <c>null</c>, but it can contain elements that are <c>null</c>, if type <c>T</c> is a reference type.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// <c>collection</c> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// The order of the elements in the <see cref="IEnumerable{T}"/> is preserved in the collection.
        /// </remarks>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));

            RangeOperationInProgress = true;

            int index = Count;
            try
            {
                foreach (var item in collection)
                    Add(item);
            }
            finally
            {
                RangeOperationInProgress = false;
                OnItemRangeAdditionFinished(new RangeAdditionFinishedEventArgs<T>(index, collection));
            }
        }

        /// <summary>
        /// Removes the item at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);

            OnItemRemoved(new CollectionChangeEventArgs<T>(index, item));
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        /// <remarks>The base class calls this method when the list is being cleared.</remarks>
        protected override void ClearItems()
        {
            for (int i = 0; i < Count; i++)
                OnItemRemoved(new CollectionChangeEventArgs<T>(i, this[i]));

            base.ClearItems();
        }

        /// <summary>
        /// Raises the <see cref="ItemRangeAdditionFinished"/> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnItemRangeAdditionFinished(RangeAdditionFinishedEventArgs<T> e) => ItemRangeAdditionFinished?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="ItemInserted"/> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnItemInserted(CollectionChangeEventArgs<T> e) => ItemInserted?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="ItemRemoved"/> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnItemRemoved(CollectionChangeEventArgs<T> e) => ItemRemoved?.Invoke(this, e);
    }
}