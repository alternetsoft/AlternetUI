using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with tree view control.
    /// </summary>
    public interface ITreeViewHandler
    {
        /// <inheritdoc cref="TreeView.HideRoot"/>
        bool HideRoot { get; set; }

        /// <inheritdoc cref="TreeView.VariableRowHeight"/>
        bool VariableRowHeight { get; set; }

        /// <inheritdoc cref="TreeView.TwistButtons"/>
        bool TwistButtons { get; set; }

        /// <inheritdoc cref="TreeView.StateImageSpacing"/>
        uint StateImageSpacing { get; set; }

        /// <inheritdoc cref="TreeView.Indentation"/>
        public uint Indentation { get; set; }

        /// <inheritdoc cref="TreeView.RowLines"/>
        bool RowLines { get; set; }

        /// <inheritdoc cref="TreeView.ShowLines"/>
        bool ShowLines { get; set; }

        /// <inheritdoc cref="TreeView.ShowRootLines"/>
        bool ShowRootLines { get; set; }

        /// <inheritdoc cref="TreeView.ShowExpandButtons"/>
        bool ShowExpandButtons { get; set; }

        /// <inheritdoc cref="TreeView.TopItem"/>
        TreeViewItem? TopItem { get; }

        /// <inheritdoc cref="TreeView.FullRowSelect"/>
        bool FullRowSelect { get; set; }

        /// <inheritdoc cref="TreeView.AllowLabelEdit"/>
        bool AllowLabelEdit { get; set; }

        /// <inheritdoc cref="TreeView.ExpandAll"/>
        void ExpandAll();

        /// <inheritdoc cref="TreeView.CollapseAll"/>
        void CollapseAll();

        /// <summary>
        /// Provides tree view item information, at a given client point, in
        /// device-independent units.
        /// </summary>
        /// <param name="point">The <see cref="PointD"/> at which to retrieve
        /// item information.</param>
        /// <returns><c>true</c> if <paramref name="item"/> or <paramref name="locations"/>
        /// is not empty; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// Use this method to determine whether a point is located in a
        /// <see cref="TreeViewItem"/> and where within the
        /// item the point is located, such as on the label or image area.
        /// </remarks>
        /// <param name="item">Output parameter. Returns item at the specified point.</param>
        /// <param name="locations">Output parameter. Returns point location.</param>
        /// <param name="needItem">Whether to get <paramref name="item"/> parameter.</param>
        /// <returns></returns>
        public bool HitTest(
            PointD point,
            out TreeViewItem? item,
            out TreeViewHitTestLocations locations,
            bool needItem = true);

        /// <summary>
        /// Gets whether item is selected.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns></returns>
        bool IsItemSelected(TreeViewItem item);

        /// <inheritdoc cref="TreeViewItem.BeginLabelEdit"/>
        void BeginLabelEdit(TreeViewItem item);

        /// <inheritdoc cref="TreeViewItem.EndLabelEdit"/>
        void EndLabelEdit(TreeViewItem item, bool cancel);

        /// <summary>
        /// Expands all children of the specified item.
        /// </summary>
        /// <param name="item">Item.</param>
        void ExpandAllChildren(TreeViewItem item);

        /// <summary>
        /// Collapses all children of the specified item.
        /// </summary>
        /// <param name="item">Item.</param>
        void CollapseAllChildren(TreeViewItem item);

        /// <inheritdoc cref="TreeView.MakeAsListBox"/>
        void MakeAsListBox();

        /// <inheritdoc cref="TreeView.EnsureVisible"/>
        void EnsureVisible(TreeViewItem item);

        /// <inheritdoc cref="TreeView.ScrollIntoView"/>
        void ScrollIntoView(TreeViewItem item);

        /// <summary>
        /// Sets focused state for the item.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="value">Focused state.</param>
        void SetFocused(TreeViewItem item, bool value);

        /// <summary>
        /// Checks whether or not item is focused.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns></returns>
        bool IsItemFocused(TreeViewItem item);

        /// <summary>
        /// Sets item's bold state.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="isBold">Bold state.</param>
        void SetItemIsBold(TreeViewItem item, bool isBold);

        /// <summary>
        /// Sets background color of the item.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="color">Background color.</param>
        void SetItemBackgroundColor(TreeViewItem item, Color? color);

        /// <summary>
        /// Sets foreground color of the item.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="color">Foreground color.</param>
        void SetItemTextColor(TreeViewItem item, Color? color);

        /// <summary>
        /// Sets item text.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="text">Item text.</param>
        void SetItemText(TreeViewItem item, string text);

        /// <summary>
        /// Sets item image index.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="imageIndex">Image index.</param>
        void SetItemImageIndex(TreeViewItem item, int? imageIndex);
    }
}
