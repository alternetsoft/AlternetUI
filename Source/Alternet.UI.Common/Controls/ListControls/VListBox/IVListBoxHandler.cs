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
    public interface IVListBoxHandler : IListBoxHandler
    {
        /// <summary>
        /// Gets or sets items count.
        /// </summary>
        int ItemsCount { get; set; }

        /// <inheritdoc cref="VListBox.HScrollBarVisible"/>
        bool HScrollBarVisible { get; set; }

        /// <inheritdoc cref="VListBox.VScrollBarVisible"/>
        public bool VScrollBarVisible { get; set; }

        /// <summary>
        /// Gets or sets selection mode.
        /// </summary>
        public ListBoxSelectionMode SelectionMode { get; set; }

        /// <inheritdoc cref="VListBox.GetItemRect"/>
        public RectD? GetItemRect(int index);

        /// <inheritdoc cref="VListBox.ScrollRows(int)"/>
        bool ScrollRows(int rows);

        /// <inheritdoc cref="VListBox.ScrollRowPages(int)"/>
        bool ScrollRowPages(int pages);

        /// <inheritdoc cref="VListBox.RefreshRow(int)"/>
        void RefreshRow(int row);

        /// <inheritdoc cref="VListBox.RefreshRows(int, int)"/>
        void RefreshRows(int from, int to);

        /// <inheritdoc cref="VListBox.GetVisibleEnd"/>
        int GetVisibleEnd();

        /// <inheritdoc cref="VListBox.GetVisibleBegin"/>
        int GetVisibleBegin();

        /// <inheritdoc cref="VListBox.IsSelected(int)"/>
        bool IsSelected(int line);

        /// <inheritdoc cref="VListBox.IsVisible(int)"/>
        bool IsVisible(int line);

        /// <summary>
        /// Clears items.
        /// </summary>
        void ClearItems();

        /// <summary>
        /// Clears selected items.
        /// </summary>
        void ClearSelected();

        /// <summary>
        /// Sets whether or not item with the specified index is selected.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="value">Item selected state.</param>
        void SetSelected(int index, bool value);

        /// <summary>
        /// Gets first selected item.
        /// </summary>
        /// <returns></returns>
        int GetFirstSelected();

        /// <summary>
        /// Gets next selected item.
        /// </summary>
        /// <returns></returns>
        int GetNextSelected();

        /// <summary>
        /// Gets number of selected items.
        /// </summary>
        /// <returns></returns>
        int GetSelectedCount();

        /// <summary>
        /// Gets selected item index.
        /// </summary>
        /// <returns></returns>
        int GetSelection();

        /// <summary>
        /// Gets hit text information for the specified point inside the control.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        int ItemHitTest(PointD position);

        /// <summary>
        /// Sets selected item index.
        /// </summary>
        /// <returns></returns>
        void SetSelection(int selection);

        /// <summary>
        /// Sets selected items bacckground color.
        /// </summary>
        /// <returns></returns>
        void SetSelectionBackground(Color color);

        /// <summary>
        /// Gets whether or not the specified item is current.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        bool IsCurrent(int current);

        /// <summary>
        /// Sets current item index.
        /// </summary>
        /// <param name="current">Index of the current item.</param>
        /// <returns></returns>
        bool DoSetCurrent(int current);
    }
}
