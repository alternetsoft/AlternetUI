using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with the list view control.
    /// </summary>
    public interface IListViewHandler
    {
        /// <inheritdoc cref="ListView.SelectedIndices"/>
        long[] SelectedIndices { get; }

        /// <inheritdoc cref="ListView.ColumnHeaderVisible"/>
        bool ColumnHeaderVisible { get; set; }

        /// <summary>
        /// Gets or sets focused item index.
        /// </summary>
        long? FocusedItemIndex { get; set; }

        /// <inheritdoc cref="ListView.AllowLabelEdit"/>
        bool AllowLabelEdit { get; set; }

        /// <inheritdoc cref="ListView.TopItem"/>
        ListViewItem? TopItem { get; }

        /// <inheritdoc cref="ListView.GridLinesDisplayMode"/>
        ListViewGridLinesDisplayMode GridLinesDisplayMode { get; set; }

        /// <inheritdoc cref="ListView.HitTest"/>
        ListViewHitTestInfo HitTest(PointD point);

        /// <inheritdoc cref="ListView.BeginLabelEdit"/>
        void BeginLabelEdit(long itemIndex);

        /// <inheritdoc cref="ListView.GetItemBounds"/>
        RectD GetItemBounds(
            long itemIndex,
            ListViewItemBoundsPortion portion);

        /// <inheritdoc cref="ListView.Clear"/>
        void Clear();

        /// <summary>
        /// Ensures that item is visible.
        /// </summary>
        /// <param name="itemIndex">Item index.</param>
        void EnsureItemVisible(long itemIndex);

        /// <summary>
        /// Sets column width.
        /// </summary>
        /// <param name="columnIndex">Column index.</param>
        /// <param name="width">Column width.</param>
        /// <param name="widthMode">Width mode.</param>
        void SetColumnWidth(
            long columnIndex,
            Coord width,
            ListViewColumnWidthMode widthMode);

        /// <summary>
        /// Sets column title.
        /// </summary>
        /// <param name="columnIndex">Column index.</param>
        /// <param name="title">Column title.</param>
        void SetColumnTitle(long columnIndex, string title);

        /// <summary>
        /// Sets item text.
        /// </summary>
        /// <param name="itemIndex">Item index.</param>
        /// <param name="columnIndex">Column index.</param>
        /// <param name="text">Item text.</param>
        void SetItemText(long itemIndex, long columnIndex, string text);

        /// <summary>
        /// Sets item image.
        /// </summary>
        /// <param name="itemIndex">Item index.</param>
        /// <param name="columnIndex">Column index.</param>
        /// <param name="imageIndex">Image index.</param>
        void SetItemImageIndex(
            long itemIndex,
            long columnIndex,
            int? imageIndex);
    }
}
