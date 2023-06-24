using System;
using System.Collections.Generic;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="ListView"/> behavior and appearance.
    /// </summary>
    public abstract class ListViewHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new ListView Control => (ListView)base.Control;

        /// <summary>
        /// Gets or sets a value indicating whether the label text of the items can be edited.
        /// </summary>
        public abstract bool AllowLabelEdit { get; set; }

        /// <summary>
        /// Gets or sets the first fully-visible item in the list view control.
        /// </summary>
        /// <value>A <see cref="ListViewItem"/> that represents the first fully-visible item in the list view control.</value>
        public abstract ListViewItem? TopItem { get; }

        /// <summary>
        /// Gets or sets the grid line display mode for this list view.
        /// </summary>
        public abstract ListViewGridLinesDisplayMode GridLinesDisplayMode { get; set; }

        /// <summary>
        /// Provides list view item information, at a given client point, in device-independent units (1/96th inch per
        /// unit).
        /// </summary>
        /// <param name="point">The <see cref="Point"/> at which to retrieve item information.</param>
        /// <returns>The hit test result information.</returns>
        /// <remarks>
        /// Use this method to determine whether a point is located in a <see cref="ListViewItem"/> and where within the
        /// item the point is located, such as on the label or image area.
        /// </remarks>
        public abstract ListViewHitTestInfo HitTest(Point point);

        /// <summary>
        /// Initiates the editing of the list view item label.
        /// </summary>
        /// <param name="itemIndex">The zero-based index of the item within the <see cref="ListView.Items"/> collection
        /// whose label you want to edit.</param>
        public abstract void BeginLabelEdit(int itemIndex);

        /// <summary>
        /// Retrieves the bounding rectangle for an item within the control.
        /// </summary>
        /// <param name="itemIndex">The zero-based index of the item within the <see cref="ListView.Items"/> collection
        /// whose bounding rectangle you want to get.</param>
        /// <param name="portion">One of the <see cref="ListViewItemBoundsPortion"/> values that represents a portion of
        /// the item for which to retrieve the bounding rectangle.</param>
        /// <returns>A <see cref="Rect"/> that represents the bounding rectangle for the specified portion of the
        /// specified <see cref="ListViewItem"/>.</returns>
        public abstract Rect GetItemBounds(int itemIndex, ListViewItemBoundsPortion portion);

        ///// <summary>
        ///// Gets or sets the custom sorting comparer for the control.
        ///// </summary>
        // public abstract IComparer<ListViewItem>? CustomItemSortComparer { get; set; }

        ///// <summary>
        ///// Gets or sets the sort mode for items in the control.
        ///// </summary>
        ///// <value>One of the <see cref="ListViewSortMode"/> values. The default is <see cref="ListViewSortMode.None"/>.</value>
        // public abstract ListViewSortMode SortMode { get; set; }

        /// <summary>
        /// Removes all items and columns from the control.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Gets or sets a boolean value which specifies whether the column header is visible in <see
        /// cref="ListViewView.Details"/> view.
        /// </summary>
        public abstract bool ColumnHeaderVisible { get; set; }

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;

        /// <summary>
        /// Ensures that the specified item is visible within the control, scrolling the contents of the control, if necessary.
        /// </summary>
        public abstract void EnsureItemVisible(int itemIndex);

        /// <summary>
        /// Gets or sets an index of the focused item within the <see cref="ListView"/> control.
        /// </summary>
        public abstract int? FocusedItemIndex { get; set; }

        /// <summary>
        /// Gets or sets a list view item text.
        /// </summary>
        public abstract void SetItemText(int itemIndex, int columnIndex, string text);

        /// <summary>
        /// Gets or sets a list view item image index.
        /// </summary>
        public abstract void SetItemImageIndex(int itemIndex, int columnIndex, int? imageIndex);

        /// <summary>
        /// Gets or sets a list view item column width.
        /// </summary>
        public abstract void SetColumnWidth(int columnIndex, double width, ListViewColumnWidthMode widthMode);

        /// <summary>
        /// Gets or sets a list view item column title.
        /// </summary>
        public abstract void SetColumnTitle(int columnIndex, string title);
    }
}