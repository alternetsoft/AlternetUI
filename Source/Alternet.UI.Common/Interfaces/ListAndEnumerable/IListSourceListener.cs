using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface for a listener that can respond to changes in an <see cref="IListSource{TItem}"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the list.</typeparam>
    public interface IListSourceListener<TItem>
    {
        /// <summary>
        /// Called when an item is inserted into the list source.
        /// </summary>
        /// <param name="sender">The list source where the item was inserted.</param>
        /// <param name="index">The index at which the item was inserted.</param>
        /// <param name="item">The item that was inserted.</param>
        void OnListSourceItemInserted(IListSource<TItem> sender, int index, TItem item);

        /// <summary>
        /// Called when an item is removed from the list source.
        /// </summary>
        /// <param name="sender">The list source where the item was removed.</param>
        /// <param name="index">The index from which the item was removed.</param>
        /// <param name="item">The item that was removed.</param>
        void OnListSourceItemRemoved(IListSource<TItem> sender, int index, TItem item);

        /// <summary>
        /// Called when the collection changes.
        /// </summary>
        /// <param name="sender">The list source where the collection changed.</param>
        /// <param name="e">The event data describing the change.</param>
        void OnListSourceChanged(IListSource<TItem> sender, ListChangedEventArgs e);

        /// <summary>
        /// Called when a property of the list source changes.
        /// </summary>
        /// <param name="sender">The list source where the property changed.</param>
        /// <param name="e">The event data describing the property change.</param>
        void OnListSourcePropertyChanged(IListSource<TItem> sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Called when the list source begins an update operation.
        /// </summary>
        /// <param name="sender">The list source where the update operation began.</param>
        void OnListSourceUpdatesStarted(IListSource<TItem> sender);

        /// <summary>
        /// Called when the list source ends an update operation.
        /// </summary>
        /// <param name="sender">The list source where the update operation ended.</param>
        void OnListSourceUpdatesFinished(IListSource<TItem> sender);
    }
}
