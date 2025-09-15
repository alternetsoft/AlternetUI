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

        /// <summary>
        /// Maps a nullable <see cref="MenuItemRole"/> to its corresponding
        /// <see cref="MenuItemRoleType"/> value.
        /// </summary>
        /// <param name="role">The nullable <see cref="MenuItemRole"/> to map.
        /// If <see langword="null"/>, the method returns <see cref="MenuItemRoleType.None"/>.</param>
        /// <returns>A <see cref="MenuItemRoleType"/> value that corresponds to the
        /// specified <paramref name="role"/>. If
        /// <paramref name="role"/> is <see langword="null"/>, the method returns
        /// <see cref="MenuItemRoleType.None"/>.
        /// If the <paramref name="role"/> does not match any predefined mapping, the method returns
        /// <see cref="MenuItemRoleType.Other"/>.</returns>
        public static MenuItemRoleType GetRoleType(MenuItemRole? role)
        {
            if (role == null)
                return MenuItemRoleType.None;

            if (role == About)
                return MenuItemRoleType.About;
            else
            if (role == Exit)
                return MenuItemRoleType.Exit;
            else
            if (role == Preferences)
                return MenuItemRoleType.Preferences;
            else
            if (role == None)
                return MenuItemRoleType.None;
            return MenuItemRoleType.Other;
        }
    }
}