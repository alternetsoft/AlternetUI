using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the role type for a menu item.
    /// </summary>
    public enum MenuItemRoleType
    {
        /// <summary>
        /// No specific role assigned.
        /// </summary>
        None,

        /// <summary>
        /// Represents an "About" menu item.
        /// </summary>
        About,

        /// <summary>
        /// Represents an "Exit" menu item.
        /// </summary>
        Exit,

        /// <summary>
        /// Represents a "Preferences" menu item.
        /// </summary>
        Preferences,

        /// <summary>
        /// Represents any other menu item role which is not explicitly defined.
        /// </summary>
        Other,
    }
}
