using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list source that can be used with a virtual list box control.
    /// This class provides a basic implementation of the <see cref="IListSource{TItem}"/> interface
    /// using an internal <see cref="BaseCollection{TItem}"/> to store the items.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class ListSource<TItem> : IListSource<TItem>
        where TItem : class, new()
    {
        private readonly BaseCollection<TItem> collection;

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSource{TItem}"/> class.
        /// </summary>
        public ListSource()
            : this(new NotNullCollection<TItem>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSource{TItem}"/> class with the specified collection of items.
        /// </summary>
        /// <param name="collection">The collection of items to initialize the list source with.</param>
        public ListSource(BaseCollection<TItem>? collection)
        {
            this.collection = collection ?? new NotNullCollection<TItem>();
            this.collection.CollectionChanged += (s, e) => CollectionChanged?.Invoke(this, e);
            this.collection.PropertyChanged += (s, e) => PropertyChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public TItem this[int index] => collection[index];

        /// <inheritdoc/>
        TItem IList<TItem>.this[int index] { get => collection[index]; set => collection[index] = value; }

        /// <summary>
        /// Gets the underlying collection of items.
        /// </summary>  
        public BaseCollection<TItem> Collection => collection;

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
        public void Sort(IComparer<TItem>? comparer)
        {
            collection.Sort(comparer);
        }

        /// <inheritdoc/>
        public void Sort(int index, int count, IComparer<TItem>? comparer)
        {
            collection.Sort(index, count, comparer);
        }

        /// <inheritdoc/>
        public void Sort(Comparison<TItem> comparison)
        {
            collection.Sort(comparison);
        }

        /// <inheritdoc/>
        public void SortDescending(Comparison<TItem> comparison)
        {
            collection.SortDescending(comparison);
        }
    }
}
