using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Interface for a container that holds tree view items.
    /// </summary>
    public interface ITreeViewItemContainer
    {
        /// <summary>
        /// Gets the container that manages the list control items.
        /// </summary>
        IListControlItemContainer ListContainer { get; }

        /// <summary>
        /// Gets the type of buttons used in the tree view.
        /// </summary>
        TreeViewButtonsKind TreeButtons { get; }

        /// <summary>
        /// Gets margin for each level in the tree.
        /// </summary>
        /// <returns>The margin for each level in the tree.</returns>
        Coord GetLevelMargin();

        /// <summary>
        /// Repaints the tree view control. Uses message queue.
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Repaints the tree view control immediately.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Begins an update of the tree view. Call this method before making
        /// multiple changes to the tree view to prevent multiple refreshes.
        /// </summary>
        void BeginUpdate();

        /// <summary>
        /// Ends an update of the tree view. Call this method after making
        /// multiple changes to the tree view to refresh the view.
        /// </summary>
        /// <exception cref="Exception">Thrown if EndUpdate is called without
        /// a preceding BeginUpdate call.</exception>
        void EndUpdate();

        /// <summary>
        /// Notifies the container that the structure of the tree has changed.
        /// </summary>
        /// <remarks>
        /// This method should be called in some cases when the tree's structure is modified.
        /// It ensures that the tree view is updated to
        /// reflect the changes.
        /// </remarks>
        void TreeChanged();

        /// <summary>
        /// Called when an item is added to this tree view control, at
        /// any nesting level.
        /// </summary>
        /// <param name="item">The <see cref="UI.TreeViewItem"/> that was added.</param>
        void RaiseItemAdded(UI.TreeViewItem item);

        /// <summary>
        /// Ensures that the tree item is visible, expanding tree items and
        /// scrolling the tree view control as necessary.
        /// </summary>
        /// <remarks>
        /// When this method is called, the tree is
        /// expanded and scrolled to ensure that the current tree
        /// item is visible in the control. This method is useful
        /// if you are selecting a tree item in code based on
        /// certain criteria. By calling this method after you select the item,
        /// the user can see and interact with the selected item.
        /// </remarks>
        void EnsureVisible(TreeViewItem? item);

        /// <summary>
        /// Scrolls control so the specified item will be fully visible.
        /// </summary>
        /// <param name="item">The <see cref="TreeViewItem"/> to show into the view. Can be null.</param>
        void ScrollIntoView(TreeViewItem? item);

        /// <summary>
        /// Called when an item is removed from this tree view control,
        /// at any nesting level.
        /// </summary>
        /// <param name="item">The <see cref="UI.TreeViewItem"/> that was removed.</param>
        void RaiseItemRemoved(UI.TreeViewItem item);

        /// <summary>
        /// Called after the tree item is expanded.
        /// </summary>
        /// <param name="item">The <see cref="TreeViewItem"/> that was expanded.</param>
        void RaiseAfterExpand(UI.TreeViewItem item);

        /// <summary>
        /// Called after the tree item is collapsed.
        /// </summary>
        /// <param name="item">The <see cref="TreeViewItem"/> that was collapsed.</param>
        void RaiseAfterCollapse(UI.TreeViewItem item);

        /// <summary>
        /// Called when a tree item is selected or deselected.
        /// </summary>
        /// <param name="item">The <see cref="TreeViewItem"/> that was selected or deselected.</param>
        /// <param name="selected">Indicates whether the item is selected.</param>
        void RaiseItemSelectedChanged(UI.TreeViewItem item, bool selected);

        /// <summary>
        /// Called before the tree item is expanded.
        /// </summary>
        /// <param name="item">The <see cref="TreeViewItem"/> that is about to be expanded.</param>
        /// <param name="cancel">Set to true to cancel the expand operation.</param>
        void RaiseBeforeExpand(UI.TreeViewItem item, ref bool cancel);

        /// <summary>
        /// Called before the tree item is collapsed.
        /// </summary>
        /// <param name="item">The <see cref="TreeViewItem"/> that is about to be collapsed.</param>
        /// <param name="cancel">Set to true to cancel the collapse operation.</param>
        void RaiseBeforeCollapse(UI.TreeViewItem item, ref bool cancel);

        /// <summary>
        /// Called after 'IsExpanded' property
        /// value of a tree item belonging to this tree view changes.
        /// </summary>
        /// <param name="item">The <see cref="TreeViewItem"/> whose expanded state changed.</param>
        void RaiseExpandedChanged(UI.TreeViewItem item);

        /// <summary>
        /// Called when a property of a tree item changes.
        /// </summary>
        /// <param name="item">The tree item whose property changed.</param>
        /// <param name="propertyName">The name of the property that changed. Can be null.</param>
        void RaiseItemPropertyChanged(UI.TreeViewItem item, string? propertyName);
    }
}
