using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the events which are related to editing a tree view item's label.
    /// </summary>
    public class TreeViewEditEventArgs : BaseCancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewEditEventArgs"/> class.
        /// </summary>
        public TreeViewEditEventArgs(TreeViewItem item, string? label)
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
        /// <value>The string value that represents the new <see cref="TreeViewItem"/> label or
        /// <see langword="null"/>
        /// if the user cancels the edit.</value>
        public string? Label { get; }
    }
}