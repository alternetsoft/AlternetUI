using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines read-only properties for a menu item.
    /// </summary>
    public interface IMenuItemProperties : IMenuProperties
    {
        /// <summary>
        /// Occurs when the state of a menu item changes.
        /// </summary>
        /// <remarks>This event is triggered whenever a menu item undergoes a change,
        /// such as being added, removed, or updated.
        /// The event provides information about the type of change through the
        /// <see cref="MenuItemChangeKind"/> parameter.</remarks>
        event EventHandler<BaseEventArgs<MenuItemChangeKind>>? Changed;

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
        /// Gets the text displayed for the menu item.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets a value indicating whether the menu item is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Gets a value indicating whether the menu item is visible.
        /// </summary>
        bool Visible { get; }

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
        /// Simulates a click action, triggering any associated event handlers or behaviors.
        /// </summary>
        /// <remarks>This method is typically used to programmatically invoke a click event.
        /// Ensure that any required preconditions for the click action are met before calling this
        /// method.</remarks>
        void DoClick();

        /// <summary>
        /// Retrieves the real image associated with the specified visual control state.
        /// </summary>
        /// <param name="state">The visual control state for which the image is to be retrieved.</param>
        /// <param name="isDark">An optional parameter indicating whether the dark mode
        /// variant of the image should be retrieved. If <see
        /// langword="null"/>, the default variant is used.</param>
        /// <param name="control">An optional parameter specifying the control
        /// context for which the image is being retrieved.
        /// If <see langword="null"/>, the image is retrieved without a specific control context.</param>
        /// <returns>The <see cref="ImageSet"/> associated with the specified state,
        /// or <see langword="null"/> if no image is available for the given parameters.</returns>
        ImageSet? GetRealImage(VisualControlState state, bool? isDark = null, Control? control = null);
    }
}