using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="VirtualListBox.EditorTextRequested"/> event.
    /// </summary>
    public class ListBoxItemTextRequestEventArgs : ListBoxItemEventArgs
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ListBoxItemTextRequestEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item for which the event is raised.</param>
        /// <param name="itemIndex">The index of the item for which the event is raised.</param>
        /// <param name="text">The text of the item.</param>
        public ListBoxItemTextRequestEventArgs(
            ListControlItem item,
            int itemIndex,
            string? text = null)
            : base(item, itemIndex)
        {
            Text = text;
        }

        /// <summary>
        /// Gets or sets the text for the item editor.
        /// </summary>
        public string? Text { get; set; }
    }
}
