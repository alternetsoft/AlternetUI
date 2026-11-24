using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface for a read-only list source that provides access to a collection of items.
    /// </summary>
    /// <typeparam name="TItem">Type of the items in the list.</typeparam>
    public interface IReadOnlyListSource<TItem> : IReadOnlyList<TItem>
    {
        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to retrieve.</param>
        /// <returns>The item at the specified index.</returns>
        TItem? GetItem(int index);
    }
}
