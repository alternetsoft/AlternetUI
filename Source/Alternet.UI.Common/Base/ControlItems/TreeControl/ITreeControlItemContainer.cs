using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Interface for a container that holds tree control items.
    /// </summary>
    public interface ITreeControlItemContainer
    {
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
        /// Called when an item is added to this tree view control, at
        /// any nesting level.
        /// </summary>
        void RaiseItemAdded(UI.TreeControlItem item);

        /// <summary>
        /// Called when an item is removed from this tree view control,
        /// at any nesting level.
        /// </summary>
        void RaiseItemRemoved(UI.TreeControlItem item);

        /// <summary>
        /// Called after the tree item is expanded.
        /// </summary>
        void RaiseAfterExpand(UI.TreeControlItem item);

        /// <summary>
        /// Called after the tree item is collapsed.
        /// </summary>
        void RaiseAfterCollapse(UI.TreeControlItem item);

        /// <summary>
        /// Called before the tree item is expanded.
        /// </summary>
        void RaiseBeforeExpand(UI.TreeControlItem item, ref bool cancel);

        /// <summary>
        /// Called before the tree item is collapsed.
        /// </summary>
        void RaiseBeforeCollapse(UI.TreeControlItem item, ref bool cancel);

        /// <summary>
        /// Called after 'IsExpanded' property
        /// value of a tree item belonging to this tree view changes.
        /// </summary>
        void RaiseExpandedChanged(UI.TreeControlItem item);
    }
}
