using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a container element that manages a collection of items of
    /// type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>This class provides functionality to manage a collection of items,
    /// including querying, observing, and accessing child elements.
    /// It supports operations such as retrieving items by tag and exposing
    /// logical and content elements for the hosted framework.</remarks>
    /// <typeparam name="T">The type of items contained in the collection.
    /// Must derive from <see cref="FrameworkElement"/>.</typeparam>
    public partial class ItemContainerElement<T>
        : HostedFrameworkElement, ICollectionObserver<object>
        where T : FrameworkElement
    {
        private BaseCollection<T>? items;

        /// <summary>
        /// Gets a value indicating whether the collection contains any items.
        /// </summary>
        [Browsable(false)]
        public bool HasItems
        {
            get
            {
                return items != null && items.Count > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether items have been allocated.
        /// </summary>
        [Browsable(false)]
        public bool ItemsAllocated
        {
            get
            {
                return items != null;
            }
        }

        /// <summary>
        /// Gets the collection of items in the current instance.
        /// </summary>
        [Content]
        public virtual BaseCollection<T> Items
        {
            get
            {
                if (items == null)
                {
                    items = new() { ThrowOnNullAdd = true };
                }

                return items;
            }
        }

        /// <summary>
        /// Gets the total number of child items currently in the collection.
        /// </summary>
        [Browsable(false)]
        int ICollectionObserver<object>.Count
        {
            get
            {
                if (HasItems)
                {
                    return Items.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements
        {
            get
            {
                if (items == null)
                    return Array.Empty<FrameworkElement>();
                return items;
            }
        }

        /// <inheritdoc />
        public override IEnumerable<FrameworkElement> LogicalChildrenCollection => ContentElements;

        INotifyCollectionChanged ICollectionObserver<object>.Notification
        {
            get => Items;
        }

        object ICollectionObserver<object>.GetItem(int index)
        {
            return Items[index];
        }

        /// <summary>
        /// Searches for an item whose <see cref="BaseObjectWithAttr.Tag"/>
        /// matches the specified value.
        /// </summary>
        /// <param name="value">The object to compare with each item's <c>Tag</c> property.</param>
        /// <returns>
        /// The first item with a matching <c>Tag</c>, or <c>null</c> if
        /// no match is found or <paramref name="value"/> is <c>null</c>.
        /// </returns>
        /// <remarks>
        /// Useful for retrieving an item based on externally assigned metadata or identifiers.
        /// </remarks>
        public virtual T? FindItemWithTag(object? value)
        {
            if (!HasItems)
                return null;
            if (value is null)
                return null;

            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                if (value.Equals(item.Tag))
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Adds the specified item to the collection of items.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>value of <paramref name="item"/> parameter to allow method chaining.</returns>
        public virtual T Add(T item)
        {
            Items.Add(item);
            return item;
        }
    }
}
