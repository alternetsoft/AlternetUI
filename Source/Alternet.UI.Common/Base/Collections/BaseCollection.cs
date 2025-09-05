using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle an event when collection is changed.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="index">The index where the change occurred.</param>
    /// <param name="item">The item affected by the change.</param>
    public delegate void CollectionItemChangedHandler<T>(object? sender, int index, T item);

    /// <summary>
    /// Represents the method that will handle an event when multiple item in the
    /// collection were changed.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="index">The index where the change occurred.</param>
    /// <param name="items">The items being affected.</param>
    public delegate void CollectionItemRangeChangedHandler<T>(
        object? sender,
        int index,
        IEnumerable<T> items);

    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items get added,
    /// removed, or when the whole list is refreshed.
    /// In addition to the functionality available in <see cref="ObservableCollection{T}"/>,
    /// provides <see cref="ItemInserted"/> and <see cref="ItemRemoved"/> events.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class BaseCollection<T> : BaseObservableCollection<T>
    {
        private CollectionSecurityFlags securityFlags;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCollection{T}"/> class.
        /// </summary>
        /// <param name="securityFlags">The <see cref="CollectionSecurityFlags"/> to apply to the collection.</param>
        public BaseCollection(CollectionSecurityFlags securityFlags = CollectionSecurityFlags.None)
        {
            this.securityFlags = securityFlags;
        }

        /// <summary>
        /// Initializes a new instance of the collection
        /// as a wrapper for the specified list.
        /// </summary>
        /// <param name="list">The list that is wrapped by the new collection.</param>
        /// <param name="securityFlags">The <see cref="CollectionSecurityFlags"/> to apply to the collection.</param>
        public BaseCollection(List<T> list, CollectionSecurityFlags securityFlags = CollectionSecurityFlags.None)
            : base(list)
        {
            this.securityFlags = securityFlags;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public new event PropertyChangedEventHandler? PropertyChanged
        {
            add => base.PropertyChanged += value;
            remove => base.PropertyChanged -= value;
        }

        /// <summary>
        /// Occurs when an item is inserted in the collection.
        /// </summary>
        public event CollectionItemChangedHandler<T>? ItemInserted;

        /// <summary>
        /// Occurs when an item is removed from the collection.
        /// </summary>
        public event CollectionItemChangedHandler<T>? ItemRemoved;

        /// <summary>
        /// Occurs when an item range addition is finished in the collection.
        /// </summary>
        public event CollectionItemRangeChangedHandler<T>? ItemRangeAdditionFinished;

        /// <summary>
        /// Gets the security flags that define the permissions or restrictions.
        /// </summary>
        public CollectionSecurityFlags Flags => securityFlags;

        /// <summary>
        /// Returns <see langword="true"/> if <see cref="AddRange"/> is being
        /// executed at the moment.
        /// </summary>
        public bool RangeOpInProgress { get; private set; }

        /// <summary>
        /// Gets the first item in the collection or 'default'.
        /// </summary>
        public T? First
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (Count > 0)
                    return this[0];
                else
                    return default;
            }
        }

        /// <summary>
        /// Gets the last item in the collection or 'default'.
        /// </summary>
        public T? Last
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (Count > 0)
                    return this[Count - 1];
                else
                    return default;
            }
        }

        /// <summary>
        /// Returns collection as <see cref="IList"/>.
        /// This is for compatibility.
        /// </summary>
        public IList AsList => this;

        /// <summary>
        /// Gets a value indicating whether an
        /// <see cref="ArgumentNullException"/> should be thrown
        /// on an attempt to add a <c>null</c> value to the collection.
        /// </summary>
        public bool ThrowOnNullAdd
        {
            get => securityFlags.HasFlag(CollectionSecurityFlags.NoNull);

            private set
            {
                if (value)
                    securityFlags |= CollectionSecurityFlags.NoNull;
                else
                    securityFlags &= ~CollectionSecurityFlags.NoNull;
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[long index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return base[(int)index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                SafeSetItem((int)index, value);
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public new T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return base[index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                SafeSetItem(index, value);
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element or <c>null</c>.</param>
        /// <returns>The element at the specified index or <c>null</c> if <paramref name="index"/>
        /// is <c>null</c>.</returns>
        public T? this[int? index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index is null)
                    return default;
                return base[index.Value];
            }
        }

        /// <summary>
        /// Adds the elements of the specified <see cref="IEnumerable{T}"/> to the end
        /// of the collection.
        /// </summary>
        /// <param name="collection">
        /// The <see cref="IEnumerable{T}"/> whose elements should be added to the end
        /// of the collection.
        /// The value itself cannot be <c>null</c>, but it can contain elements that are
        /// <c>null</c>, if type <c>T</c> is a reference type.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// <c>collection</c> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// The order of the elements in the <see cref="IEnumerable{T}"/> is preserved
        /// in the collection.
        /// </remarks>
        public void AddRange(IEnumerable<T>? collection)
        {
            if (collection is null)
                return;

            RangeOpInProgress = true;

            int index = Count;
            try
            {
                foreach (var item in collection)
                    Add(item);
            }
            finally
            {
                RangeOpInProgress = false;
                OnItemRangeAdditionFinished(index, collection);
            }
        }

        /// <summary>
        /// Removes the last item from the collection.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveLast()
        {
            if (Count > 0)
                RemoveAt(Count - 1);
        }

        /// <summary>
        /// Removes the first item from the collection.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveFirst()
        {
            if (Count > 0)
                RemoveAt(0);
        }

        /// <summary>
        /// Same as adding of the item, but prepends the items to the beginning of the
        /// list of items owned by this collection.
        /// </summary>
        /// <param name="item">The object to insert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Prepend(T item) => Insert(0, item);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first
        /// occurrence within the entire collection.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the collection.
        /// The value can be null for reference types.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of item within the entire collection,
        /// if found; otherwise, <c>null</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int? IndexOfOrNull(T item)
        {
            var result = IndexOf(item);
            if (result < 0)
                return null;
            return result;
        }

        /// <summary>
        /// Changes the number of elements in the collection.
        /// </summary>
        /// <param name="newCount">New number of elements.</param>
        /// <param name="createItem">Function which creates new item.</param>
        /// <remarks>
        /// If collection has more items than specified in <paramref name="newCount"/>,
        /// these items are removed. If collection has less items, new items are created
        /// using <paramref name="createItem"/> function.
        /// </remarks>
        public virtual void SetCount(int newCount, Func<T> createItem)
        {
            ListUtils.SetCount(this, newCount, createItem);
        }

        /// <summary>
        /// Sets the index of the specified item in the collection.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <param name="newIndex">The new index value of the item.</param>
        /// <exception cref="ArgumentException">The item is not in the collection.</exception>
        /// <remarks>
        /// If <paramref name="newIndex"/> = -1, moves to the end of the collection.
        /// </remarks>
        public virtual void SetItemIndex(T item, int newIndex)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            int childIndex = GetItemIndex(item);
            if (childIndex != newIndex)
            {
                if (newIndex >= Count || newIndex == -1)
                    newIndex = Count - 1;
                MoveItem(item, childIndex, newIndex);
            }
        }

        /// <summary>
        /// Retrieves the index of the specified item within
        /// the collection, and optionally raises an exception if
        /// the specified item is not within the collection.
        /// </summary>
        /// <param name="item">The item to search for in the collection.</param>
        /// <param name="throwException">
        /// <see langword="true" /> to throw an exception if the item specified
        /// in the <paramref name="item" /> parameter is not a item in
        /// the collection; otherwise, <see langword="false" />.</param>
        /// <returns>A zero-based index value that represents the location of the specified
        /// item within the collection; otherwise -1 if the specified
        /// item is not found in the collection.</returns>
        /// <exception cref="ArgumentException">The <paramref name="item" />
        /// is not in the collection, and the <paramref name="throwException" /> parameter value
        /// is <see langword="true" />.</exception>
        public virtual int GetItemIndex(T item, bool throwException = true)
        {
            int num = IndexOf(item);
            if (num == -1 && throwException)
                throw new ArgumentException("Item is not in the collection.", nameof(item));
            return num;
        }

        /// <summary>
        /// Moves the item at the specified index to a new location in the collection.
        /// </summary>
        /// <param name="oldIndex">The zero-based index specifying the location
        /// of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        public new virtual void Move(int oldIndex, int newIndex)
        {
            base.Move(oldIndex, newIndex);
        }

        internal static void Copy(
            IList<T> source,
            int sourceIndex,
            IList<T> dest,
            int destIndex,
            int length)
        {
            if (sourceIndex < destIndex)
            {
                sourceIndex += length;
                destIndex += length;
                while (length > 0)
                {
                    dest[--destIndex] = source[--sourceIndex];
                    length--;
                }
            }
            else
            {
                while (length > 0)
                {
                    dest[destIndex++] = source[sourceIndex++];
                    length--;
                }
            }
        }

        internal void MoveItem(T element, int fromIndex, int toIndex)
        {
            int num = toIndex - fromIndex;
            if (num == -1 || num == 1)
            {
                Items[fromIndex] = Items[toIndex];
            }
            else
            {
                int num2;
                int num3;
                if (num > 0)
                {
                    num2 = fromIndex + 1;
                    num3 = fromIndex;
                }
                else
                {
                    num2 = toIndex;
                    num3 = toIndex + 1;
                    num = -num;
                }

                Copy(this, num2, this, num3, num);
            }

            Items[toIndex] = element;
        }

        internal void SafeSetItem(int index, T item)
        {
            if (securityFlags.HasFlag(CollectionSecurityFlags.NoReplace))
                throw new InvalidOperationException("Replacing items is not allowed.");

            if (Items.IsReadOnly)
                throw new NotSupportedException(ErrorMessages.Default.CannotChangeReadOnlyObject);

            if ((uint)index >= (uint)Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            SetItem(index, item);
        }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>item</c> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, T item)
        {
            if (ThrowOnNullAdd && item is null)
                throw new ArgumentNullException(nameof(item));

            base.InsertItem(index, item);
            OnItemInserted(index, item);
        }

        /// <summary>
        /// Removes the item at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);
            OnItemRemoved(index, item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        /// <remarks>The base class calls this method when the list is being cleared.</remarks>
        protected override void ClearItems()
        {
            if (Count == 0)
                return;
            for (int i = Count - 1; i >= 0; i--)
            {
                RemoveItem(i);
            }

            base.ClearItems();
        }

        /// <summary>
        /// Raises the <see cref="ItemRangeAdditionFinished"/> event with the provided arguments.
        /// </summary>
        /// <param name="insertionIndex">The index to insert the <paramref name="items"/> at.</param>
        /// <param name="items">The items being inserted.</param>
        protected virtual void OnItemRangeAdditionFinished(int insertionIndex, IEnumerable<T> items)
        {
            ItemRangeAdditionFinished?.Invoke(this, insertionIndex, items);
        }

        /// <summary>
        /// Raises the <see cref="ItemInserted"/> event with the provided arguments.
        /// </summary>
        /// <param name="index">The index where the change occurred.</param>
        /// <param name="item">The item affected by the change.</param>
        protected virtual void OnItemInserted(int index, T item)
        {
            ItemInserted?.Invoke(this, index, item);
        }

        /// <summary>
        /// Raises the <see cref="ItemRemoved"/> event with the provided arguments.
        /// </summary>
        /// <param name="index">The index where the change occurred.</param>
        /// <param name="item">The item affected by the change.</param>
        protected virtual void OnItemRemoved(int index, T item)
        {
            ItemRemoved?.Invoke(this, index, item);
        }
    }
}