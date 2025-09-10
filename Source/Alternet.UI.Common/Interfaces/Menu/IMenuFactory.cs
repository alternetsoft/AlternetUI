using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods to create and manage menus and menu items.
    /// </summary>
    public interface IMenuFactory
    {
        /// <summary>
        /// Show menu on screen.
        /// </summary>
        /// <param name="control">The target control.</param>
        /// <param name="position">The position in local coordinates.</param>
        /// <param name="onClose">The action to be invoked when the menu is closed.</param>
        /// <param name="menuHandle">The handle of the context menu to show.</param>
        void Show(
            ContextMenu menuHandle,
            AbstractControl control,
            PointD? position = null,
            Action? onClose = null);

        /// <summary>
        /// Sets the main menu for the specified window.
        /// </summary>
        /// <remarks>This method associates the specified main menu with the given window.
        /// If <paramref name="menu"/> is <see langword="null"/>,  the current main
        /// menu for the window will be removed.</remarks>
        /// <param name="window">The window for which the main menu is being set.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="menu">The main menu to set.
        /// Pass <see langword="null"/> to remove the current main menu.</param>
        void SetMainMenu(Window window, MainMenu? menu);
    }
}
