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
    public abstract partial class Menu : ItemContainerElement<MenuItem>, IMenuProperties
    {
        private static readonly BaseDictionary<ObjectUniqueId, Menu> menusById = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        protected Menu()
        {
            menusById.Add(UniqueId, this);
        }

        /// <summary>
        /// Occurs when an item is changed.
        /// </summary>
        /// <remarks>This event is raised whenever an item is modified, allowing subscribers to respond to
        /// the change. Ensure that event handlers are properly unsubscribed to avoid memory leaks.</remarks>
        public event EventHandler<MenuChangeEventArgs>? ItemChanged;

        /// <summary>
        /// Retrieves a menu by its unique identifier.
        /// </summary>
        /// <remarks>This method searches for a menu in the collection using the provided unique
        /// identifier. If the identifier does not exist in the collection,
        /// the method returns <see langword="null"/>.</remarks>
        /// <param name="id">The unique identifier of the menu to retrieve.</param>
        /// <returns>The <see cref="Menu"/> associated with the specified identifier,
        /// or <see langword="null"/> if no menu is found.</returns>
        public static Menu? MenuFromId(ObjectUniqueId id)
        {
            menusById.TryGetValue(id, out var menu);
            return menu;
        }

        /// <summary>
        /// Retrieves a <see cref="Menu"/> instance corresponding to the specified string identifier.
        /// </summary>
        /// <param name="stringId">The string representation of the unique identifier for the menu.
        /// This value can be <see langword="null"/>.</param>
        /// <returns>The <see cref="Menu"/> instance associated with the given string identifier,
        /// or <see langword="null"/> if
        /// the identifier is invalid or does not correspond to an existing menu.</returns>
        public static Menu? MenuFromStringId(string? stringId)
        {
            var id = ObjectUniqueId.FromString(stringId);
            if (id is null)
                return null;

            return MenuFromId(id.Value);
        }

        /// <summary>
        /// Performs some action for the each element in <see cref="ItemContainerElement{T}.Items"/>.
        /// </summary>
        /// <param name="action">Specifies action which will be called for the
        /// each item.</param>
        /// <param name="recursive">Specifies whether to call action for all
        /// sub-items recursively or only for the direct sub-items.</param>
        public virtual void ForEachItem(Action<MenuItem> action, bool recursive = false)
        {
            if (!HasItems)
                return;
            foreach (var child in Items)
            {
                action(child);
                if (recursive)
                    child.ForEachItem(action, true);
            }
        }

        /// <summary>
        /// Checks the menu item whose <c>Tag</c> matches the specified value, and unchecks all others.
        /// </summary>
        /// <param name="value">The value to search for in each item's <c>Tag</c>.</param>
        /// <param name="flags">The flags that modify the search behavior.</param>
        /// <remarks>
        /// Ensures exclusive checked state among sibling items.
        /// </remarks>
        public virtual bool CheckSingleItemWithTag(object? value, FindItemFlags flags = FindItemFlags.None)
        {
            var item = FindItemWithTag(value, flags);
            return CheckSingleItem(item);
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
        public virtual bool CheckSingleItem(MenuItem? item)
        {
            UncheckItems();

            if (item is not null)
            {
                item.Checked = true;
            }

            return item is not null;
        }

        /// <summary>
        /// Unchecks all child menu items by setting their <c>Checked</c> property to <c>false</c>.
        /// </summary>
        /// <remarks>
        /// Iterates through the items collection and clears the checked state
        /// of each item.
        /// If the <c>Items</c> collection is not initialized, the method exits
        /// without performing any action.
        /// </remarks>
        public virtual void UncheckItems()
        {
            if (!HasItems)
                return;
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                item.Checked = false;
            }
        }

        /// <summary>
        /// Creates a context menu handle using the specified menu factory.
        /// Uses child items for populating the created context menu, if any.
        /// </summary>
        /// <remarks>If the menu contains items, they are added to the created context menu using the
        /// specified or default factory.</remarks>
        /// <param name="factory">The <see cref="IMenuFactory"/> instance used to create the context menu handle.
        /// If null, a default factory is used.</param>
        /// <returns>A <see cref="IMenuFactory.ContextMenuHandle"/> representing the created context menu, or
        /// <see langword="null"/> if no factory is available.</returns>
        public virtual IMenuFactory.ContextMenuHandle? CreateItemsHandle(IMenuFactory? factory = null)
        {
            factory ??= MenuUtils.Factory;
            if (factory == null)
                return null;
            var result = factory.CreateContextMenu(UniqueId.ToString());

            if(!HasItems)
                return result;

            foreach (var item in Items)
            {
                var itemHandle = item.CreateItemHandle(factory);
                if(itemHandle is not null)
                    factory.MenuAddItem(result, itemHandle);
            }

            return result;
        }

        /// <summary>
        /// Adds a separator item to the <see cref="ItemContainerElement{T}.Items"/> collection.
        /// </summary>
        /// <returns>The created separator item.</returns>
        public virtual MenuItem AddSeparator()
        {
            MenuItem item = new("-");
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Creates and adds new item to <see cref="ItemContainerElement{T}.Items"/> collection.
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

        /// <summary>
        /// Notifies that a change has occurred to the specified child item.
        /// </summary>
        /// <param name="item">The child item that has been changed. Cannot be <see langword="null"/>.</param>
        /// <param name="action">The type of change that occurred, represented
        /// by a <see cref="MenuChangeKind"/> value.</param>
        public virtual void RaiseItemChanged(Menu item, MenuChangeKind action)
        {
            ItemChanged?.Invoke(this, new MenuChangeEventArgs(item, action));
            if (LogicalParent is Menu parentMenu)
                parentMenu.RaiseItemChanged(this, action);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            menusById.Remove(UniqueId);
            base.DisposeManaged();
        }
    }
}