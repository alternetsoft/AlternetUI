using System;
using System.Collections.Generic;
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
        /// Gets or sets the number of items in the list.
        /// </summary>
        /// <returns>
        /// The number of items in the list.
        /// </returns>
        new int Count { get; set;}

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
    }
}
