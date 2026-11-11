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
        void ItemInserted(IListSource<TItem> sender, int index, TItem item);

        /// <summary>
        /// Called when an item is removed from the list source.
        /// </summary>
        /// <param name="sender">The list source where the item was removed.</param>
        /// <param name="index">The index from which the item was removed.</param>
        /// <param name="item">The item that was removed.</param>
        void ItemRemoved(IListSource<TItem> sender, int index, TItem item);

        /// <summary>
        /// Called when the collection changes.
        /// </summary>
        /// <param name="sender">The list source where the collection changed.</param>
        /// <param name="e">The event data describing the change.</param>
        void Changed(IListSource<TItem> sender, ListChangedEventArgs e);

        /// <summary>
        /// Called when a property of an item in the list source changes.
        /// This method is only called if the item notifies its containing list source about property changes.
        /// </summary>
        /// <param name="sender">The list source where the item property changed.</param>
        /// <param name="item">The item whose property changed.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        void ItemPropertyChanged(IListSource<TItem> sender, TItem item, string? propertyName);

        /// <summary>
        /// Called when a property of an item in the list source is about to change.
        /// This method is only called if the item notifies its containing list source about property changes.
        /// </summary>
        /// <param name="sender">The list source where the item property is about to change.</param>
        /// <param name="item">The item whose property is about to change.</param>
        /// <param name="propertyName">The name of the property that is about to change.</param>
        void ItemPropertyChanging(IListSource<TItem> sender, TItem item, string? propertyName);

        /// <summary>
        /// Called to determine whether a property of an item in the list source can be changed.
        /// This method is only called if the item notifies its containing list source about property changes.
        /// </summary>
        /// <param name="sender">The list source where the item property is about to change.</param>
        /// <param name="item">The item whose property is about to change.</param>
        /// <param name="propertyName">The name of the property that is about to change.</param>
        /// <returns><c>true</c> if the property can be changed; otherwise, <c>false</c>.</returns>
        bool CanChangeItemProperty(IListSource<TItem> sender, TItem item, string propertyName);

        /// <summary>
        /// Called when a property of the list source changes.
        /// </summary>
        /// <param name="sender">The list source where the property changed.</param>
        /// <param name="e">The event data describing the property change.</param>
        void PropertyChanged(IListSource<TItem> sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Called when the list source begins an update operation.
        /// </summary>
        /// <param name="sender">The list source where the update operation began.</param>
        void UpdatesStarted(IListSource<TItem> sender);

        /// <summary>
        /// Called when the list source ends an update operation.
        /// </summary>
        /// <param name="sender">The list source where the update operation ended.</param>
        void UpdatesFinished(IListSource<TItem> sender);
    }

    /// <summary>
    /// A simple implementation of <see cref="IListSourceListener{TItem}"/> that logs events to a specified log writer.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the list source.</typeparam>
    public class ListSourceListenerLogger<TItem> : IListSourceListener<TItem>
    {
        private readonly ILogWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSourceListenerLogger{TItem}"/> class.
        /// </summary>
        /// <param name="writer">The log writer to use for logging. If null, <see cref="LogWriter.Debug"/> is used.</param>
        public ListSourceListenerLogger(ILogWriter? writer = null)
        {
            this.writer = writer ?? LogWriter.Debug;   
        }

        /// <inheritdoc/>
        public void Changed(IListSource<TItem> sender, ListChangedEventArgs e)
        {
            writer.WriteLine($"ListSource changed: {e.Action}");
        }

        /// <inheritdoc/>
        public void ItemInserted(IListSource<TItem> sender, int index, TItem item)
        {
            writer.WriteLine($"ListSource item inserted at index {index}: {item}");
        }

        /// <inheritdoc/>
        public void ItemRemoved(IListSource<TItem> sender, int index, TItem item)
        {
            writer.WriteLine($"ListSource item removed from index {index}: {item}");
        }

        /// <inheritdoc/>
        public void PropertyChanged(IListSource<TItem> sender, PropertyChangedEventArgs e)
        {
            writer.WriteLine($"ListSource property changed: {e.PropertyName}");
        }

        /// <inheritdoc/>
        public void UpdatesStarted(IListSource<TItem> sender)
        {
            writer.WriteLine("ListSource updates started.");
        }

        /// <inheritdoc/>
        public void UpdatesFinished(IListSource<TItem> sender)
        {
            writer.WriteLine("ListSource updates finished.");
        }

        /// <inheritdoc/>
        public void ItemPropertyChanged(IListSource<TItem> sender, TItem item, string? propertyName)
        {
            writer.WriteLine($"ListSource item property changed: {propertyName ?? "null"} on item {item}");
        }

        /// <inheritdoc/>
        public void ItemPropertyChanging(IListSource<TItem> sender, TItem item, string? propertyName)
        {
            writer.WriteLine($"ListSource item property changing: {propertyName ?? "null"} on item {item}");
        }

        /// <inheritdoc/>
        public bool CanChangeItemProperty(IListSource<TItem> sender, TItem item, string propertyName)
        {
            return true;
        }
    }
}
