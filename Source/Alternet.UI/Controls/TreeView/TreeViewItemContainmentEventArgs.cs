using System;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for <see cref="TreeView.ItemAdded"/> and <see cref="TreeView.ItemRemoved"/> events.
    /// </summary>
    public class TreeViewItemContainmentEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemContainmentEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item being added or removed.</param>
        public TreeViewItemContainmentEventArgs(TreeViewItem item)
        {
            Item = item;
        }

        /// <summary>
        /// Gets the item that is being added or removed.
        /// </summary>
        /// <value>The item that is being added or removed.</value>
        /// <remarks>
        /// Use the <see cref="Item"/> property to access the properties of the item that is being added or removed.
        /// </remarks>
        public TreeViewItem Item { get; }
    }
}