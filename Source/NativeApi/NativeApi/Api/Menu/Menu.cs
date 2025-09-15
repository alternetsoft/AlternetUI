#pragma warning disable
using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class Menu
    {
        /* Work with main menu */

        public static IntPtr GetMainMenu(Window window) => throw new Exception();

        public static IntPtr FindMenuItem(Window window, string id) => throw new Exception();

        public static void SetMainMenu(Window window, IntPtr menu) => throw new Exception();

        public static bool MainMenuAppend(IntPtr menuHandle, IntPtr menu, string text)
            => throw new Exception();

        public static void MainMenuSetEnabled(IntPtr menuHandle, string childId, bool enable)
            => throw new Exception();

        public static IntPtr MainMenuGetSubMenu(IntPtr menuHandle, string childId)
            => throw new Exception();

        public static IntPtr MainMenuRemove(IntPtr menuHandle, string childId) => throw new Exception();

        public static bool MainMenuInsert(IntPtr menuHandle, string childId, IntPtr menu, string title)
            => throw new Exception();

        public static IntPtr MainMenuReplace(IntPtr menuHandle, string childId, IntPtr menu, string title)
            => throw new Exception();

        public static void MainMenuSetText(IntPtr menuHandle, string childId, string label)
            => throw new Exception();

        /* Create menu and menu items */

        public static IntPtr CreateMainMenu(string id) => throw new Exception();
        public static IntPtr CreateContextMenu(string id) => throw new Exception();

        public static IntPtr CreateMenuItem(
            MenuItemType itemType,
            string id,
            string title,
            string help,
            IntPtr menuHandle)
            => throw new Exception();

        /* Destroy menu and menu items */

        public static void DestroyMainMenu(IntPtr menuHandle) => throw new Exception();
        public static void DestroyMenuItem(IntPtr menuHandle) => throw new Exception();
        public static void DestroyContextMenu(IntPtr menuHandle) => throw new Exception();

        /* Common methods */

        public static string GetMenuId(IntPtr handle) => throw new Exception();

        /* Work with menu item */

        public static MenuItemType GetMenuItemType(IntPtr handle) => default;
        public static void SetMenuItemBitmap(IntPtr handle, ImageSet? value) { }
        public static void SetMenuItemEnabled(IntPtr handle, bool value) { }
        public static void SetMenuItemRole(IntPtr handle, string role) { }

        public static void SetMenuItemText(
            IntPtr handle,
            string value,
            string rightValue) { }

        public static void SetMenuItemChecked(IntPtr handle, bool value) { }
        public static void SetMenuItemSubMenu(IntPtr handle, IntPtr subMenuHandle) { }
        public static void SetMenuItemShortcut(IntPtr handle, 
            Key key, ModifierKeys modifierKeys) { }

        /* Work with menu */

        public static void MenuAddItem(IntPtr handle, IntPtr itemHandle) { }
        public static void MenuRemoveItem(IntPtr handle, string childId) { }
        public static bool MenuInsertItem(IntPtr handle, string childId, IntPtr itemHandle)
            => throw new Exception();

        public static void Show(
            IntPtr menuHandle,
            Control control,
            PointD position)
        { }

        /* Events */

        public event EventHandler? MenuClick;
        public event EventHandler? MenuHighlight;
        public event EventHandler? MenuOpened;
        public event EventHandler? MenuClosed;
        public event EventHandler? MenuDestroying;

        public static string EventMenuItemId { get; }
        public static bool EventMenuItemChecked { get; }
    }
}