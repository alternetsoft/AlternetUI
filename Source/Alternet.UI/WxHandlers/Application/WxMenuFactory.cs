using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxMenuFactory : DisposableObject, IMenuFactory
    {
        private Native.Menu nativeMenu;

        public WxMenuFactory()
        {
            nativeMenu = new();
            Native.Menu.GlobalObject = nativeMenu;
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
            Native.Menu.GlobalObject = null;
            base.DisposeManaged();
        }

        public event EventHandler<StringEventArgs>? MenuClick;

        public event EventHandler<StringEventArgs>? MenuHighlight;

        public event EventHandler<StringEventArgs>? MenuOpened;

        public event EventHandler<StringEventArgs>? MenuClosed;

        public IMenuFactory.ContextMenuHandle CreateContextMenu(string id, ContextMenu? menu)
        {
            return new(Native.Menu.CreateContextMenu(id));
        }

        public IMenuFactory.MainMenuHandle CreateMainMenu(string id, MainMenu? menu)
        {
            return new(Native.Menu.CreateMainMenu(id));
        }

        public IMenuFactory.MenuItemHandle CreateMenuItem(MenuItemType itemType, string id, MenuItem? menuItem)
        {
            return new(Native.Menu.CreateMenuItem(itemType, id));
        }

        public virtual void DestroyContextMenu(IMenuFactory.ContextMenuHandle menuHandle)
        {
            var ptr = menuHandle.AsPointer;
            if (ptr == IntPtr.Zero)
                return;
            Native.Menu.DestroyContextMenu(ptr);
        }

        public virtual void DestroyMainMenu(IMenuFactory.MainMenuHandle menuHandle)
        {
            var ptr = menuHandle.AsPointer;
            if (ptr == IntPtr.Zero)
                return;
            Native.Menu.DestroyMainMenu(ptr);
        }

        public virtual void DestroyMenuItem(IMenuFactory.MenuItemHandle menuHandle)
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

        public virtual void MenuAddItem(IMenuFactory.ContextMenuHandle handle, IMenuFactory.MenuItemHandle itemHandle)
        {
            var menuPtr = handle.AsPointer;
            var itemPtr = itemHandle.AsPointer;
            if (menuPtr == IntPtr.Zero || itemPtr == IntPtr.Zero)
                return;
            Native.Menu.MenuAddItem(menuPtr, itemPtr);
        }

        public virtual void MenuRemoveItem(IMenuFactory.ContextMenuHandle handle, IMenuFactory.MenuItemHandle itemHandle)
        {
            var menuPtr = handle.AsPointer;
            var itemPtr = itemHandle.AsPointer;
            if (menuPtr == IntPtr.Zero || itemPtr == IntPtr.Zero)
                return;
            Native.Menu.MenuRemoveItem(menuPtr, itemPtr);
        }

        public virtual void SetMenuItemBitmap(IMenuFactory.MenuItemHandle handle, ImageSet? value)
        {
            var itemPtr = handle.AsPointer;
            var nativeImage = (UI.Native.ImageSet?)value?.Handler;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemBitmap(itemPtr, nativeImage);
        }

        public virtual void SetMenuItemChecked(IMenuFactory.MenuItemHandle handle, bool value)
        {
            var itemPtr = handle.AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemChecked(itemPtr, value);
        }

        public virtual void Show(
            IMenuFactory.ContextMenuHandle menuHandle,
            AbstractControl control,
            PointD? position = null,
            Action? onClose = null)
        {
            var menuPtr = menuHandle.AsPointer;
            if (menuPtr == IntPtr.Zero)
                return;

            if (position == null)
            {
                var window = control.ParentWindow;
                if (window == null)
                    return;
                var toolRect = control.Bounds;
                var pt = control.Parent!.ClientToScreen(toolRect.BottomLeft);
                position = window.ScreenToClient(pt);
            }

            Native.Menu.Show(
                menuPtr,
                (UI.Native.Control)control.NativeControl ?? throw new Exception(),
                position.Value);
        }


        public virtual string GetId(CustomNativeHandle handle)
        {
            var ptr = handle.AsPointer;
            if (ptr == IntPtr.Zero)
                return string.Empty;
            return Native.Menu.GetMenuId(ptr);
        }

        public virtual void SetMenuItemEnabled(IMenuFactory.MenuItemHandle handle, bool value)
        {
            var itemPtr = handle.AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemEnabled(itemPtr, value);
        }

        public virtual void SetMenuItemShortcut(
            IMenuFactory.MenuItemHandle handle,
            Key key,
            ModifierKeys modifierKeys)
        {
            var itemPtr = handle.AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemShortcut(itemPtr, key, modifierKeys);
        }

        public virtual void SetMenuItemSubMenu(
            IMenuFactory.MenuItemHandle handle,
            IMenuFactory.ContextMenuHandle subMenuHandle)
        {
            var itemPtr = handle.AsPointer;
            var subMenuPtr = subMenuHandle.AsPointer;
            if (itemPtr == IntPtr.Zero || subMenuPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemSubMenu(itemPtr, subMenuPtr);
        }

        public virtual void SetMenuItemText(IMenuFactory.MenuItemHandle handle, string value, string rightValue)
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
