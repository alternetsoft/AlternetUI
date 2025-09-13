using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for menu change events, indicating which menu
    /// item was changed and the type of change.
    /// </summary>
    public class MenuChangeEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuChangeEventArgs"/> class.
        /// </summary>
        /// <param name="item">The menu item that was changed.</param>
        /// <param name="action">The type of change that occurred.</param>
        public MenuChangeEventArgs(Menu item, MenuChangeKind action)
        {
            Item = item;
            Action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuChangeEventArgs"/>
        /// class with the specified menu item, action, and index.
        /// </summary>
        /// <remarks>This constructor allows specifying the index of the menu item in
        /// addition to the item and the action.</remarks>
        /// <param name="item">The <see cref="Menu"/> item associated with the event.</param>
        /// <param name="action">The kind of change that occurred, represented by
        /// a <see cref="MenuChangeKind"/> value.</param>
        /// <param name="index">The zero-based index of the menu item in the collection.
        /// Must be greater than or equal to 0.</param>
        public MenuChangeEventArgs(Menu item, MenuChangeKind action, int index)
            : this(item, action)
        {
            Index = index;
        }

        /// <summary>
        /// Gets or sets the menu item that was changed.
        /// </summary>
        public Menu Item { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the item. This is optional and may be null.
        /// <see cref="Index"/> is typically used to indicate the position where item was inserted
        /// or removed in the case of <see cref="MenuChangeKind.ItemInserted"/> or
        /// <see cref="MenuChangeKind.ItemRemoved"/>.
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// Gets or sets the type of change that occurred.
        /// </summary>
        public MenuChangeKind Action { get; set; }
    }
}
