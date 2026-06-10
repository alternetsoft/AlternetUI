using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface to work with a list of items.
    /// </summary>
    /// <typeparam name="TItem">Type of the items in the list.</typeparam>
    public interface IListSource<TItem> : IReadOnlyListSource<TItem>, IList<TItem>
    {
        /// <summary>
        /// Occurs when the list of items changes.
        /// </summary>
        event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the number of items in the list.
        /// </summary>
        /// <returns>
        /// The number of items in the list.
        /// </returns>
        new int Count { get; set;}

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to get.</param>
        /// <returns>The item at the specified index.</returns>
        new TItem this[int index] { get; }

        /// <summary>
        /// Sets size of the list.
        /// </summary>
        /// <param name="count">The new size of the list.</param>
        /// <param name="fnCreateItem">A function to create new items if the list is expanded.</param>
        void SetCount(int count, Func<TItem> fnCreateItem);

        /// <summary>
        /// Sets the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to set.</param>
        /// <param name="value">The item to set at the specified index.</param>
        void SetItem(int index, TItem value);

        /// <summary>
        /// Adds a range of items.
        /// </summary>
        /// <param name="items">The items to add.</param>
        void AddRange(IEnumerable<TItem> items);

        /// <summary>
        /// Sorts the items in the list.
        /// </summary>
        void Sort();
    }
}
