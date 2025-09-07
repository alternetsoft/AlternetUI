using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for menu change events, indicating which menu item was changed and the type of change.
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
        /// Gets or sets the menu item that was changed.
        /// </summary>
        public Menu Item { get; set; }

        /// <summary>
        /// Gets or sets the type of change that occurred.
        /// </summary>
        public MenuChangeKind Action { get; set; }
    }
}
