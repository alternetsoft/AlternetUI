using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="VirtualListBox.ItemTextEdited"/> event.
    /// </summary>
    public class ItemTextEditedEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemTextEditedEventArgs"/>
        /// class with the empty new text.
        /// </summary>
        public ItemTextEditedEventArgs()
        {
            NewText = string.Empty;
        }

        /// <summary>
        /// Gets or sets the index of the item for which the text was edited.
        /// </summary>
        public int ItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the item for which the text was edited.
        /// </summary>
        public ListControlItem? Item { get; set; }

        /// <summary>
        /// Gets or sets the new text of the item after editing.
        /// </summary>
        public virtual string NewText { get; set; }
    }
}
