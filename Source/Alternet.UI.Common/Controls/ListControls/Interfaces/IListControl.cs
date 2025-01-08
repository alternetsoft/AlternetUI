using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to some method and properties of the list controls.
    /// </summary>
    public interface IListControl : IControl
    {
        /// <summary>
        /// Gets or sets index of the selected item.
        /// </summary>
        int? SelectedIndex { get; set; }

        /// <summary>
        /// Adds new item to the control.
        /// </summary>
        /// <param name="item">Item to add.</param>
        void Add(ListControlItem item);

        /// <inheritdoc cref="StringSearch.FindStringExact(string)"/>
        int? FindStringExact(string s);

        /// <summary>
        /// Gets item with the specified index as object.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns></returns>
        object? GetItemAsObject(int index);
    }
}
