using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IListViewHandler : IControlHandler
    {
        long[] SelectedIndices { get; }

        bool ColumnHeaderVisible { get; set; }

        bool HasBorder { get; set; }

        long? FocusedItemIndex { get; set; }

        bool AllowLabelEdit { get; set; }

        ListViewItem? TopItem { get; }

        ListViewGridLinesDisplayMode GridLinesDisplayMode { get; set; }

        ListViewHitTestInfo HitTest(PointD point);

        void BeginLabelEdit(long itemIndex);

        RectD GetItemBounds(
            long itemIndex,
            ListViewItemBoundsPortion portion);

        void Clear();

        void EnsureItemVisible(long itemIndex);

        void SetColumnWidth(
            long columnIndex,
            double width,
            ListViewColumnWidthMode widthMode);

        void SetColumnTitle(long columnIndex, string title);

        void SetItemText(long itemIndex, long columnIndex, string text);

        void SetItemImageIndex(
            long itemIndex,
            long columnIndex,
            int? imageIndex);
    }
}
