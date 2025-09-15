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
        private int updateCounter;

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
        /// <remarks>This event is raised whenever an item is modified,
        /// allowing subscribers to respond to
        /// the change. Ensure that event handlers are properly unsubscribed
        /// to avoid memory leaks.</remarks>
        public event EventHandler<MenuChangeEventArgs>? ItemChanged;

        /// <summary>
        /// Gets a value indicating whether the object is currently in an update block.
        /// </summary>
        public virtual bool InUpdates => updateCounter > 0;

        /// <summary>
        /// Gets the <see cref="MainMenu"/> associated with this element, if any.
        /// </summary>
        /// <remarks>This property traverses the logical tree of the element to locate the nearest
        /// <see cref="MainMenu"/> ancestor.
        /// It returns <see langword="null"/> if no such ancestor exists.</remarks>
        [Browsable(false)]
        public virtual MainMenu? MenuBar
        {
            get
            {
                FrameworkElement? p = LogicalParent;
                while (p is not null)
                {
                    if (p is MainMenu mainMenu)
                        return mainMenu;
                    p = p.LogicalParent;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the first visible menu item in the collection,
        /// or <see langword="null"/> if no items are visible.
        /// </summary>
        [Browsable(false)]
        public virtual MenuItem? FirstVisibleItem
        {
            get
            {
                if (!HasItems)
                    return null;
                foreach (var child in Items)
                {
                    if (child.Visible)
                        return child;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the last visible menu item in the collection of items.
        /// </summary>
        /// <remarks>This property iterates through the collection of items in reverse order to find the
        /// last item with its <see cref="MenuItem.Visible"/> property
        /// set to <see langword="true"/>.</remarks>
        [Browsable(false)]
        public virtual MenuItem? LastVisibleItem
        {
            get
            {
                if (!HasItems)
                    return null;
                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    var child = Items[i];
                    if (child.Visible)
                        return child;
                }

                return null;
            }
        }

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
        /// Begins an update block, incrementing the update counter.
        /// </summary>
        public virtual void BeginUpdate()
        {
            updateCounter++;
            (LogicalParent as Menu)?.BeginUpdate();
        }

        /// <summary>
        /// Executes <paramref name="action"/> between calls to <see cref="BeginUpdate"/>
        /// and <see cref="EndUpdate"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        public virtual void DoInsideUpdate(Action action)
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

        /// <summary>
        /// Ends an update block, decrementing the update counter.
        /// </summary>
        public virtual void EndUpdate()
        {
            if (updateCounter > 0)
            {
                updateCounter--;
            }
            else
            {
                RaiseItemChanged(this, new MenuChangeEventArgs(this, MenuChangeKind.Any));
            }

            (LogicalParent as Menu)?.EndUpdate();
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
        /// Raises an event to notify that a menu item has changed.
        /// </summary>
        /// <param name="item">The <see cref="Menu"/> instance that has changed.</param>
        /// <param name="kind">The type of change that occurred, represented
        /// by a <see cref="MenuChangeKind"/> value.</param>
        public void RaiseItemChanged(Menu item, MenuChangeKind kind)
        {
            var e = new MenuChangeEventArgs(item, kind);
            RaiseItemChanged(item, e);
        }

        /// <summary>
        /// Notifies that a change has occurred to the specified child item.
        /// </summary>
        /// <param name="item">The child item that has been changed.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="e"></param>
        public void RaiseItemChanged(Menu item, MenuChangeEventArgs e)
        {
            OnItemChanged(item, e);
            ItemChanged?.Invoke(this, e);
            if (LogicalParent is Menu parentMenu)
                parentMenu.RaiseItemChanged(item, e);
        }

        /// <summary>
        /// Invoked when an item in the menu is changed.
        /// </summary>
        /// <remarks>This method is called to notify derived classes of changes to a menu item.
        /// Subclasses can override this method to handle item changes, such as updates, additions,
        /// or removals.</remarks>
        /// <param name="item">The <see cref="Menu"/> instance that was changed.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="e">The event arguments containing information about the change.</param>
        protected virtual void OnItemChanged(Menu item, MenuChangeEventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnItemRemoved(object? sender, int index, MenuItem item)
        {
            base.OnItemRemoved(sender, index, item);
            var e = new MenuChangeEventArgs(item, MenuChangeKind.ItemRemoved, index);
            RaiseItemChanged(item, e);
            StaticMenuEvents.RaiseItemRemoved(this, e);
        }

        /// <inheritdoc/>
        protected override void OnItemInserted(object? sender, int index, MenuItem item)
        {
            base.OnItemInserted(sender, index, item);
            var e = new MenuChangeEventArgs(item, MenuChangeKind.ItemInserted, index);
            RaiseItemChanged(item, e);
            StaticMenuEvents.RaiseItemInserted(this, e);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            menusById.Remove(UniqueId);
            base.DisposeManaged();
        }
    }
}