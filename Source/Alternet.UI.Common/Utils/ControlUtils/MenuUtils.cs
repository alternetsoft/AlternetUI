using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods for working with menus and menu items.
    /// </summary>
    public static class MenuUtils
    {
        /// <summary>
        /// Attaches event handlers to the specified <see cref="MenuItem"/> to log its Opened, Closed,
        /// and Highlighted events.
        /// </summary>
        /// <remarks>This method ensures that the <see cref="MenuItem"/> logs debug messages whenever it
        /// is opened, closed, or highlighted.</remarks>
        /// <param name="menuItem">The <see cref="MenuItem"/> to which the event handlers will be bound.
        /// Cannot be <see langword="null"/>.</param>
        [Conditional("DEBUG")]
        public static void BindMenuItemEventsLogger(MenuItem? menuItem)
        {
            if (menuItem == null)
                return;

            menuItem.Opened -= MenuItemOpened;
            menuItem.Opened += MenuItemOpened;
            menuItem.Closed -= MenuItemClosed;
            menuItem.Closed += MenuItemClosed;
            menuItem.Highlighted -= MenuItemHighlighted;
            menuItem.Highlighted += MenuItemHighlighted;

            void MenuItemHighlighted(object? sender, EventArgs e)
            {
                Alternet.UI.App.DebugLogIf($"Menu item '{menuItem.Text}' highlighted", false);
            }

            void MenuItemOpened(object? sender, EventArgs e)
            {
                Alternet.UI.App.DebugLogIf($"Menu item '{menuItem.Text}' opened", false);
            }

            void MenuItemClosed(object? sender, EventArgs e)
            {
                Alternet.UI.App.DebugLogIf($"Menu item '{menuItem.Text}' closed", false);
            }
        }

        /// <summary>
        /// Attaches debug logging handlers to the specified context menu's events.
        /// </summary>
        /// <remarks>This method binds handlers to the <see cref="Alternet.UI.ContextMenu.Opening"/>,
        /// <see cref="Alternet.UI.ContextMenu.Closing"/>, and <see cref="Alternet.UI.ContextMenu.Closed"/>
        /// events to log debug messages when these events occur. The method is only executed in debug builds,
        /// as it is marked
        /// with the <see cref="System.Diagnostics.ConditionalAttribute"/> for the "DEBUG" symbol.</remarks>
        /// <param name="menu">The <see cref="Alternet.UI.ContextMenu"/> instance to which
        /// the event handlers will be bound.</param>
        [Conditional("DEBUG")]
        public static void BindContextMenuEventsLogger(Alternet.UI.ContextMenu? menu)
        {
            if (menu == null)
                return;

            menu.Opening -= MenuOpening;
            menu.Opening += MenuOpening;
            menu.Closing -= MenuClosing;
            menu.Closing += MenuClosing;
            menu.Closed -= MenuClosed;
            menu.Closed += MenuClosed;

            void MenuOpening(object? sender, EventArgs e)
            {
                Alternet.UI.App.DebugLogIf("Editor context menu opening", false);
            }

            void MenuClosing(object? sender, EventArgs e)
            {
                Alternet.UI.App.DebugLogIf("Editor context menu closing", false);
            }

            void MenuClosed(object? sender, EventArgs e)
            {
                Alternet.UI.App.DebugLogIf("Editor context menu closed", false);
            }
        }
    }
}
