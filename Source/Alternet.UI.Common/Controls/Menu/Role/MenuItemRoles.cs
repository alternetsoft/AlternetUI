using System;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a set of standard menu item roles.
    /// </summary>
    /// <remarks>
    /// Menu items roles provide a mechanism for automatically adjusting certain menu items
    /// to macOS conventions.
    /// For example, "About", "Exit", and "Preferences" items should be placed in the
    /// application menu on macOS, and have a standard shortcuts.
    /// </remarks>
    public static class MenuItemRoles
    {
        /// <summary>
        /// No role is defined for the menu item. On macOS, the item will appear exactly
        /// as defined in the menu.
        /// </summary>
        public static readonly MenuItemRole None = new("None");

        /// <summary>
        /// "About" menu item role.
        /// </summary>
        /// <remarks>
        /// On macOS, a menu item with the "About" role is placed in the application
        /// menu with a display text of "About [application name]...".
        /// </remarks>
        public static readonly MenuItemRole About = new("About");

        /// <summary>
        /// "Exit" menu item role.
        /// </summary>
        /// <remarks>
        /// On macOS, a menu item with the "Exit" role is placed in the application
        /// menu with a display text of "Quit" and a shortcut of "Cmd+Q".
        /// </remarks>
        public static readonly MenuItemRole Exit = new("Exit");

        /// <summary>
        /// "Preferences" menu item role.
        /// </summary>
        /// <remarks>
        /// On macOS, a menu item with the "Preferences" role is placed in the application
        /// menu with a display text of "Preferences..." and a shortcut of "Cmd+,".
        /// </remarks>
        public static readonly MenuItemRole Preferences = new("Preferences");
    }
}