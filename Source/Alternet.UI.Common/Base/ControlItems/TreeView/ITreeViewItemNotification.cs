using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Interface which allows receiving notifications from tree view items.
    /// </summary>
    public interface ITreeViewItemNotification
    {
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

        /// <summary>
        /// Called when an item is added to this tree view control, at
        /// any nesting level.
        /// </summary>
        /// <param name="item">The <see cref="UI.TreeViewItem"/> that was added.</param>
        void RaiseItemAdded(UI.TreeViewItem item);

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
    }
}
