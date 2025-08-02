using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for 'BeforeExpand', 'BeforeCollapse', 'ExpandedChanged'
    /// and other cancelable events.
    /// </summary>
    public class TreeControlCancelEventArgs : BaseCancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeControlCancelEventArgs"/>
        /// class.
        /// </summary>
        /// <param name="item">The item being affected.</param>
        public TreeControlCancelEventArgs(TreeViewItem item)
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