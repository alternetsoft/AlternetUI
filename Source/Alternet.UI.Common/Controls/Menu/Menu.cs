using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the base functionality for all menus.
    /// </summary>
    public abstract partial class Menu : FrameworkElement, IMenuProperties
    {
        private BaseCollection<MenuItem>? items;
        private IList<object>? hostObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        protected Menu()
        {
        }

        /// <summary>
        /// Gets list of host objects. For example, it can contain native object references or
        /// <see cref="IContextMenuHost"/> controls
        /// such as <see cref="PopupToolBar"/> or <see cref="InnerPopupToolBar"/>.
        /// </summary>
        [Browsable(false)]
        public IList<object>? HostObjects
        {
            get
            {
                return hostObjects;
            }
        }

        /// <summary>
        /// Indicates whether this <see cref="MenuItem"/> contains any child items.
        /// </summary>
        /// <remarks>
        /// Returns <c>true</c> if the internal <c>Items</c> collection has been initialized;
        /// otherwise, <c>false</c>.
        /// </remarks>
        [Browsable(false)]
        public bool HasItems
        {
            get
            {
                return items != null;
            }
        }

        /// <summary>
        /// Gets a collection of <see cref="MenuItem"/> objects associated with the menu.
        /// </summary>
        [Content]
        public virtual BaseCollection<MenuItem> Items
        {
            get
            {
                if(items == null)
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

        /// <summary>
        /// Retrieves the first host object of the specified type
        /// from the available host object.
        /// </summary>
        /// <remarks>This method iterates through the collection of host objects
        /// associated with the menu
        /// and returns the first
        /// instance that matches the specified type <typeparamref name="T"/>.
        /// If no matching host object is found, the
        /// method returns <see langword="null"/>.</remarks>
        /// <typeparam name="T">The type of the host object to retrieve.
        /// Must be a class that implements <see cref="IContextMenuHost"/>.</typeparam>
        /// <returns>The first host object of type <typeparamref name="T"/> if found;
        /// otherwise, <see langword="null"/>.</returns>
        public virtual T? GetHostObject<T>()
            where T : class
        {
            if (HostObjects is not null)
            {
                foreach (var host in HostObjects)
                {
                    if (host is T typedHost)
                        return typedHost;
                }
            }

            return null;
        }

        /// <summary>
        /// Performs some action for the each element in <see cref="Items"/>.
        /// </summary>
        /// <param name="action">Specifies action which will be called for the
        /// each item.</param>
        /// <param name="recursive">Specifies whether to call action for all
        /// sub-items recursively or only for the direct sub-items.</param>
        public virtual void ForEachItem(Action<MenuItem> action, bool recursive = false)
        {
            if (items is null)
                return;
            foreach (var child in Items)
            {
                action(child);
                if (recursive)
                    child.ForEachItem(action, true);
            }
        }

        object ICollectionObserver<object>.GetItem(int index)
        {
            return Items[index];
        }

        /// <summary>
        /// Searches the menu for an item whose <see cref="BaseObjectWithAttr.Tag"/>
        /// matches the specified value.
        /// </summary>
        /// <param name="value">The object to compare with each item's <c>Tag</c> property.</param>
        /// <returns>
        /// The first <see cref="MenuItem"/> with a matching <c>Tag</c>, or <c>null</c> if
        /// no match is found or <paramref name="value"/> is <c>null</c>.
        /// </returns>
        /// <remarks>
        /// Useful for retrieving a menu item based on externally assigned metadata or identifiers.
        /// </remarks>
        public virtual MenuItem? FindItemWithTag(object? value)
        {
            if (items is null)
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
        /// Checks the menu item whose <c>Tag</c> matches the specified value, and unchecks all others.
        /// </summary>
        /// <param name="value">The value to search for in each item's <c>Tag</c>.</param>
        /// <remarks>
        /// Uses <see cref="FindItemWithTag(object?)"/> to locate the target item, then
        /// delegates to <see cref="CheckSingleItem(MenuItem?)"/>.
        /// Ensures exclusive checked state among sibling items.
        /// </remarks>
        public virtual void CheckSingleItemWithTag(object? value)
        {
            var item = FindItemWithTag(value);
            CheckSingleItem(item);
        }

        /// <summary>
        /// Checks the specified item and unchecks all other sibling menu items.
        /// </summary>
        /// <param name="item">The item to check exclusively; may be <c>null</c>.</param>
        /// <remarks>
        /// If <paramref name="item"/> is not <c>null</c>, its <c>Checked</c> property
        /// will be set to <c>true</c>.
        /// All other items will be unchecked to maintain exclusivity.
        /// </remarks>
        public virtual void CheckSingleItem(MenuItem? item)
        {
            UncheckItems();
            if (item is not null)
                item.Checked = true;
        }

        /// <summary>
        /// Unchecks all child menu items by setting their <c>Checked</c> property to <c>false</c>.
        /// </summary>
        /// <remarks>
        /// Iterates through the <see cref="Items"/> collection and clears the checked state
        /// of each item.
        /// If the <c>Items</c> collection is not initialized, the method exits
        /// without performing any action.
        /// </remarks>
        public virtual void UncheckItems()
        {
            if (items is null)
                return;
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                item.Checked = false;
            }
        }

        /// <summary>
        /// Adds a host object to the collection of context menu hosts if it is not already present.
        /// </summary>
        /// <remarks>This method ensures that the specified host object is added only once to the
        /// collection.</remarks>
        /// <param name="host">The host object to add. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="host"/>
        /// is <see langword="null"/>.</exception>
        public virtual void AddHostObject(object host)
        {
            if (host is null)
                throw new ArgumentNullException(nameof(host));
            if (hostObjects is null)
                hostObjects = new List<object>();
            if (!hostObjects.Contains(host))
                hostObjects.Add(host);
        }

        /// <summary>
        /// Adds item to <see cref="Items"/>.
        /// </summary>
        /// <param name="item">Menu item.</param>
        /// <returns><paramref name="item"/> parameter.</returns>
        public virtual MenuItem Add(MenuItem item)
        {
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds a separator item to the <see cref="Items"/> collection.
        /// </summary>
        /// <returns>The created separator item.</returns>
        public virtual MenuItem AddSeparator()
        {
            MenuItem item = new("-");
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Creates and adds new item to <see cref="Items"/>.
        /// </summary>
        /// <returns>Created item.</returns>
        /// <param name="title">Item title.</param>
        /// <param name="onClick">Item click action.</param>
        /// <returns></returns>
        public virtual MenuItem Add(string title, Action onClick)
        {
            MenuItem item = new(title, onClick);
            Items.Add(item);
            return item;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if (hostObjects != null)
            {
                foreach (var host in hostObjects)
                {
                    if (host is IDisposable disposable)
                        disposable.Dispose();
                }

                hostObjects.Clear();
                hostObjects = null;
            }

            base.DisposeManaged();
        }
    }
}