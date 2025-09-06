using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class WxMenuFactory : DisposableObject, IMenuFactory
    {
        private Native.Menu nativeMenu;

        public WxMenuFactory()
        {
            nativeMenu = new();
            nativeMenu.MenuClick += OnNativeMenuClick;
            nativeMenu.MenuHighlight += OnNativeMenuHighlight;
            nativeMenu.MenuOpened += OnNativeMenuOpened;
            nativeMenu.MenuClosed += OnNativeMenuClosed;
        }

        protected override void DisposeManaged()
        {
            nativeMenu.MenuClick -= OnNativeMenuClick;
            nativeMenu.MenuHighlight -= OnNativeMenuHighlight;
            nativeMenu.MenuOpened -= OnNativeMenuOpened;
            nativeMenu.MenuClosed -= OnNativeMenuClosed;
            nativeMenu.Dispose();
            nativeMenu = null!;
            base.DisposeManaged();
        }

        public event EventHandler<StringEventArgs>? MenuClick;

        public event EventHandler<StringEventArgs>? MenuHighlight;

        public event EventHandler<StringEventArgs>? MenuOpened;

        public event EventHandler<StringEventArgs>? MenuClosed;

        public IMenuFactory.ContextMenuHandle CreateContextMenu(string id)
        {
            return new(Native.Menu.CreateContextMenu(id));
        }

        public IMenuFactory.MainMenuHandle CreateMainMenu(string id)
        {
            return new(Native.Menu.CreateMainMenu(id));
        }

        public IMenuFactory.MenuItemHandle CreateMenuItem(MenuItemType itemType, string id)
        {
            return new(Native.Menu.CreateMenuItem(itemType, id));
        }

        public void DestroyContextMenu(IMenuFactory.ContextMenuHandle menuHandle)
        {
            var ptr = menuHandle.AsPointer;
            if (ptr == IntPtr.Zero)
                return;
            Native.Menu.DestroyContextMenu(ptr);
        }

        public void DestroyMainMenu(IMenuFactory.MainMenuHandle menuHandle)
        {
            var ptr = menuHandle.AsPointer;
            if (ptr == IntPtr.Zero)
                return;
            Native.Menu.DestroyMainMenu(ptr);
        }

        public void DestroyMenuItem(IMenuFactory.MenuItemHandle menuHandle)
        {
            var ptr = menuHandle.AsPointer;
            if (ptr == IntPtr.Zero)
                return;
            Native.Menu.DestroyMenuItem(ptr);
        }

        public MenuItemType GetMenuItemType(IMenuFactory.MenuItemHandle handle)
        {
            var ptr = handle.AsPointer;
            if (ptr == IntPtr.Zero)
                return MenuItemType.Null;
            return Native.Menu.GetMenuItemType(ptr);
        }

        public void MenuAddItem(IMenuFactory.ContextMenuHandle handle, IMenuFactory.MenuItemHandle itemHandle)
        {
            var menuPtr = handle.AsPointer;
            var itemPtr = itemHandle.AsPointer;
            if (menuPtr == IntPtr.Zero || itemPtr == IntPtr.Zero)
                return;
            Native.Menu.MenuAddItem(menuPtr, itemPtr);
        }

        public void MenuRemoveItem(IMenuFactory.ContextMenuHandle handle, IMenuFactory.MenuItemHandle itemHandle)
        {
            var menuPtr = handle.AsPointer;
            var itemPtr = itemHandle.AsPointer;
            if (menuPtr == IntPtr.Zero || itemPtr == IntPtr.Zero)
                return;
            Native.Menu.MenuRemoveItem(menuPtr, itemPtr);
        }

        public void SetMenuItemBitmap(IMenuFactory.MenuItemHandle handle, ImageSet? value)
        {
            var itemPtr = handle.AsPointer;
            var nativeImage = (UI.Native.ImageSet?)value?.Handler;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemBitmap(itemPtr, nativeImage);
        }

        public void SetMenuItemChecked(IMenuFactory.MenuItemHandle handle, bool value)
        {
            var itemPtr = handle.AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemChecked(itemPtr, value);
        }

        public void SetMenuItemEnabled(IMenuFactory.MenuItemHandle handle, bool value)
        {
            var itemPtr = handle.AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemEnabled(itemPtr, value);
        }

        public void SetMenuItemSubMenu(IMenuFactory.MenuItemHandle handle, IMenuFactory.ContextMenuHandle subMenuHandle)
        {
            var itemPtr = handle.AsPointer;
            var subMenuPtr = subMenuHandle.AsPointer;
            if (itemPtr == IntPtr.Zero || subMenuPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemSubMenu(itemPtr, subMenuPtr);
        }

        public void SetMenuItemText(IMenuFactory.MenuItemHandle handle, string value, string rightValue)
        {
            var itemPtr = handle.AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemText(itemPtr, value, rightValue);
        }

        protected virtual void OnNativeMenuClosed()
        {
            MenuClosed?.Invoke(this, new StringEventArgs(Native.Menu.EventMenuItemId));
        }

        protected virtual void OnNativeMenuOpened()
        {
            MenuOpened?.Invoke(this, new StringEventArgs(Native.Menu.EventMenuItemId));
        }

        protected virtual void OnNativeMenuHighlight()
        {
            MenuHighlight?.Invoke(this, new StringEventArgs(Native.Menu.EventMenuItemId));
        }

        protected virtual void OnNativeMenuClick()
        {
            MenuClick?.Invoke(this, new StringEventArgs(Native.Menu.EventMenuItemId));
        }
    }
}
