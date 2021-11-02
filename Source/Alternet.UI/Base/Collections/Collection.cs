using System;
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