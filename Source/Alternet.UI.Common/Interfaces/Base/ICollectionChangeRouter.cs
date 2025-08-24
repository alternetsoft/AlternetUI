using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines methods to route collection change notifications such
    /// as add, remove, replace, move, and reset.
    /// Used in <see cref="ListUtils.RouteCollectionChange"/>.
    /// </summary>
    public interface ICollectionChangeRouter
    {
        /// <summary>
        /// Called when items are added to the collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="newItems">The items that were added.</param>
        /// <param name="newIndex">The index at which the new items were inserted.</param>
        void OnCollectionAdd(object? sender, IList newItems, int newIndex);

        /// <summary>
        /// Called when items are removed from the collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="oldItems">The items that were removed.</param>
        /// <param name="oldIndex">The index from which the items were removed.</param>
        void OnCollectionRemove(object? sender, IList oldItems, int oldIndex);

        /// <summary>
        /// Called when items in the collection are replaced.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="oldItems">The items that were replaced.</param>
        /// <param name="newItems">The new items that replaced the old items.</param>
        /// <param name="index">The index at which the replacement occurred.</param>
        void OnCollectionReplace(object? sender, IList oldItems, IList newItems, int index);

        /// <summary>
        /// Called when items are moved within the collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="movedItems">The items that were moved.</param>
        /// <param name="oldIndex">The original index of the moved items.</param>
        /// <param name="newIndex">The new index of the moved items.</param>
        void OnCollectionMove(object? sender, IList movedItems, int oldIndex, int newIndex);

        /// <summary>
        /// Called when the collection is reset.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        void OnCollectionReset(object? sender);
    }
}
