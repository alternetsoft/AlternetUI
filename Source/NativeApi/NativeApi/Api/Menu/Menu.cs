#pragma warning disable
using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class Menu
    {
        public static void Show(
            IntPtr menuHandle,
            Control control,
            PointD position)
        { }

        public static IntPtr CreateMainMenu(string id) => throw new Exception();
        public static IntPtr CreateContextMenu(string id) => throw new Exception();
        public static IntPtr CreateMenuItem(MenuItemType itemType, string id) => throw new Exception();

        public static void DestroyMainMenu(IntPtr menuHandle) => throw new Exception();
        public static void DestroyMenuItem(IntPtr menuHandle) => throw new Exception();
        public static void DestroyContextMenu(IntPtr menuHandle) => throw new Exception();

        public static MenuItemType GetMenuItemType(IntPtr handle) => default;

        public static string GetMenuId(IntPtr handle) => throw new Exception();

        public static void SetMenuItemBitmap(IntPtr handle, ImageSet? value) { }
        public static void SetMenuItemEnabled(IntPtr handle, bool value) { }
        public static void SetMenuItemText(IntPtr handle, string value, string rightValue) { }
        public static void SetMenuItemChecked(IntPtr handle, bool value) { }
        public static void SetMenuItemSubMenu(IntPtr handle, IntPtr subMenuHandle) { }

        public static void SetMenuItemShortcut(IntPtr handle, Key key, ModifierKeys modifierKeys) { }

        public static void MenuAddItem(IntPtr handle, IntPtr itemHandle) { }
        public static void MenuRemoveItem(IntPtr handle, IntPtr itemHandle) { }

        public event EventHandler? MenuClick;
        public event EventHandler? MenuHighlight;
        public event EventHandler? MenuOpened;
        public event EventHandler? MenuClosed;

        public static string EventMenuItemId { get; }
     }
}