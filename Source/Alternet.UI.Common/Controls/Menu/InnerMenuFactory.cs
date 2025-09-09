using System;
using System.Collections.Generic;
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
        /// <inheritdoc/>
        public event EventHandler<StringEventArgs>? MenuClick;

        /// <inheritdoc/>
        public event EventHandler<StringEventArgs>? MenuHighlight;

        /// <inheritdoc/>
        public event EventHandler<StringEventArgs>? MenuOpened;

        /// <inheritdoc/>
        public event EventHandler<StringEventArgs>? MenuClosed;

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
        public virtual void SetMainMenu(Window window, MainMenu? menuHandle)
        {
        }

        /// <summary>
        /// Raises the <see cref="MenuClick"/> event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuClick(StringEventArgs args)
        {
            MenuClick?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="MenuHighlight"/> event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuHighlight(StringEventArgs args)
        {
            MenuHighlight?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="MenuOpened"/> event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuOpened(StringEventArgs args)
        {
            MenuOpened?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="MenuClosed"/> event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuClosed(StringEventArgs args)
        {
            MenuClosed?.Invoke(this, args);
        }
    }
}
