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
        /// Initializes a new instance of the <see cref="ListBoxItemTextEditedEventArgs"/>
        /// class with the empty new text.
        /// </summary>
        public ListBoxItemTextEditedEventArgs()
        {
            NewText = string.Empty;
        }

        /// <summary>
        /// Gets or sets the new text of the item after editing.
        /// </summary>
        public virtual string NewText { get; set; }
    }
}
