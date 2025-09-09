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
        /// Occurs when a menu item is clicked.
        /// </summary>
        event EventHandler<StringEventArgs>? MenuClick;

        /// <summary>
        /// Occurs when a menu item is highlighted.
        /// </summary>
        event EventHandler<StringEventArgs>? MenuHighlight;

        /// <summary>
        /// Occurs when a menu is opened.
        /// </summary>
        event EventHandler<StringEventArgs>? MenuOpened;

        /// <summary>
        /// Occurs when a menu is closed.
        /// </summary>
        event EventHandler<StringEventArgs>? MenuClosed;

        /// <summary>
        /// Appends a sub-menu to the main menu with the specified text.
        /// </summary>
        /// <param name="menuHandle">The handle of the main menu to append to.</param>
        /// <param name="menu">The sub-menu handle to append.</param>
        /// <param name="text">The text to display for the appended menu.</param>
        /// <returns>True if the menu was appended successfully; otherwise, false.</returns>
        bool MainMenuAppend(MainMenuHandle menuHandle, ContextMenuHandle menu, string text);

        /// <summary>
        /// Gets the number of items in the specified main menu.
        /// </summary>
        /// <param name="menuHandle">The handle of the main menu.</param>
        /// <returns>The number of items in the main menu.</returns>
        int MainMenuGetCount(MainMenuHandle menuHandle);

        /// <summary>
        /// Sets whether the submenu at the specified position in the main menu is enabled.
        /// </summary>
        /// <param name="menuHandle">The handle of the main menu.</param>
        /// <param name="pos">The position of the submenu.</param>
        /// <param name="enable">True to enable the submenu; false to disable.</param>
        void MainMenuSetEnabled(MainMenuHandle menuHandle, int pos, bool enable);

        /// <summary>
        /// Gets the submenu at the specified index from the main menu.
        /// </summary>
        /// <param name="menuHandle">The handle of the main menu.</param>
        /// <param name="menuIndex">The index of the submenu to retrieve.</param>
        /// <returns>The handle of the submenu at the specified index.</returns>
        ContextMenuHandle MainMenuGetSubMenu(MainMenuHandle menuHandle, int menuIndex);

        /// <summary>
        /// Removes the submenu at the specified position from the main menu.
        /// </summary>
        /// <param name="menuHandle">The handle of the main menu.</param>
        /// <param name="pos">The position of the submenu to remove.</param>
        /// <returns>The handle of the removed submenu.</returns>
        ContextMenuHandle MainMenuRemove(MainMenuHandle menuHandle, int pos);

        /// <summary>
        /// Inserts a submenu into the main menu at the specified position with the given title.
        /// </summary>
        /// <param name="menuHandle">The handle of the main menu.</param>
        /// <param name="pos">The position at which to insert the menu.</param>
        /// <param name="menu">The context submenu handle to insert.</param>
        /// <param name="title">The title of the inserted menu.</param>
        /// <returns>True if the submenu was inserted successfully; otherwise, false.</returns>
        bool MainMenuInsert(MainMenuHandle menuHandle, int pos, ContextMenuHandle menu, string title);

        /// <summary>
        /// Replaces the submenu at the specified position in the main menu with a new submenu and title.
        /// </summary>
        /// <param name="menuHandle">The handle of the main menu.</param>
        /// <param name="pos">The position of the submenu to replace.</param>
        /// <param name="menu">The new submenu handle to set.</param>
        /// <param name="title">The title of the new menu.</param>
        /// <returns>The handle of the replaced submenu.</returns>
        ContextMenuHandle MainMenuReplace(MainMenuHandle menuHandle, int pos, ContextMenuHandle menu, string title);

        /// <summary>
        /// Sets the text of the item at the specified position in the main menu.
        /// </summary>
        /// <param name="menuHandle">The handle of the main menu.</param>
        /// <param name="pos">The position of the item.</param>
        /// <param name="label">The new text label for the item.</param>
        void MainMenuSetText(MainMenuHandle menuHandle, int pos, string label);

        /// <summary>
        /// Creates a main menu instance.
        /// </summary>
        /// <returns>The handle to the created main menu.</returns>
        /// <param name="id">The unique identifier for the main menu.</param>
        /// <param name="menu">The main menu instance.</param>
        MainMenuHandle CreateMainMenu(string id, MainMenu? menu);

        /// <summary>
        /// Creates a context menu instance.
        /// </summary>
        /// <returns>The handle to the created context menu.</returns>
        /// <param name="id">The unique identifier for the menu.</param>
        /// <param name="menu">The context menu instance.</param>
        ContextMenuHandle CreateContextMenu(string id, ContextMenu? menu);

        /// <summary>
        /// Creates a menu item of the specified type.
        /// </summary>
        /// <param name="itemType">The type of the menu item to create.</param>
        /// <param name="id">The unique identifier for the menu item.</param>
        /// <param name="menuItem">The menu item instance.</param>
        /// <returns>The handle to the created menu item.</returns>
        MenuItemHandle CreateMenuItem(MenuItemType itemType, string id, MenuItem? menuItem);

        /// <summary>
        /// Show menu on screen.
        /// </summary>
        /// <param name="control">The target control.</param>
        /// <param name="position">The position in local coordinates.</param>
        /// <param name="onClose">The action to be invoked when the menu is closed.</param>
        /// <param name="menuHandle">The handle of the context menu to show.</param>
        void Show(
            ContextMenuHandle menuHandle,
            AbstractControl control,
            PointD? position = null,
            Action? onClose = null);

        /// <summary>
        /// Destroys the specified main menu.
        /// </summary>
        /// <param name="menuHandle">The handle of the main menu to destroy.</param>
        void DestroyMainMenu(MainMenuHandle menuHandle);

        /// <summary>
        /// Destroys the specified menu item.
        /// </summary>
        /// <param name="menuHandle">The handle of the menu item to destroy.</param>
        void DestroyMenuItem(MenuItemHandle menuHandle);

        /// <summary>
        /// Destroys the specified context menu.
        /// </summary>
        /// <param name="menuHandle">The handle of the context menu to destroy.</param>
        void DestroyContextMenu(ContextMenuHandle menuHandle);

        /// <summary>
        /// Gets the type of the menu item associated with the specified handle.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <returns>The type of the menu item.</returns>
        MenuItemType GetMenuItemType(MenuItemHandle handle);

        /// <summary>
        /// Sets the bitmap image for the specified menu item.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <param name="value">The image set to assign.</param>
        void SetMenuItemBitmap(MenuItemHandle handle, ImageSet? value);

        /// <summary>
        /// Sets a keyboard shortcut for the specified menu item.
        /// </summary>
        /// <remarks>The combination of <paramref name="key"/> and <paramref name="modifierKeys"/> defines
        /// the shortcut. Ensure that the shortcut does not conflict with existing shortcuts in the
        /// application.</remarks>
        /// <param name="handle">The handle of the menu item to which the shortcut will be assigned.</param>
        /// <param name="key">The key to be used as the shortcut.</param>
        /// <param name="modifierKeys">The modifier keys (e.g., Ctrl, Alt) to be combined with the shortcut key.</param>
        void SetMenuItemShortcut(MenuItemHandle handle, Key key, ModifierKeys modifierKeys);

        /// <summary>
        /// Sets whether the specified menu item is enabled.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <param name="value">True to enable, false to disable.</param>
        void SetMenuItemEnabled(MenuItemHandle handle, bool value);

        /// <summary>
        /// Retrieves the unique identifier associated with the specified native handle.
        /// </summary>
        /// <param name="handle">The <see cref="CustomNativeHandle"/> representing the native
        /// resource for which the identifier is retrieved.
        /// Cannot be null.</param>
        /// <returns>A string containing the unique identifier associated with the specified handle.</returns>
        string GetId(CustomNativeHandle handle);

        /// <summary>
        /// Sets the text and optional right-aligned text for the specified menu item.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <param name="value">The main text of the menu item.</param>
        /// <param name="rightValue">The right-aligned text (e.g., shortcut).</param>
        void SetMenuItemText(MenuItemHandle handle, string value, string rightValue);

        /// <summary>
        /// Sets whether the specified menu item is checked.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <param name="value">True to check, false to uncheck.</param>
        void SetMenuItemChecked(MenuItemHandle handle, bool value);

        /// <summary>
        /// Sets a submenu for the specified menu item.
        /// </summary>
        /// <param name="handle">The handle of the menu item.</param>
        /// <param name="subMenuHandle">The handle of the submenu to assign.</param>
        void SetMenuItemSubMenu(MenuItemHandle handle, ContextMenuHandle subMenuHandle);

        /// <summary>
        /// Adds a menu item to the specified context menu.
        /// </summary>
        /// <param name="handle">The handle of the context menu.</param>
        /// <param name="itemHandle">The handle of the menu item to add.</param>
        void MenuAddItem(ContextMenuHandle handle, MenuItemHandle itemHandle);

        /// <summary>
        /// Removes a menu item from the specified context menu.
        /// </summary>
        /// <param name="handle">The handle of the context menu.</param>
        /// <param name="itemHandle">The handle of the menu item to remove.</param>
        void MenuRemoveItem(ContextMenuHandle handle, MenuItemHandle itemHandle);

        /// <summary>
        /// Represents a handle to a main menu.
        /// </summary>
        public class MainMenuHandle : CustomNativeHandle
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MainMenuHandle"/> class.
            /// </summary>
            public MainMenuHandle()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="MainMenuHandle"/> class with the specified handle.
            /// </summary>
            /// <param name="handle">The native handle object to wrap.</param>
            public MainMenuHandle(object handle)
                : base(handle)
            {
            }
        }

        /// <summary>
        /// Represents a handle to a menu item.
        /// </summary>
        public class MenuItemHandle : CustomNativeHandle
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MenuItemHandle"/> class.
            /// </summary>
            public MenuItemHandle()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="MenuItemHandle"/> class with the specified handle.
            /// </summary>
            /// <param name="handle">The native handle object to wrap.</param>
            public MenuItemHandle(object handle)
                : base(handle)
            {
            }
        }

        /// <summary>
        /// Represents a handle to a context menu.
        /// </summary>
        public class ContextMenuHandle : CustomNativeHandle
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ContextMenuHandle"/> class.
            /// </summary>
            public ContextMenuHandle()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ContextMenuHandle"/> class with the specified handle.
            /// </summary>
            /// <param name="handle">The native handle object to wrap.</param>
            public ContextMenuHandle(object handle)
                : base(handle)
            {
            }
        }
    }
}
