using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the cell selection changed event of a control that has columns.
    /// </summary>
    public class CellSelectionEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellSelectionEventArgs"/> class with the specified parameters.
        /// </summary>
        /// <param name="item">The cell which selection state has changed.</param>
        /// <param name="selected">The new selection state of the cell.</param>
        public CellSelectionEventArgs(ListControlItem item, bool selected)
        {
            Item = item;
            Selected = selected;
        }

        /// <summary>
        /// Gets or sets the cell which selection state has changed.
        /// </summary>
        public ListControlItem Item { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell is selected.
        /// </summary>
        public bool Selected { get; set; }
    }
}
