using System;
using System.Collections.Generic;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the base functionality for all menus.
    /// </summary>
    public abstract partial class Menu : NonVisualControl
    {
        private BaseCollection<MenuItem>? items;

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        protected Menu()
        {
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
                    items.ItemInserted += OnItemInserted;
                    items.ItemRemoved += OnItemRemoved;
                }

                return items;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Menu;

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

        /// <inheritdoc />
        protected override bool IsDummy => true;

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

        private void OnItemInserted(object? sender, int index, MenuItem item)
        {
            // This is required for data binding inheritance.
            Children.Add(item);
        }

        private void OnItemRemoved(object? sender, int index, MenuItem item)
        {
            Children.Remove(item);
        }
    }
}