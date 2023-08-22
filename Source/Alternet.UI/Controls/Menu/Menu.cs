using System;
using System.Collections.Generic;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the base functionality for all menus.
    /// </summary>
    public abstract class Menu : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        protected Menu()
        {
            Items.ItemInserted += Items_ItemInserted;
            Items.ItemRemoved += Items_ItemRemoved;
        }

        /// <summary>
        /// Gets a collection of <see cref="MenuItem"/> objects associated with the menu.
        /// </summary>
        [Content]
        public Collection<MenuItem> Items { get; } = new Collection<MenuItem>();

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.Menu;

        /// <inheritdoc/>
        public override IReadOnlyList<FrameworkElement> ContentElements => Items;

        internal IntPtr MenuHandle => (Handler.NativeControl as Native.Menu)!.MenuHandle;

        internal override bool IsDummy => true;

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection => Items;

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<MenuItem> e)
        {
            // This is required for data binding inheritance.
            Children.Add(e.Item);
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<MenuItem> e)
        {
            Children.Remove(e.Item);
        }
    }
}