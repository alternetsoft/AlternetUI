using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the cell click event of a list control that has columns.
    /// </summary>
    public class ListBoxCellClickEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxCellClickEventArgs"/> class with the specified parameters
        /// </summary>
        /// <param name="itemIndex">The zero-based index of the item that was clicked.</param>
        /// <param name="item">The item that was clicked.</param>
        /// <param name="column">The column that was clicked.</param>
        /// <param name="mouseEventArgs">The mouse event arguments.</param>
        public ListBoxCellClickEventArgs(
            int itemIndex,
            ListControlItem item,
            ListControlColumn column,
            MouseEventArgs mouseEventArgs)
        {
            ItemIndex = itemIndex;
            Item = item;
            Column = column;
            MouseEventArgs = mouseEventArgs;
        }

        /// <summary>
        /// Gets or sets the zero-based index of the item that was clicked.
        /// </summary>
        public int ItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the item that was clicked.
        /// </summary>
        public ListControlItem Item { get; set; }

        /// <summary>
        /// Gets or sets the mouse event arguments associated with the click event.
        /// </summary>
        public MouseEventArgs MouseEventArgs { get; set; }

        /// <summary>
        /// Gets or sets the column that was clicked.
        /// </summary>
        public ListControlColumn Column { get; set; }
    }
}
