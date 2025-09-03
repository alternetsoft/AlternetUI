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
        /// The menu item role will be automatically deduced from the item text.
        /// </summary>
        /// <remarks>
        /// Setting <see cref="MenuItem.Role"/> to <see langword="null"/>
        /// has the same effect as the <see cref="Auto"/> value. <see langword="null"/>
        /// is the default <see cref="MenuItem.Role"/> property value.
        /// </remarks>
        public static readonly MenuItemRole Auto = new("Auto");

        /// <summary>
        /// "About" menu item role.
        /// </summary>
        /// <remarks>
        /// This role is deduced from the <see cref="MenuItem.Text"/> when its value
        /// starts with "About".
        /// On macOS, a menu item with the "About" role is placed in the application
        /// menu with a display text of "About [appname]...".
        /// </remarks>
        public static readonly MenuItemRole About = new("About");

        /// <summary>
        /// "Exit" menu item role.
        /// </summary>
        /// <remarks>
        /// This role is deduced from the <see cref="MenuItem.Text"/> when its value
        /// is "Exit" or "Quit".
        /// On macOS, a menu item with the "Exit" role is placed in the application
        /// menu with a display text of "Quit"
        /// and a shortcut of "Cmd+Q".
        /// </remarks>
        public static readonly MenuItemRole Exit = new("Exit");

        /// <summary>
        /// "Preferences" menu item role.
        /// </summary>
        /// <remarks>
        /// This role is deduced from the <see cref="MenuItem.Text"/>
        /// when its value is "Preferences" or "Options" with optional ellipsis at the end.
        /// On macOS, a menu item with the "Preferences" role is placed in the application
        /// menu with a display text of "Preferences..."
        /// and a shortcut of "Cmd+,".
        /// </remarks>
        public static readonly MenuItemRole Preferences = new("Preferences");
    }
}