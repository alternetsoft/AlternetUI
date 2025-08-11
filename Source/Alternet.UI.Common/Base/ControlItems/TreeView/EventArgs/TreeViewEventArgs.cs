using System;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the events related to tree view items.
    /// </summary>
    public class TreeViewEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item being affected.</param>
        public TreeViewEventArgs(TreeViewItem item)
        {
            Item = item;
        }

        /// <summary>
        /// Gets the item that is being affected.
        /// </summary>
        /// <value>The item that is being affected.</value>
        /// <remarks>
        /// Use the <see cref="Item"/> property to access the properties of the item.
        /// </remarks>
        public TreeViewItem Item { get; }
    }
}