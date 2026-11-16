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
        /// Called when a transaction on an item in the list source has ended.
        /// </summary>
        /// <param name="sender">The list source where the item transaction ended.</param>
        /// <param name="item">The item whose transaction ended.</param>
        void ItemTransactionEnded(IListSource<TItem> sender, TItem item);

        /// <summary>
        /// Called when a transaction on an item in the list source has started.
        /// </summary>
        /// <param name="sender">The list source where the item transaction started.</param>
        /// <param name="item">The item whose transaction started.</param>
        void ItemTransactionStarted(IListSource<TItem> sender, TItem item);

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
    public class ListSourceListener<TItem> : IListSourceListener<TItem>
    {
        /// <summary>
        /// Defines a delegate that handles property change notifications from an item source.
        /// </summary>
        /// <param name="sender">The item source that raised the notification.</param>
        /// <param name="e">The event data describing the property change.</param>
        public delegate void PropertyChangedDelegate(IListSource<TItem> sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Defines a delegate that handles collection change notifications from an item source.
        /// </summary>
        /// <param name="sender">The item source that raised the notification.</param>
        public delegate void UpdatesStartedDelegate(IListSource<TItem> sender);

        /// <summary>
        /// Defines a delegate that handles collection change notifications from an item source.
        /// </summary>
        /// <param name="sender">The item source that raised the notification.</param>
        public delegate void UpdatesFinishedDelegate(IListSource<TItem> sender);

        /// <summary>
        /// Defines a delegate that handles item property change notifications from an item source.
        /// </summary>
        /// <param name="sender">The item source that raised the notification.</param>
        /// <param name="index">The index of the item that was inserted.</param>
        /// <param name="item">The item that was inserted.</param>
        public delegate void ItemInsertedDelegate(IListSource<TItem> sender, int index, TItem item);

        /// <summary>
        /// Defines a delegate that handles item removal notifications from an item source.
        /// </summary>
        /// <param name="sender">The item source that raised the notification.</param>
        /// <param name="index">The index of the item that was removed.</param>
        /// <param name="item">The item that was removed.</param>
        public delegate void ItemRemovedDelegate(IListSource<TItem> sender, int index, TItem item);

        /// <summary>
        /// Defines a delegate that handles collection change notifications from an item source.
        /// </summary>
        /// <param name="sender">The item source that raised the notification.</param>
        /// <param name="e">The event data describing the change.</param>
        public delegate void ChangedDelegate(IListSource<TItem> sender, ListChangedEventArgs e);

        /// <summary>
        /// Defines a delegate that handles item property change notifications from an item source.
        /// </summary>
        /// <param name="sender">The item source that raised the notification.</param>
        /// <param name="item">The item whose property changed.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        public delegate void ItemPropertyChangedDelegate(IListSource<TItem> sender, TItem item, string? propertyName);

        /// <summary>
        /// Defines a delegate that handles item transaction end notifications from an item source.
        /// </summary>
        /// <param name="sender">The item source that raised the notification.</param>
        /// <param name="item">The item whose transaction ended.</param>
        public delegate void ItemTransactionEndedDelegate(IListSource<TItem> sender, TItem item);

        /// <summary>
        /// Defines a delegate that handles item transaction start notifications from an item source.
        /// </summary>
        /// <param name="sender">The item source that raised the notification.</param>
        /// <param name="item">The item whose transaction started.</param>
        public delegate void ItemTransactionStartedDelegate(IListSource<TItem> sender, TItem item);

        /// <summary>
        /// Defines a delegate that handles item property changing notifications from an item source.
        /// </summary>
        /// <param name="sender">The item source that raised the notification.</param>
        /// <param name="item">The item whose property is changing.</param>
        /// <param name="propertyName">The name of the property that is changing.</param>
        public delegate void ItemPropertyChangingDelegate(IListSource<TItem> sender, TItem item, string? propertyName);

        /// <summary>
        /// Defines a delegate that handles item property change permission checks from an item source.
        /// </summary>
        /// <param name="sender">The item source that raised the notification.</param>
        /// <param name="item">The item whose property change is being checked.</param>
        /// <param name="propertyName">The name of the property whose change is being checked.</param>
        public delegate bool CanChangeItemPropertyDelegate(IListSource<TItem> sender, TItem item, string propertyName);

        /// <summary>
        /// Occurs when an item is inserted into the list source.
        /// </summary>
        public event ItemInsertedDelegate? WhenItemInserted;

        /// <summary>
        /// Occurs when an item is removed from the list source.
        /// </summary>
        public event ItemRemovedDelegate? WhenItemRemoved;

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event ChangedDelegate? WhenChanged;

        /// <summary>
        /// Occurs when an item's property changes.
        /// </summary>
        public event ItemPropertyChangedDelegate? WhenItemPropertyChanged;

        /// <summary>
        /// Occurs when an item's property is about to change.
        /// </summary>
        public event ItemTransactionEndedDelegate? WhenItemTransactionEnded;

        /// <summary>
        /// Occurs when an item's property is about to change.
        /// </summary>
        public event ItemTransactionStartedDelegate? WhenItemTransactionStarted;

        /// <summary>
        /// Occurs when an item's property is about to change.
        /// </summary>
        public event ItemPropertyChangingDelegate? WhenItemPropertyChanging;

        /// <summary>
        /// Occurs when a request is made to determine whether an item's property can be changed.
        /// </summary>
        public event PropertyChangedDelegate? WhenPropertyChanged;

        /// <summary>
        /// Occurs when a request is made to determine whether an item's property can be changed.
        /// </summary>
        public event UpdatesStartedDelegate? WhenUpdatesStarted;

        /// <summary>
        /// Occurs when a request is made to determine whether an item's property can be changed.
        /// </summary>
        public event UpdatesFinishedDelegate? WhenUpdatesFinished;

        /// <summary>
        /// Occurs when a request is made to determine whether an item's property can be changed.
        /// </summary>
        public event CanChangeItemPropertyDelegate? CanChangeItemPropertyRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSourceListener{TItem}"/> class.
        /// </summary>
        public ListSourceListener()
        {
        }

        /// <summary>
        /// Registers or unregisters a logger to log events from the list source listener.
        /// </summary>
        /// <param name="writer">The log writer to use for logging events.</param>
        /// <param name="register">True to register the logger; false to unregister.</param>
        public virtual void RegisterLogger(ILogWriter? writer = null, bool register = true)
        {
            writer ??= LogWriter.Debug;

            if (register)
            {
                WhenPropertyChanged+= OnPropertyChanged;
                WhenUpdatesStarted += OnUpdatesStarted;
                WhenUpdatesFinished += OnUpdatesFinished;
                WhenItemInserted += OnItemInserted;
                WhenItemRemoved += OnItemRemoved;
                WhenChanged += OnChanged;
                WhenItemPropertyChanged += OnItemPropertyChanged;
                WhenItemTransactionEnded += OnItemTransactionEnded;
                WhenItemTransactionStarted += OnItemTransactionStarted;
                WhenItemPropertyChanging += OnItemPropertyChanging;
                CanChangeItemPropertyRequest += OnCanChangeItemProperty;
            }
            else
            {
                WhenPropertyChanged -= OnPropertyChanged;
                WhenUpdatesStarted -= OnUpdatesStarted;
                WhenUpdatesFinished -= OnUpdatesFinished;
                WhenItemInserted -= OnItemInserted;
                WhenItemRemoved -= OnItemRemoved;
                WhenChanged -= OnChanged;
                WhenItemPropertyChanged -= OnItemPropertyChanged;
                WhenItemTransactionEnded -= OnItemTransactionEnded;
                WhenItemTransactionStarted -= OnItemTransactionStarted;
                WhenItemPropertyChanging -= OnItemPropertyChanging;
                CanChangeItemPropertyRequest -= OnCanChangeItemProperty;
            }

            void OnPropertyChanged(IListSource<TItem> sender, PropertyChangedEventArgs e)
            {
                writer.WriteLine($"ListSource property changed: {e.PropertyName}");
            }

            void OnUpdatesStarted(IListSource<TItem> sender)
            {
                writer.WriteLine("ListSource updates started.");
            }

            void OnUpdatesFinished(IListSource<TItem> sender)
            {
                writer.WriteLine("ListSource updates finished.");
            }
            
            void OnItemInserted(IListSource<TItem> sender, int index, TItem item)
            {
                writer.WriteLine($"ListSource item inserted at index {index}: {item}");
            }
            
            void OnItemRemoved(IListSource<TItem> sender, int index, TItem item)
            {
                writer.WriteLine($"ListSource item removed from index {index}: {item}");
            }
            
            void OnChanged(IListSource<TItem> sender, ListChangedEventArgs e)
            {
                writer.WriteLine($"ListSource changed: {e.Action}");
            }
            
            void OnItemPropertyChanged(IListSource<TItem> sender, TItem item, string? propertyName)
            {
                writer.WriteLine($"ListSource item property changed: {propertyName ?? "null"} on item {item}");
            }
            
            void OnItemTransactionEnded(IListSource<TItem> sender, TItem item)
            {
                writer.WriteLine($"ListSource item transaction ended on item {item}");
            }
            
            void OnItemTransactionStarted(IListSource<TItem> sender, TItem item)
            {
                writer.WriteLine($"ListSource item transaction started on item {item}");
            }
            
            void OnItemPropertyChanging(IListSource<TItem> sender, TItem item, string? propertyName)
            {
                writer.WriteLine($"ListSource item property changing: {propertyName ?? "null"} on item {item}");
            }
           
            bool OnCanChangeItemProperty(IListSource<TItem> sender, TItem item, string propertyName)
            {
                writer.WriteLine($"ListSource can change item property: {propertyName} on item {item}");
                return true;
            }
        }

        /// <inheritdoc/>
        public virtual void Changed(IListSource<TItem> sender, ListChangedEventArgs e)
        {
            WhenChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public virtual void ItemInserted(IListSource<TItem> sender, int index, TItem item)
        {
            WhenItemInserted?.Invoke(sender, index, item);
        }

        /// <inheritdoc/>
        public virtual void ItemRemoved(IListSource<TItem> sender, int index, TItem item)
        {
            WhenItemRemoved?.Invoke(sender, index, item);
        }

        /// <inheritdoc/>
        public virtual void PropertyChanged(IListSource<TItem> sender, PropertyChangedEventArgs e)
        {
            WhenPropertyChanged?.Invoke(sender, e);
        }

        /// <inheritdoc/>
        public virtual void UpdatesStarted(IListSource<TItem> sender)
        {
            WhenUpdatesStarted?.Invoke(sender);
        }

        /// <inheritdoc/>
        public virtual void UpdatesFinished(IListSource<TItem> sender)
        {
            WhenUpdatesFinished?.Invoke(sender);
        }

        /// <inheritdoc/>
        public virtual void ItemPropertyChanged(IListSource<TItem> sender, TItem item, string? propertyName)
        {
            WhenItemPropertyChanged?.Invoke(sender, item, propertyName);
        }

        /// <inheritdoc/>
        public virtual void ItemPropertyChanging(IListSource<TItem> sender, TItem item, string? propertyName)
        {
            WhenItemPropertyChanging?.Invoke(sender, item, propertyName);
        }

        /// <inheritdoc/>
        public virtual bool CanChangeItemProperty(IListSource<TItem> sender, TItem item, string propertyName)
        {
            return CanChangeItemPropertyRequest?.Invoke(sender, item, propertyName) ?? true;
        }

        /// <inheritdoc/>
        public virtual void ItemTransactionEnded(IListSource<TItem> sender, TItem item)
        {
            WhenItemTransactionEnded?.Invoke(sender, item);
        }

        /// <inheritdoc/>
        public virtual void ItemTransactionStarted(IListSource<TItem> sender, TItem item)
        {
            WhenItemTransactionStarted?.Invoke(sender, item);
        }
    }
}