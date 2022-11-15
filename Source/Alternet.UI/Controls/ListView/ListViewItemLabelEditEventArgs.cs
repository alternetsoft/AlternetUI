using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="ListView.BeforeLabelEdit"/> and <see cref="ListView.AfterLabelEdit"/> events.
    /// </summary>
    public class ListViewItemLabelEditEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItemLabelEditEventArgs"/> class.
        /// </summary>
        public ListViewItemLabelEditEventArgs(int itemIndex, string? label)
        {
            ItemIndex = itemIndex;
            Label = label;
        }

        /// <summary>
        /// Gets the zero-based index of the <see cref="ListViewItem"/> containing the label to edit.
        /// </summary>
        public int ItemIndex { get; }

        /// <summary>
        /// Gets the new text to associate with the list item.
        /// </summary>
        /// <value>The string value that represents the new <see cref="ListViewItem"/> label or <see langword="null"/>
        /// if the user cancels the edit.</value>
        public string? Label { get; }
    }
}