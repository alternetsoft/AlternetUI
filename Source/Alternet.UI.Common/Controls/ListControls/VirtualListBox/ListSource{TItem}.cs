using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list source that can be used with a virtual list box control.
    /// This class provides a basic implementation of the <see cref="IListSource{TItem}"/> interface
    /// using an internal <see cref="BaseCollection{TItem}"/> to store the items.
    /// </summary>
    /// <typeparam name="TItem">The type of items stored in the list source.</typeparam>
    public class ListSource<TItem> : DisposableObject, IListSource<TItem>
        where TItem : class, new()
    {
        private readonly object syncRoot = new();
        private BaseCollection<TItem> collection;
        private IListSourceListener<TItem>[] listeners = [];

        /// <inheritdoc/>
        public event EventHandler? UpdatesStarted;

        /// <inheritdoc/>
        public event EventHandler? UpdatesFinished;

        /// <inheritdoc/>
        public event CollectionItemChangedHandler<TItem>? ItemInserted;

        /// <inheritdoc/>
        public event CollectionItemChangedHandler<TItem>? ItemRemoved;

        /// <inheritdoc/>
        public event ListChangedEventHandler? CollectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSource{TItem}"/> class.
        /// </summary>
        public ListSource()
            : this(new NotNullCollection<TItem>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSource{TItem}"/>
        /// class with the specified collection of items.
        /// </summary>
        /// <param name="collection">The collection of items to initialize the list source with.</param>
        public ListSource(BaseCollection<TItem>? collection)
        {
            SetCollection(collection);
        }

        /// <inheritdoc/>
        public TItem this[int index] => collection[index];

        /// <inheritdoc/>
        TItem IList<TItem>.this[int index] { get => collection[index]; set => collection[index] = value; }

        /// <summary>
        /// Gets or sets the underlying collection of items.
        /// </summary>  
        public BaseCollection<TItem> Collection
        {
            get
            {
                return collection;
            }

            set
            {
                SetCollection(value);
            }
        }

        /// <inheritdoc/>
        public bool InUpdate => collection.InUpdate;

        /// <inheritdoc/>
        public virtual int Count
        {
            get
            {
                return collection.Count;
            }

            set
            {
                SetCount(value, () => new TItem());
            }
        }

        /// <inheritdoc/>
        public virtual bool IsReadOnly => false;

        /// <inheritdoc/>
        public IList AsList => collection;

        /// <inheritdoc/>
        public virtual void Add(TItem item)
        {
            collection.Add(item);
        }

        /// <inheritdoc/>
        public virtual void AddRange(IEnumerable<TItem> items)
        {
            collection.AddRange(items);
        }

        /// <inheritdoc/>
        public virtual void BeginUpdate()
        {
            collection.BeginUpdate();
        }

        /// <inheritdoc/>
        public virtual void EndUpdate()
        {
            collection.EndUpdate();
        }

        /// <inheritdoc/>
        public virtual void Sort()
        {
            collection.Sort();
        }

        /// <inheritdoc/>
        public virtual void Clear()
        {
            collection.Clear();
        }

        /// <inheritdoc/>
        public void DoInsideUpdate(Action action)
        {
            BeginUpdate();
            try
            {
                action();
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <inheritdoc/>
        public virtual bool Contains(TItem item)
        {
            return collection.Contains(item);
        }

        /// <inheritdoc/>
        public virtual void CopyTo(TItem[] array, int arrayIndex)
        {
            collection.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public virtual IEnumerator<TItem> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        /// <inheritdoc/>
        public virtual TItem? GetItem(int index)
        {
            return collection[index];
        }

        /// <inheritdoc/>
        public virtual int IndexOf(TItem item)
        {
            return collection.IndexOf(item);
        }

        /// <inheritdoc/>
        public virtual void Insert(int index, TItem item)
        {
            collection.Insert(index, item);
        }

        /// <inheritdoc/>
        public virtual bool Remove(TItem item)
        {
            return collection.Remove(item);
        }

        /// <inheritdoc/>
        public virtual void RemoveAt(int index)
        {
            collection.RemoveAt(index);
        }

        /// <inheritdoc/>
        public virtual void SetCount(int count, Func<TItem> fnCreateItem)
        {
            collection.SetCount(count, fnCreateItem);
        }

        /// <inheritdoc/>
        public virtual void SetItem(int index, TItem value)
        {
            collection[index] = value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public virtual void Sort(IComparer<TItem>? comparer)
        {
            DoInsideUpdate(() => collection.Sort(comparer));
        }

        /// <inheritdoc/>
        public virtual void Sort(int index, int count, IComparer<TItem>? comparer)
        {
            DoInsideUpdate(() => collection.Sort(index, count, comparer));
        }

        /// <inheritdoc/>
        public virtual void Sort(Comparison<TItem> comparison)
        {
            DoInsideUpdate(() => collection.Sort(comparison));
        }

        /// <inheritdoc/>
        public virtual void SortDescending(Comparison<TItem> comparison)
        {
            DoInsideUpdate(() => collection.SortDescending(comparison));
        }

        /// <summary>
        /// Subscribes to events of the current underlying collection.
        /// </summary>
        protected virtual void BindCollection()
        {
            collection.CollectionChanged += OnCollectionChanged;
            collection.PropertyChanged += OnPropertyChanged;
            collection.ItemInserted += OnItemInserted;
            collection.ItemRemoved += OnItemRemoved;
            collection.UpdatesStarted += OnUpdatesStarted;
            collection.UpdatesFinished += OnUpdatesFinished;
        }

        /// <summary>
        /// Unsubscribes from events of the current underlying collection.
        /// </summary>
        protected virtual void UnbindCollection()
        {
            if (collection != null)
            {
                collection.CollectionChanged -= OnCollectionChanged;
                collection.PropertyChanged -= OnPropertyChanged;
                collection.ItemInserted -= OnItemInserted;
                collection.ItemRemoved -= OnItemRemoved;
                collection.UpdatesStarted -= OnUpdatesStarted;
                collection.UpdatesFinished -= OnUpdatesFinished;
            }
        }

        /// <summary>
        /// Sets the underlying collection of items for this list source.
        /// </summary>
        /// <param name="newCollection">The collection of items to set as the underlying collection.</param>
        [MemberNotNull(nameof(collection))]
        protected virtual void SetCollection(BaseCollection<TItem>? newCollection)
        {
            UnbindCollection();
            collection = newCollection ?? CreateCollection();
            BindCollection();
        }

        /// <summary>
        /// Creates a new instance of the underlying collection of items for this list source.
        /// </summary>
        /// <returns>The newly created collection.</returns>
        protected virtual BaseCollection<TItem> CreateCollection()
        {
            return new NotNullCollection<TItem>();
        }

        /// <summary>
        /// Subscribes the specified listener to receive notifications of changes in the list source.
        /// </summary>
        /// <param name="listener">The listener to subscribe.</param>
        public virtual void Subscribe(IListSourceListener<TItem> listener)
        {
            if (DisposingOrDisposed)
                return;
            lock (syncRoot)
                listeners = ArrayUtils.WithAddedElement(listeners, listener);
        }

        /// <summary>
        /// Notifies listeners when a property of an item in the underlying collection changes.
        /// </summary>
        /// <param name="item">The item whose property has changed.</param>
        /// <param name="propertyName">The name of the property that has changed.</param>
        public virtual void RaiseItemPropertyChanged(TItem item, string? propertyName)
        {
            if (DisposingOrDisposed)
                return;
            foreach (var listener in listeners)
                listener.ItemPropertyChanged(this, item, propertyName);
        }

        /// <summary>
        /// Unsubscribes the specified listener from receiving notifications of changes in the list source.
        /// </summary>
        /// <param name="listener">The listener to unsubscribe.</param>
        public virtual void Unsubscribe(IListSourceListener<TItem> listener)
        {
            lock (syncRoot)
            {
                var index = Array.IndexOf(listeners, listener);
                if (index >= 0)
                    listeners = ArrayUtils.WithoutElementAt(listeners, index);
            }
        }

        /// <summary>
        /// Raises the <see cref="UpdatesFinished"/> event when an update operation on the list is finished.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnUpdatesFinished(object? sender, EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            UpdatesFinished?.Invoke(this, EventArgs.Empty);
            foreach (var listener in listeners)
                listener.UpdatesFinished(this);
        }

        /// <summary>
        /// Raises the <see cref="UpdatesStarted"/> event when an update operation on the list is started.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnUpdatesStarted(object? sender, EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            UpdatesStarted?.Invoke(this, EventArgs.Empty);
            foreach (var listener in listeners)
                listener.UpdatesStarted(this);
        }

        /// <summary>
        /// Raises the <see cref="ItemRemoved"/> event when an item is removed from the underlying collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="index">The index at which the item was removed.</param>
        /// <param name="item">The item that was removed.</param>
        protected virtual void OnItemRemoved(object? sender, int index, TItem item)
        {
            if (DisposingOrDisposed)
                return;
            ItemRemoved?.Invoke(this, index, item);
            foreach (var listener in listeners)
                listener.ItemRemoved(this, index, item);
        }

        /// <summary>
        /// Raises the <see cref="ItemInserted"/> event when an item is inserted into the underlying collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="index">The index at which the item was inserted.</param>
        /// <param name="item">The item that was inserted.</param>
        protected virtual void OnItemInserted(object? sender, int index, TItem item)
        {
            if (DisposingOrDisposed)
                return;
            ItemInserted?.Invoke(this, index, item);
            foreach (var listener in listeners)
                listener.ItemInserted(this, index, item);
        }

        /// <summary>
        /// Raises the PropertyChanged event when a property of the underlying collection changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="PropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            RaisePropertyChangedEx(e);
            foreach (var listener in listeners)
                listener.PropertyChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event when the underlying collection changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="ListChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnCollectionChanged(object? sender, ListChangedEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            CollectionChanged?.Invoke(this, e);
            foreach (var listener in listeners)
                listener.Changed(this, e);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            UnbindCollection();
            base.DisposeManaged();
        }
    }
}
