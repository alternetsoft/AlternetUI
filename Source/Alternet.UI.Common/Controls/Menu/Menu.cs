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
        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        protected Menu()
        {
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
        /// <remarks>
        /// Uses <see cref="ItemContainerElement{T}.FindItemWithTag(object?)"/>
        /// to locate the target item, then
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
    }
}