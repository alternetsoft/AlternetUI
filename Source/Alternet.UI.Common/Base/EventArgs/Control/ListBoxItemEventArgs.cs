using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the events related to a list control item.
    /// Includes the item itself and its index in the list control.
    /// </summary>
    public class ListBoxItemEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ListBoxItemEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item for which the event is raised.</param>
        /// <param name="itemIndex">The index of the item for which the event is raised.</param>
        public ListBoxItemEventArgs(ListControlItem item, int itemIndex)
        {
            Item = item;
            ItemIndex = itemIndex;
        }

        /// <summary>
        /// Gets or sets the index of the item for which the event is raised.
        /// </summary>
        public int ItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the item for which the event is raised.
        /// </summary>
        public ListControlItem Item { get; set; }
    }
}
