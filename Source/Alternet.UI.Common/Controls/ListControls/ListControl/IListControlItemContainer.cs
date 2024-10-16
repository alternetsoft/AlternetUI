using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow <see cref="ListControlItem"/>
    /// to get information about it's container.
    /// </summary>
    public interface IListControlItemContainer
    {
        /// <summary>
        /// Gets defaults used when item is painted.
        /// </summary>
        IListControlItemDefaults Defaults { get; }

        /// <summary>
        /// Gets <see cref="ListControlItem"/> item with the specified index.
        /// If index of the item is invalid or item is not <see cref="ListControlItem"/>,
        /// returns <c>null</c>.
        /// </summary>
        ListControlItem? SafeItem(int index);

        /// <summary>
        /// Returns the text representation of item with the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Item index from which to get the contents to display.</param>
        string GetItemText(int index);

        /// <summary>
        /// Gets whether item with the specified index is current.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        bool IsCurrent(int index);

        /// <summary>
        /// Gets whether item with the specified index is selected.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        bool IsSelected(int index);
    }
}