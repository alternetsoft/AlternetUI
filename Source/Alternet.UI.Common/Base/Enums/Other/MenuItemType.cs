using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the type of a menu item.
    /// </summary>
    public enum MenuItemType
    {
        /// <summary>
        /// A standard menu item.
        /// </summary>
        Standard,

        /// <summary>
        /// A menu item that can be checked or unchecked.
        /// </summary>
        Check,

        /// <summary>
        /// A menu item that behaves as a radio button within a group.
        /// </summary>
        Radio,

        /// <summary>
        /// A menu item that acts as a separator between other items.
        /// </summary>
        Separator,

        /// <summary>
        /// Represents a null menu item.
        /// </summary>
        Null,
    }
}
