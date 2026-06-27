using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="VirtualListBox.ItemTextEdited"/> event.
    /// </summary>
    public class ListBoxItemTextEditedEventArgs : ListBoxItemEventArgs
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ListBoxItemTextEditedEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item for which the event is raised.</param>
        /// <param name="itemIndex">The index of the item for which the event is raised.</param>
        /// <param name="newText">The new text for the item editor.</param>
        public ListBoxItemTextEditedEventArgs(
            ListControlItem item,
            int itemIndex,
            string? newText)
            : base(item, itemIndex)
        {
            NewText = newText;
        }

        /// <summary>
        /// Gets or sets the new text of the item after editing.
        /// </summary>
        public virtual string? NewText { get; set; }

        /// <summary>
        /// Gets the new text of the item after editing or an empty string if the new text is null.
        /// </summary>
        public string NewTextOrDefault => NewText ?? string.Empty;
    }
}
