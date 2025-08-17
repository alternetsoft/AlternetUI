using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines read-only properties for a menu item.
    /// </summary>
    public interface IReadOnlyMenuItemProperties
    {
        /// <summary>
        /// Gets the shortcut key gesture associated with the menu item.
        /// </summary>
        KeyGesture? Shortcut { get; }

        /// <summary>
        /// Gets the image displayed for the menu item.
        /// </summary>
        ImageSet? Image { get; }

        /// <summary>
        /// Gets the image displayed when the menu item is disabled.
        /// </summary>
        ImageSet? DisabledImage { get; }

        /// <summary>
        /// Gets the SVG image associated with the menu item.
        /// </summary>
        SvgImage? SvgImage { get; }

        /// <summary>
        /// Gets the size of the SVG image.
        /// </summary>
        SizeI? SvgImageSize { get; }

        /// <summary>
        /// Gets the role of the menu item.
        /// </summary>
        MenuItemRole? Role { get; }

        /// <summary>
        /// Gets the action to execute when the menu item is clicked.
        /// </summary>
        Action? ClickAction { get; }

        /// <summary>
        /// Gets the text displayed for the menu item.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets a value indicating whether the menu item is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Gets a value indicating whether the shortcut is enabled for the menu item.
        /// </summary>
        bool IsShortcutEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether the menu item is checked.
        /// </summary>
        bool Checked { get; }

        /// <summary>
        /// Gets the command source associated with the menu item.
        /// </summary>
        ICommandSource CommandSource { get; }

        /// <summary>
        /// Gets a value indicating whether the child items collection contains any elements.
        /// </summary>
        bool HasItems { get; }

        /// <summary>
        /// Gets the total number of child items currently in the collection.
        /// </summary>
        int ItemCount { get; }

        /// <summary>
        /// Retrieves the child menu item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the child menu item to retrieve.
        /// Must be within the valid range of the child menu items collection.</param>
        /// <returns>An object implementing <see cref="IReadOnlyMenuItemProperties"/>
        /// that represents the menu item at the specified index.</returns>
        IReadOnlyMenuItemProperties GetItem(int index);

        /// <summary>
        /// Simulates a click action, triggering any associated event handlers or behaviors.
        /// </summary>
        /// <remarks>This method is typically used to programmatically invoke a click event.
        /// Ensure that any required preconditions for the click action are met before calling this
        /// method.</remarks>
        void DoClick();
    }
}