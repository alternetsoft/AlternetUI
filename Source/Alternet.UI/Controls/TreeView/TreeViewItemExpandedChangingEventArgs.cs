using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for <see cref="TreeView.BeforeExpand"/>, <see cref="TreeView.BeforeCollapse"/>, <see cref="TreeView.ExpandedChanged"/> events.
    /// </summary>
    public class TreeViewItemExpandedChangingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemExpandedChangingEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item being expanded or collapsed.</param>
        public TreeViewItemExpandedChangingEventArgs(TreeViewItem item)
        {
            Item = item;
        }

        /// <summary>
        /// Gets the item that is being expanded or collapsed.
        /// </summary>
        /// <value>The item that is being expanded or collapsed.</value>
        /// <remarks>
        /// Use the <see cref="Item"/> property to access the properties of the item that is being expanded or collapsed.
        /// </remarks>
        public TreeViewItem Item { get; }
    }
}