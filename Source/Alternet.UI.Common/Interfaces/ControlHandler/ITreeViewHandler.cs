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
        /// <summary>
        /// Gets or sets a value indicating whether the root element should be hidden.
        /// </summary>
        bool HideRoot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether rows in the control can have variable heights.
        /// </summary>
        bool VariableRowHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the twist buttons are enabled.
        /// </summary>
        bool TwistButtons { get; set; }

        /// <summary>
        /// Gets or sets the spacing between state images and other item parts.
        /// </summary>
        uint StateImageSpacing { get; set; }

        /// <summary>
        /// Gets or sets the indentation of the item.
        /// </summary>
        public uint Indentation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether horizontal lines are displayed between rows.
        /// </summary>
        bool RowLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether horizontal lines are displayed in the control.
        /// </summary>
        bool ShowLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lines are displayed between the root nodes
        /// of a tree structure.
        /// </summary>
        bool ShowRootLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether expand buttons are displayed.
        /// </summary>
        bool ShowExpandButtons { get; set; }

        /// <inheritdoc cref="StdTreeView.TopItem"/>
        TreeViewItem? TopItem { get; }

        /// <summary>
        /// Gets or sets a value indicating whether selecting a row highlights the entire row.
        /// </summary>
        bool FullRowSelect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether label editing is allowed.
        /// </summary>
        bool AllowLabelEdit { get; set; }

        /// <inheritdoc cref="StdTreeView.ExpandAll"/>
        void ExpandAll();

        /// <inheritdoc cref="StdTreeView.CollapseAll"/>
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

        /// <summary>
        /// Initiates the label editing mode for the specified <see cref="TreeViewItem"/>.
        /// </summary>
        /// <remarks>This method allows the user to edit the label of the specified tree view item.
        /// Ensure that the item supports label editing before calling this method.</remarks>
        /// <param name="item">The <see cref="TreeViewItem"/> whose label is to be edited.
        /// Must not be <see langword="null"/>.</param>
        void BeginLabelEdit(TreeViewItem item);

        /// <summary>
        /// Ends the label editing process for the specified <see cref="TreeViewItem"/>.
        /// </summary>
        /// <remarks>If <paramref name="cancel"/> is <see langword="true"/>, any modifications to the
        /// label are discarded. If <paramref name="cancel"/> is <see langword="false"/>,
        /// the changes are applied to the
        /// <paramref name="item"/>.</remarks>
        /// <param name="item">The <see cref="TreeViewItem"/> whose label editing
        /// is being finalized. Cannot be <see langword="null"/>.</param>
        /// <param name="cancel"><see langword="true"/> to cancel the label editing
        /// and discard changes; otherwise, <see langword="false"/>
        /// to apply the changes made during editing.</param>
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

        /// <inheritdoc cref="StdTreeView.MakeAsListBox"/>
        void MakeAsListBox();

        /// <summary>
        /// Ensures that the specified <see cref="TreeViewItem"/> is visible within the tree view.
        /// </summary>
        /// <remarks>If the specified item is not currently visible, this method scrolls
        /// the tree view as
        /// needed to bring the item into view.</remarks>
        /// <param name="item">The <see cref="TreeViewItem"/> to make visible.
        /// Must not be <see langword="null"/>.</param>
        void EnsureVisible(TreeViewItem item);

        /// <inheritdoc cref="StdTreeView.ScrollIntoView"/>
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
