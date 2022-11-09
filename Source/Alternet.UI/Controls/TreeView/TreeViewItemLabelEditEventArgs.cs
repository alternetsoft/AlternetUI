using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="TreeView.BeforeLabelEdit"/> and <see cref="TreeView.AfterLabelEdit"/> events.
    /// </summary>
    public class TreeViewItemLabelEditEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemLabelEditEventArgs"/> class.
        /// </summary>
        public TreeViewItemLabelEditEventArgs(TreeViewItem item, string? label)
        {
            Item = item;
            Label = label;
        }

        /// <summary>
        /// Gets the tree item containing the text to edit.
        /// </summary>
        public TreeViewItem Item { get; }

        /// <summary>
        /// Gets the new text to associate with the tree item.
        /// </summary>
        /// <value>The string value that represents the new <see cref="TreeViewItem"/> label or <see langword="null"/>
        /// if the user cancels the edit.</value>
        public string? Label { get; }
    }
}