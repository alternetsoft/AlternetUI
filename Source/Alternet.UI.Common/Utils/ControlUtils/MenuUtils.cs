using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods for working with menus and menu items.
    /// </summary>
    public static class MenuUtils
    {
        private static IMenuFactory? menuFactory;
        private static bool menuFactoryLoaded;

        /// <summary>
        /// Specifies flags that control the behavior of binding menu item event loggers.
        /// </summary>
        [Flags]
        public enum BindMenuLoggerFlags
        {
            /// <summary>
            /// No special binding behavior.
            /// </summary>
            None = 0,

            /// <summary>
            /// Bind event loggers recursively to all child menu items.
            /// </summary>
            Recursive = 1,

            /// <summary>
            /// Unbind event loggers from the menu item(s).
            /// </summary>
            Unbind = 2,
        }

        /// <summary>
        /// Gets or sets the factory responsible for creating menu instances.
        /// </summary>
        /// <remarks>The property initializes the factory on first access if it has not been explicitly
        /// set and a valid application handler is available. Once set, the factory remains loaded until explicitly
        /// replaced.</remarks>
        public static IMenuFactory? Factory
        {
            get
            {
                if (menuFactory != null || menuFactoryLoaded)
                    return menuFactory;

                var handler = App.Handler;

                if (handler == null)
                    return null;

                Factory = handler.CreateMenuFactory();
                return menuFactory;
            }

            set
            {
                if (menuFactory is not null)
                {
                }

                menuFactory = value;
                menuFactoryLoaded = true;

                if (menuFactory is not null)
                {
                }
            }
        }

        /// <summary>
        /// Ensures that the required dependencies or configurations are initialized.
        /// </summary>
        /// <remarks>This method is typically used to verify that necessary components are set up before
        /// proceeding with further operations.</remarks>
        public static void Required()
        {
#pragma warning disable
            var factory = Factory;
#pragma warning restore
        }

        /// <summary>
        /// Attaches event handlers to the specified <see cref="MenuItem"/> to log its Opened, Closed,
        /// and Highlighted events.
        /// </summary>
        /// <remarks>This method ensures that the <see cref="MenuItem"/> logs debug messages whenever it
        /// is opened, closed, or highlighted.</remarks>
        /// <param name="menuItem">The <see cref="MenuItem"/> to which the event handlers will be bound.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="flags">The flags that specify the binding behavior.</param>
        [Conditional("DEBUG")]
        public static void BindMenuItemEventsLogger(
            MenuItem? menuItem,
            BindMenuLoggerFlags flags = BindMenuLoggerFlags.Recursive)
        {
            if (menuItem == null)
                return;

            menuItem.Opened -= MenuItemOpened;
            menuItem.Closed -= MenuItemClosed;
            menuItem.Highlighted -= MenuItemHighlighted;

            if (flags.HasFlag(BindMenuLoggerFlags.Unbind))
                return;

            menuItem.Opened += MenuItemOpened;
            menuItem.Closed += MenuItemClosed;
            menuItem.Highlighted += MenuItemHighlighted;

            void MenuItemHighlighted(object? sender, EventArgs e)
            {
                Alternet.UI.App.DebugLogIf($"Menu item '{menuItem.Text}' highlighted", true);
            }

            void MenuItemOpened(object? sender, EventArgs e)
            {
                var s = $"Menu item '{menuItem.Text}' opened";
                Alternet.UI.App.DebugLogIf(s, true);
            }

            void MenuItemClosed(object? sender, EventArgs e)
            {
                Alternet.UI.App.DebugLogIf($"Menu item '{menuItem.Text}' closed", true);
            }

            if (!flags.HasFlag(BindMenuLoggerFlags.Recursive))
                return;

            if (!menuItem.HasItems)
                return;

            foreach (var item in menuItem.Items)
            {
                BindMenuItemEventsLogger(item, flags);
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
        /// <param name="flags">The flags that specify the binding behavior.</param>
        [Conditional("DEBUG")]
        public static void BindContextMenuEventsLogger(
            Alternet.UI.ContextMenu? menu,
            BindMenuLoggerFlags flags = BindMenuLoggerFlags.Recursive)
        {
            if (menu == null)
                return;

            menu.Opening -= MenuOpening;
            menu.Closing -= MenuClosing;
            menu.Closed -= MenuClosed;

            if (flags.HasFlag(BindMenuLoggerFlags.Unbind))
                return;

            menu.Opening += MenuOpening;
            menu.Closing += MenuClosing;
            menu.Closed += MenuClosed;

            void MenuOpening(object? sender, EventArgs e)
            {
                Alternet.UI.App.DebugLogIf("Context menu opening", true);
            }

            void MenuClosing(object? sender, EventArgs e)
            {
                Alternet.UI.App.DebugLogIf("Context menu closing", true);
            }

            void MenuClosed(object? sender, EventArgs e)
            {
                Alternet.UI.App.DebugLogIf("Context menu closed", true);
            }

            if (!flags.HasFlag(BindMenuLoggerFlags.Recursive))
                return;

            if (!menu.HasItems)
                return;

            foreach (var item in menu.Items)
            {
                BindMenuItemEventsLogger(item, flags);
            }
        }
    }
}
