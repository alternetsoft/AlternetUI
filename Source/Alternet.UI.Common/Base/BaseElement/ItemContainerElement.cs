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
        /// Specifies flags that modify the behavior of item search operations.
        /// </summary>
        /// <remarks>These flags can be combined using a bitwise OR operation to customize the search
        /// behavior.</remarks>
        [Flags]
        public enum FindItemFlags
        {
            /// <summary>
            /// Represents the absence of any specific options or flags.
            /// </summary>
            None = 0,

            /// <summary>
            /// Specifies that string comparisons should ignore case differences.
            /// </summary>
            IgnoreCase = 1 << 0,

            /// <summary>
            /// Specifies that whitespace should be trimmed from the beginning and end of the search text.
            /// </summary>
            TrimText = 1 << 1,
        }

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
        /// Gets the first item in the collection, or <see langword="null"/> if the collection is empty.
        /// </summary>
        public T? FirstItem
        {
            get
            {
                if (!HasItems)
                    return null;
                return Items[0];
            }
        }

        /// <summary>
        /// Gets the last item in the collection, or <see langword="null"/> if the collection is empty.
        /// </summary>
        public T? LastItem
        {
            get
            {
                if (!HasItems)
                    return null;
                return Items[Items.Count - 1];
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
                    items = new(CollectionSecurityFlags.NoNullOrReplace);
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

        INotifyCollectionChanged ICollectionObserver<object>.Notification
        {
            get => Items;
        }

        object ICollectionObserver<object>.GetItem(int index)
        {
            return Items[index];
        }

        /// <summary>
        /// Retrieves an item from the collection by its name.
        /// </summary>
        /// <remarks>This method performs a linear search through the <see cref="Items"/> collection.
        /// If multiple items share
        /// the same name, the first matching item is returned.
        /// If the collection is empty, the method returns <see langword="null"/>.</remarks>
        /// <param name="name">The name of the item to retrieve. This value is case-sensitive.</param>
        /// <returns>The item of type <typeparamref name="T"/> if an item with the specified
        /// name exists in the collection; otherwise, <see langword="null"/>.</returns>
        public virtual T? ItemByName(string name)
        {
            if (!HasItems)
                return null;

            foreach (var child in Items)
            {
                if (child.Name == name)
                    return child;
            }

            return null;
        }

        /// <summary>
        /// Searches for an item whose <see cref="BaseObjectWithAttr.Tag"/>
        /// matches the specified value.
        /// </summary>
        /// <param name="value">The object to compare with each item's <c>Tag</c> property.</param>
        /// <param name="flags">The <see cref="FindItemFlags"/> that modify the search behavior.</param>
        /// <returns>
        /// The first item with a matching <c>Tag</c>, or <c>null</c> if
        /// no match is found or <paramref name="value"/> is <c>null</c>.
        /// </returns>
        /// <remarks>
        /// Useful for retrieving an item based on externally assigned metadata or identifiers.
        /// </remarks>
        public virtual T? FindItemWithTag(object? value, FindItemFlags flags = FindItemFlags.None)
        {
            if (!HasItems)
                return null;
            if (value is null)
                return null;

            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                if (ValuesAreEqual(value, item.Tag))
                    return item;
            }

            return null;

            bool ValuesAreEqual(object? item1, object? item2)
            {
                var result = item1?.Equals(item2) ?? item2 is null;

                if (result)
                    return true;

                bool trimText = flags.HasFlag(FindItemFlags.TrimText);
                bool ignoreCase = flags.HasFlag(FindItemFlags.IgnoreCase);

                if(trimText || ignoreCase)
                {
                    var s1 = item1?.ToString() ?? string.Empty;
                    var s2 = item2?.ToString() ?? string.Empty;

                    if (trimText)
                    {
                        s1 = s1.Trim();
                        s2 = s2.Trim();
                    }

                    var comparison = ignoreCase ?
                        StringComparison.OrdinalIgnoreCase :
                        StringComparison.Ordinal;
                    return string.Equals(s1, s2, comparison);
                }
                else
                {
                    return false;
                }
            }
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
