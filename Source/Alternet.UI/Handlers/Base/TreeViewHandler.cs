using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific
    /// <see cref="TreeView"/> behavior and appearance.
    /// </summary>
    public abstract class TreeViewHandler : ControlHandler
    {
        /// <inheritdoc cref="TreeView.HasBorder"/>
        public abstract bool HasBorder { get; set; }

        /// <summary>
        /// Gets a <see cref="TreeView"/> this handler provides the implementation for.
        /// </summary>
        public new TreeView Control => (TreeView)base.Control;

        /// <inheritdoc cref="TreeView.HideRoot"/>
        public abstract bool HideRoot { get; set; }

        /// <inheritdoc cref="TreeView.VariableRowHeight"/>
        public abstract bool VariableRowHeight { get; set; }

        /// <inheritdoc cref="TreeView.TwistButtons"/>
        public abstract bool TwistButtons { get; set; }

        /// <inheritdoc cref="TreeView.StateImageSpacing"/>
        public abstract uint StateImageSpacing { get; set; }

        /// <inheritdoc cref="TreeView.Indentation"/>
        public abstract uint Indentation { get; set; }

        /// <inheritdoc cref="TreeView.RowLines"/>
        public abstract bool RowLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lines are drawn between tree
        /// nodes in the tree view control.
        /// </summary>
        public abstract bool ShowLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lines are drawn between the
        /// tree nodes that are at the root of the tree view.
        /// </summary>
        public abstract bool ShowRootLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether expand buttons are displayed
        /// next to tree nodes that contain child
        /// tree nodes.
        /// </summary>
        public abstract bool ShowExpandButtons { get; set; }

        /// <summary>
        /// Gets or sets the first fully-visible tree item in the tree view control.
        /// </summary>
        public abstract TreeViewItem? TopItem { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the selection highlight spans
        /// the width of the tree view control.
        /// </summary>
        public abstract bool FullRowSelect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the label text of the tree
        /// nodes can be edited.
        /// </summary>
        public abstract bool AllowLabelEdit { get; set; }

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;

        /// <summary>
        /// Expands all child tree items.
        /// </summary>
        public abstract void ExpandAll();

        /// <summary>
        /// Collapses all child tree items.
        /// </summary>
        public abstract void CollapseAll();

        /// <summary>
        /// Provides tree view item information, at a given client point, in
        /// device-independent units (1/96th inch per
        /// unit).
        /// </summary>
        public abstract TreeViewHitTestInfo HitTest(Point point);

        /// <summary>
        /// Gets a value indicating whether the specified tree item is in the
        /// selected state.
        /// </summary>
        public abstract bool IsItemSelected(TreeViewItem treeViewItem);

        /// <summary>
        /// Initiates the editing of the tree node label.
        /// </summary>
        public abstract void BeginLabelEdit(TreeViewItem treeViewItem);

        /// <summary>
        /// Ends the editing of the tree node label.
        /// </summary>
        public abstract void EndLabelEdit(TreeViewItem treeViewItem, bool cancel);

        /// <summary>
        /// Expands this <see cref="TreeViewItem"/> and all the child tree items.
        /// </summary>
        public abstract void ExpandAllChildren(TreeViewItem treeViewItem);

        /// <summary>
        /// Collapses this <see cref="TreeViewItem"/> and all the child tree items.
        /// </summary>
        public abstract void CollapseAllChildren(TreeViewItem treeViewItem);

        /// <summary>
        /// Ensures that the tree item is visible, expanding tree items and
        /// scrolling the tree view control as
        /// necessary.
        /// </summary>
        public abstract void EnsureVisible(TreeViewItem treeViewItem);

        /// <summary>
        /// Scrolls the item into view.
        /// </summary>
        public abstract void ScrollIntoView(TreeViewItem treeViewItem);

        /// <summary>
        /// Sets a value indicating whether the tree item is in the focused state.
        /// </summary>
        public abstract void SetFocused(TreeViewItem treeViewItem, bool value);

        /// <summary>
        /// Gets a value indicating whether the tree item is in the focused state.
        /// </summary>
        public abstract bool IsItemFocused(TreeViewItem treeViewItem);

        /// <summary>
        /// Sets the specified item text.
        /// </summary>
        public abstract void SetItemText(TreeViewItem treeViewItem, string text);

        /// <summary>
        /// Sets the specified item image index.
        /// </summary>
        public abstract void SetItemImageIndex(
            TreeViewItem treeViewItem,
            int? imageIndex);
    }
}