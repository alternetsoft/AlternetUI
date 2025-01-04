using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with virtual list box control.
    /// </summary>
    public interface IVListBoxHandler
    {
        /// <inheritdoc cref="VirtualListControl.HasBorder"/>
        bool HasBorder { get; set; }

        /// <summary>
        /// Gets or sets items count.
        /// </summary>
        int ItemsCount { get; set; }

        /// <inheritdoc cref="VirtualListBox.HorizontalScrollbar"/>
        bool HScrollBarVisible { get; set; }

        /// <inheritdoc cref="VirtualListBox.VScrollBarVisible"/>
        public bool VScrollBarVisible { get; set; }

        /// <inheritdoc cref="VirtualListBox.GetItemRect"/>
        public RectD? GetItemRect(int index);

        /// <inheritdoc cref="VirtualListBox.ScrollRows(int)"/>
        bool ScrollRows(int rows);

        /// <inheritdoc cref="VirtualListBox.ScrollToRow(int)"/>
        bool ScrollToRow(int row);

        /// <inheritdoc cref="VirtualListBox.ScrollRowPages(int)"/>
        bool ScrollRowPages(int pages);

        /// <inheritdoc cref="VirtualListBox.RefreshRow(int)"/>
        void RefreshRow(int row);

        /// <inheritdoc cref="VirtualListBox.RefreshRows(int, int)"/>
        void RefreshRows(int from, int to);

        /// <inheritdoc cref="VirtualListBox.GetVisibleEnd"/>
        int GetVisibleEnd();

        /// <summary>
        /// Detaches events from items.
        /// </summary>
        void DetachItems(IListControlItems<ListControlItem> items);

        /// <summary>
        /// Attaches events to items.
        /// </summary>
        void AttachItems(IListControlItems<ListControlItem> items);

        /// <inheritdoc cref="VirtualListBox.GetVisibleBegin"/>
        int GetVisibleBegin();

        /// <inheritdoc cref="VirtualListBox.IsItemVisible(int)"/>
        bool IsVisible(int line);

        /// <summary>
        /// Gets hit text information for the specified point inside the control.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        int ItemHitTest(PointD position);

        /// <inheritdoc cref="VirtualListControl.EnsureVisible"/>
        void EnsureVisible(int itemIndex);

        /// <inheritdoc cref="VirtualListControl.HitTest"/>
        int? HitTest(PointD position);
    }
}
