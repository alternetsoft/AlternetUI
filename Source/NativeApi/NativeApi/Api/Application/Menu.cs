#pragma warning disable
using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class Menu
    {
        // Used to detect localized Help menu titles
        public NativeStringSpan MacHelpMenuTitleName { get; }

        // Used to identify the Window menu
        public NativeStringSpan MacWindowMenuTitleName { get; }

        public void MacSetCommonMenuBar(IntPtr menuBar) { }

        /* Work with main menu */

        public static IntPtr GetMainMenu(Window window) => throw new Exception();

        public static IntPtr FindMenuItem(Window window, NativeStringSpan id) => throw new Exception();

        public static void SetMainMenu(Window window, IntPtr menu) => throw new Exception();

        public static bool MainMenuAppend(IntPtr menuHandle, IntPtr menu, NativeStringSpan text)
            => throw new Exception();

        public static void MainMenuSetEnabled(IntPtr menuHandle, NativeStringSpan childId, bool enable)
            => throw new Exception();

        public static IntPtr MainMenuGetSubMenu(IntPtr menuHandle, NativeStringSpan childId)
            => throw new Exception();

        public static IntPtr MainMenuRemove(IntPtr menuHandle, NativeStringSpan childId) => throw new Exception();

        public static bool MainMenuInsert(IntPtr menuHandle, NativeStringSpan childId, IntPtr menu, NativeStringSpan title)
            => throw new Exception();

        public static IntPtr MainMenuReplace(IntPtr menuHandle, NativeStringSpan childId, IntPtr menu, NativeStringSpan title)
            => throw new Exception();

        public static void MainMenuSetText(IntPtr menuHandle, NativeStringSpan childId, NativeStringSpan label)
            => throw new Exception();

        /* Create menu and menu items */

        public static IntPtr CreateMainMenu(NativeStringSpan id) => throw new Exception();
        public static IntPtr CreateContextMenu(NativeStringSpan id) => throw new Exception();

        public static IntPtr CreateMenuItem(
            MenuItemType itemType,
            NativeStringSpan id,
            NativeStringSpan title,
            NativeStringSpan help,
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
        public static void SetMenuItemBitmap(IntPtr handle, Image? value) { }
        public static void SetMenuItemEnabled(IntPtr handle, bool value) { }
        public static void SetMenuItemRole(IntPtr handle, NativeStringSpan role) { }

        public static void SetMenuItemText(
            IntPtr handle,
            NativeStringSpan value,
            NativeStringSpan rightValue) { }

        public static void SetMenuItemChecked(IntPtr handle, bool value) { }
        public static void SetMenuItemSubMenu(IntPtr handle, IntPtr subMenuHandle) { }
        public static void SetMenuItemShortcut(IntPtr handle, 
            Key key, ModifierKeys modifierKeys) { }

        /* Work with menu */

        public static void MenuAddItem(IntPtr handle, IntPtr itemHandle) { }
        public static void MenuRemoveItem(IntPtr handle, NativeStringSpan childId) { }
        public static bool MenuInsertItem(IntPtr handle, NativeStringSpan childId, IntPtr itemHandle)
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