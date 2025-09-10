using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides functionality for creating, managing, and interacting with menus,
    /// including context menus, main menus, and menu items.
    /// </summary>
    /// <remarks>The <see cref="InnerMenuFactory"/> class implements the <see cref="IMenuFactory"/> interface,
    /// offering methods to create and destroy menus and menu items,
    /// as well as to configure their properties such as
    /// text, shortcuts, and submenus. It also provides event handling for menu interactions, such as clicks,
    /// highlights, and open/close events. <para> This class is designed to be extensible, allowing derived
    /// classes to override its virtual methods to customize behavior.
    /// It also inherits from <see cref="DisposableObject"/>,
    /// ensuring proper resource management. </para></remarks>
    public class InnerMenuFactory : DisposableObject, IMenuFactory
    {
        /// <summary>
        /// Indicates whether the factory logs menu events.
        /// </summary>
        internal static bool LogFactoryEvents = false;

        /// <inheritdoc/>
        public virtual void Show(
            ContextMenu menu,
            AbstractControl control,
            PointD? position = null,
            Action? onClose = null)
        {
            var pos = Mouse.CoercePosition(position, control);

            while (!control.IsPlatformControl)
            {
                if (control.Parent == null)
                    return;
                pos += control.Location;
                control = control.Parent;
            }

            menu?.ShowInsideControl(control, menu.RelatedControl, pos, onClose);
        }

        /// <inheritdoc/>
        public virtual void SetMainMenu(Window window, MainMenu? menu)
        {
        }

        /// <summary>
        /// Calls the <see cref="MenuItem.RaiseClick()"/> method of the <see cref="MenuItem"/> instance
        /// specified by <paramref name="args"/>.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuClick(StringEventArgs args)
        {
            var menu = Menu.MenuFromStringId(args.Value);

            LogEvent(menu, "Click");

            if (menu is MenuItem menuItem)
            {
                menuItem.RaiseClick();
                return;
            }
        }

        /// <summary>
        /// Calls the <see cref="MenuItem.RaiseHighlighted"/> method of the <see cref="MenuItem"/> instance
        /// specified by <paramref name="args"/>.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuHighlight(StringEventArgs args)
        {
            var menu = Menu.MenuFromStringId(args.Value);

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

        /// <summary>
        /// Calls the <see cref="MenuItem.RaiseOpened()"/> method of the <see cref="MenuItem"/> instance
        /// specified by <paramref name="args"/>.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuOpened(StringEventArgs args)
        {
            var menu = Menu.MenuFromStringId(args.Value);

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

        /// <summary>
        /// Calls the <see cref="MenuItem.RaiseClosed()"/> method if instance
        /// specified by <paramref name="args"/> is <see cref="MenuItem"/>.
        /// Calls the <see cref="ContextMenu.RaiseClosing(EventArgs)"/> method if instance
        /// specified by <paramref name="args"/> is <see cref="ContextMenu"/>.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuClosed(StringEventArgs args)
        {
            var menu = Menu.MenuFromStringId(args.Value);

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
