using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a menu item role.
    /// </summary>
    /// <remarks>
    /// Menu items roles provide a mechanism for automatically adjusting certain menu items to macOS conventions.
    /// For example, "About", "Exit", and "Preferences" items should be placed in the application menu on macOS, and have a standard shortcuts.
    /// For more information, see <see cref="MenuItemRoles"/> class members.
    /// </remarks>
    [TypeConverter(typeof(MenuItemRoleConverter))]
    public class MenuItemRole
    {
        /// <summary>
        /// Initializes and instance of <see cref="MenuItemRole"/> class.
        /// </summary>
        /// <param name="name">The name of the role.</param>
        public MenuItemRole(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            Name = name;
        }

        /// <summary>
        /// Gets or sets the name for the role.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as MenuItemRole;
            if (other == null)
                return false;

            return Name == other.Name;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => Name.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}