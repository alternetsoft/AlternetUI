using System;

namespace Alternet.UI
{
    /// <summary>
    /// Contains information about <see cref="ListView.HitTest(Alternet.Drawing.PointD)"/> result for a <see cref="ListView"/> control.
    /// </summary>
    public class ListViewHitTestInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewHitTestInfo"/> class.
        /// </summary>
        public ListViewHitTestInfo(ListViewHitTestLocations location, ListViewItem? item, ListViewItemCell? cell)
        {
            Location = location;
            Item = item;
            Cell = cell;
        }

        /// <summary>
        /// Gets the location of a hit test on a <see cref="ListView"/> control, in relation to the
        /// <see cref="ListView"/> and the nodes it contains.
        /// </summary>
        public ListViewHitTestLocations Location { get; }

        /// <summary>
        /// Gets the <see cref="ListViewItem"/> at the position indicated by a hit test of a <see cref="ListView"/>
        /// control.
        /// </summary>
        public ListViewItem? Item { get; }

        /// <summary>
        /// Gets the <see cref="ListViewItemCell"/> at the position indicated by a hit test of a <see cref="ListView"/>
        /// control.
        /// </summary>
        public ListViewItemCell? Cell { get; }
    }
}