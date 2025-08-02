using System;

namespace Alternet.UI
{
    /// <summary>
    /// Contains information about the hit test
    /// result for a <see cref="TreeView"/> control.
    /// </summary>
    public class TreeViewHitTestInfo : BaseObject
    {
        /// <summary>
        /// Gets an empty <see cref="TreeViewHitTestInfo"/> object.
        /// </summary>
        public static readonly TreeViewHitTestInfo Empty = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewHitTestInfo"/> class.
        /// </summary>
        public TreeViewHitTestInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewHitTestInfo"/> class.
        /// </summary>
        public TreeViewHitTestInfo(TreeViewHitTestLocations location, TreeViewItem? item)
        {
            Location = location;
            Item = item;
        }

        /// <summary>
        /// Gets the location of a hit test on a <see cref="TreeView"/> control,
        /// in relation to the <see cref="TreeView"/> and the nodes it contains.
        /// </summary>
        public TreeViewHitTestLocations Location { get; }

        /// <summary>
        /// Gets the <see cref="TreeViewItem"/> at the position indicated by a hit test of a
        /// <see cref="TreeView"/> control.
        /// </summary>
        public TreeViewItem? Item { get; }
    }
}