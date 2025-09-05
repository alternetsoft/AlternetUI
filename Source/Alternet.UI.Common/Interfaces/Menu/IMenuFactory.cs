using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods to create and manage menus and menu items.
    /// </summary>
    public interface IMenuFactory
    {
        /// <summary>
        /// Occurs when a menu item is clicked.
        /// </summary>
        event EventHandler<BaseEventArgs<string>>? MenuClick;

        /// <summary>
        /// Occurs when a menu item is highlighted.
        /// </summary>
        event EventHandler<BaseEventArgs<string>>? MenuHighlight;

        /// <summary>
        /// Occurs when a menu is opened.
        /// </summary>
        event EventHandler<BaseEventArgs<string>>? MenuOpened;

        /// <summary>
        /// Occurs when a menu is closed.
        /// </summary>
        event EventHandler<BaseEventArgs<string>>? MenuClosed;

        /// <summary>
        /// Creates a main menu instance.
        /// </summary>
        /// <returns>The handle to the created main menu.</returns>
        object CreateMainMenu();

        /// <summary>
        /// Creates a context menu instance.
        /// </summary>
        /// <returns>The handle to the created context menu.</returns>
        object CreateContextMenu();

        /// <summary>
        /// Creates a menu item of the specified type.
        /// </summary>
        /// <param name="itemType">The type of the menu item to create.</param>
        /// <returns>The handle to the created menu item.</returns>
        object CreateMenuItem(MenuItemType itemType);

        /// <summary>
        /// Destroys the specified main menu.
        /// </summary>
        /// <param name="menuHandle">The handle of the main menu to destroy.</param>
        void DestroyMainMenu(object menuHandle);

        /// <summary>
        /// Destroys the specified menu item.
        /// </summary>
        /// <param name="menuHandle">The handle of the menu item to destroy.</param>
        void DestroyMenuItem(object menuHandle);

        /// <summary>
        /// Destroys the specified context menu.
        /// </summary>
        /// <param name="menuHandle">The handle of the context menu to destroy.</param>
        void DestroyContextMenu(object menuHandle);

        /// <summary>
        /// Gets the type of the menu item associated with the specified handle.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <returns>The type of the menu item.</returns>
        MenuItemType GetMenuItemType(object handle);

        /// <summary>
        /// Sets the bitmap image for the specified menu item.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <param name="value">The image set to assign.</param>
        void SetMenuItemBitmap(object handle, ImageSet value);

        /// <summary>
        /// Sets whether the specified menu item is enabled.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <param name="value">True to enable, false to disable.</param>
        void SetMenuItemEnabled(object handle, bool value);

        /// <summary>
        /// Sets the text and optional right-aligned text for the specified menu item.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <param name="value">The main text of the menu item.</param>
        /// <param name="rightValue">The right-aligned text (e.g., shortcut).</param>
        void SetMenuItemText(object handle, string value, string rightValue);

        /// <summary>
        /// Sets whether the specified menu item is checked.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <param name="value">True to check, false to uncheck.</param>
        void SetMenuItemChecked(object handle, bool value);

        /// <summary>
        /// Sets the identifier for the specified menu item.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <param name="id">The identifier to assign.</param>
        void SetMenuItemId(object handle, string id);

        /// <summary>
        /// Sets a submenu for the specified menu item.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <param name="subMenuHandle">The handle of the submenu to assign.</param>
        void SetMenuItemSubMenu(object handle, object subMenuHandle);

        /// <summary>
        /// Adds a menu item to the specified menu.
        /// </summary>
        /// <param name="handle">The handle of the menu.</param>
        /// <param name="itemHandle">The handle of the menu item to add.</param>
        void MenuAddItem(object handle, object itemHandle);

        /// <summary>
        /// Removes a menu item from the specified menu.
        /// </summary>
        /// <param name="handle">The handle of the menu.</param>
        /// <param name="itemHandle">The handle of the menu item to remove.</param>
        void MenuRemoveItem(object handle, object itemHandle);
    }
}
