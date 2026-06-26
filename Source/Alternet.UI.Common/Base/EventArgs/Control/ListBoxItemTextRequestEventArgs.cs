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
        /// Gets or sets the text for the item editor.
        /// </summary>
        public string? Text { get; set; }
    }
}
