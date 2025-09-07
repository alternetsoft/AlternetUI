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
        /// Indicates whether the factory logs menu events.
        /// </summary>
        internal static bool LogFactoryEvents = false;

        private static IMenuFactory? menuFactory;
        private static bool menuFactoryLoaded;

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
                    menuFactory.MenuClick -= OnMenuFactoryClick;
                    menuFactory.MenuHighlight -= OnMenuFactoryHighlight;
                    menuFactory.MenuOpened -= OnMenuFactoryOpened;
                    menuFactory.MenuClosed -= OnMenuFactoryClosed;
                }

                menuFactory = value;
                menuFactoryLoaded = true;

                if (menuFactory is not null)
                {
                    menuFactory.MenuClick += OnMenuFactoryClick;
                    menuFactory.MenuHighlight += OnMenuFactoryHighlight;
                    menuFactory.MenuOpened += OnMenuFactoryOpened;
                    menuFactory.MenuClosed += OnMenuFactoryClosed;
                }
            }
        }

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

        private static void OnMenuFactoryClosed(object? sender, StringEventArgs e)
        {
            var menu = Menu.MenuFromStringId(e.Value);

            LogEvent(menu, "Closed");

            if (menu is null)
                return;

            if (menu is MenuItem menuItem)
            {
                menuItem.RaiseClosed();
                return;
            }

            if (menu is ContextMenu contextMenu)
            {
                contextMenu.RaiseClosing(EventArgs.Empty);
                return;
            }

            if (menu is MainMenu mainMenu)
            {
                return;
            }
        }

        private static void OnMenuFactoryOpened(object? sender, StringEventArgs e)
        {
            var menu = Menu.MenuFromStringId(e.Value);

            LogEvent(menu, "Opened");

            if (menu is null)
                return;

            if (menu is MenuItem menuItem)
            {
                menuItem.RaiseOpened();
                return;
            }

            if (menu is ContextMenu contextMenu)
            {
                return;
            }

            if (menu is MainMenu mainMenu)
            {
                return;
            }
        }

        private static void OnMenuFactoryHighlight(object? sender, StringEventArgs e)
        {
            var menu = Menu.MenuFromStringId(e.Value);

            LogEvent(menu, "Highlight");

            if (menu is null)
                return;

            if (menu is MenuItem menuItem)
            {
                menuItem.RaiseHighlighted();
                return;
            }

            if (menu is ContextMenu contextMenu)
            {
                return;
            }

            if (menu is MainMenu mainMenu)
            {
                return;
            }
        }

        private static void OnMenuFactoryClick(object? sender, StringEventArgs e)
        {
            var menu = Menu.MenuFromStringId(e.Value);

            LogEvent(menu, "Click");

            if (menu is MenuItem menuItem)
            {
                menuItem.RaiseClick();
                return;
            }
        }

        [Conditional("DEBUG")]
        private static void LogEvent(Menu? menu, string message)
        {
            if (!LogFactoryEvents)
                return;

            if (menu is null)
            {
                App.Log($"MenuFactory.{message}: <null>");
            }
            else
            {
                App.Log($"MenuFactory.{message}: {menu.GetType().Name}, '{(menu as MenuItem)?.Text}'");
            }
        }
    }
}
