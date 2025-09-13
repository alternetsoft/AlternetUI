using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxMenuFactory : InnerMenuFactory, IMenuFactory
    {
        private Native.Menu nativeMenu;

        static WxMenuFactory()
        {
            StaticMenuEvents.ItemEnabledChanged += (s, e) =>
            {
                if (s is not MenuItem item)
                    return;
                var itemHandles = GetItemHandles(item);

                foreach (var itemHandle in itemHandles)
                    CurrentFactory?.SetMenuItemEnabled(itemHandle, item.Enabled);
            };

            StaticMenuEvents.ItemTextChanged += (s, e) =>
            {
                if (s is not MenuItem item)
                    return;
                var itemHandles = GetItemHandles(item);
                foreach (var itemHandle in itemHandles)
                    CurrentFactory?.SetMenuItemTextAndShortcut(itemHandle, item);
            };

            StaticMenuEvents.ItemShortcutChanged += (s, e) =>
            {
                if (s is not MenuItem item)
                    return;
                var itemHandles = GetItemHandles(item);
                foreach (var itemHandle in itemHandles)
                    CurrentFactory?.SetMenuItemTextAndShortcut(itemHandle, item);
            };

            StaticMenuEvents.ItemCheckedChanged += (s, e) =>
            {
                if (s is not MenuItem item)
                    return;
                var itemHandles = GetItemHandles(item);
                foreach (var itemHandle in itemHandles)
                    CurrentFactory?.SetMenuItemChecked(itemHandle, item.Checked && !item.HasItems);
            };

            StaticMenuEvents.ItemRoleChanged += (s, e) =>
            {
                if (s is not MenuItem item)
                    return;
                var itemHandles = GetItemHandles(item);
                foreach (var itemHandle in itemHandles)
                    CurrentFactory?.SetMenuItemRole(itemHandle, item.Role?.Name ?? string.Empty);
            };

            StaticMenuEvents.ItemImageChanged += (s, e) =>
            {
                if (s is not MenuItem item)
                    return;
                var itemHandles = GetItemHandles(item);
                foreach (var itemHandle in itemHandles)
                    CurrentFactory?.SetMenuItemBitmap(itemHandle, item.Image);
            };

            StaticMenuEvents.ItemVisibleChanged += (s, e) =>
            {
                if (s is not MenuItem item || item.LogicalParent is null)
                    return;

                if (item.Visible)
                {
                    ShowItem(item);
                }
                else
                {
                    HideItem(item);
                }
            };

            StaticMenuEvents.ItemRemoved += (s, e) =>
            {
                if (e.Item is not MenuItem item)
                    return;
                HideItem(item);
            };

            StaticMenuEvents.ItemInserted += (s, e) =>
            {
                if (e.Item is not MenuItem item)
                    return;
                ShowItem(item);
            };

            StaticMenuEvents.ItemChanged += (s, e) =>
            {
            };

            void ShowItem(MenuItem item)
            {
                if (item.HasMainMenuParent)
                {
                    ShowItemInMainMenu();
                }
                else
                {
                    ShowItemInContextMenu();
                }

                void ShowItemInMainMenu()
                {
                }

                void ShowItemInContextMenu()
                {
                }
            }

            void HideItem(MenuItem item)
            {
                if (item.HasMainMenuParent)
                {
                    HideItemInMainMenu();
                }
                else
                {
                    HideItemInContextMenu();
                }

                void HideItemInMainMenu()
                {
                    var menuBarHandles = item.MenuBar?.GetHostObjects<MainMenuHandle>() ?? [];
                    var id = item.ItemsMenu.UniqueId.ToString();
                    foreach (var menuBarHandle in menuBarHandles)
                    {
                        CurrentFactory?.MainMenuRemove(menuBarHandle, id);
                    }
                }

                void HideItemInContextMenu()
                {
                    if (item.LogicalParent is not ContextMenu contextMenu)
                        return;
                    var contextMenuHandles = contextMenu.GetHostObjects<ContextMenuHandle>();
                    var id = item.UniqueId.ToString();

                    foreach (var menuHandle in contextMenuHandles)
                    {
                        CurrentFactory?.MenuRemoveItem(menuHandle, id);
                    }
                }
            }
        }

        public WxMenuFactory()
        {
            nativeMenu = new();
            Native.Menu.GlobalObject = nativeMenu;
            nativeMenu.MenuClick += OnNativeMenuClick;
            nativeMenu.MenuHighlight += OnNativeMenuHighlight;
            nativeMenu.MenuOpened += OnNativeMenuOpened;
            nativeMenu.MenuClosed += OnNativeMenuClosed;
            nativeMenu.MenuDestroying += OnNativeMenuDestroying;
        }

        protected override void DisposeManaged()
        {
            nativeMenu.MenuClick -= OnNativeMenuClick;
            nativeMenu.MenuHighlight -= OnNativeMenuHighlight;
            nativeMenu.MenuOpened -= OnNativeMenuOpened;
            nativeMenu.MenuClosed -= OnNativeMenuClosed;
            nativeMenu.MenuDestroying -= OnNativeMenuDestroying;
            nativeMenu.Dispose();
            nativeMenu = null!;
            Native.Menu.GlobalObject = null;
            base.DisposeManaged();
        }

        public static WxMenuFactory? CurrentFactory => MenuUtils.Factory as WxMenuFactory;

        public static MenuItemHandle? MenuItemHandleFromId(Window window, string id)
        {
            if (window is null)
                return null;
            var handler = window.Handler as WindowHandler;
            if (handler?.NativeControl is not UI.Native.Window nativeWindow)
                return null;
            var itemPtr = Native.Menu.FindMenuItem(nativeWindow, id);
            if (itemPtr == IntPtr.Zero)
                return null;
            return new MenuItemHandle(itemPtr);
        }

        public static MenuItemHandle? FindMenuItem(Window window, string id)
        {
            var handler = window.Handler as WindowHandler;
            if (handler?.NativeControl is not UI.Native.Window nativeWindow)
                return null;
            var itemPtr = Native.Menu.FindMenuItem(nativeWindow, id);
            if (itemPtr == IntPtr.Zero)
                return null;
            return new MenuItemHandle(itemPtr);
        }

        public static IEnumerable<MenuItemHandle> GetItemHandles(MenuItem item)
        {
            if (item is null || item.HostObjects is null)
                yield break;

            foreach (var hostObject in item.HostObjects)
            {
                if (hostObject is MenuItemHandle itemHandle)
                    yield return itemHandle;
            }
        }

        public static MainMenuHandle? MainMenuHandleFromItem(MenuItem? item)
        {
            if (item is null)
                return null;
            var menuBar = item.MenuBar;
            if (menuBar is null)
                return null;
            var window = menuBar.AttachedWindow;
            if (window is null)
                return null;
            if (MenuUtils.Factory is not WxMenuFactory factory)
                return null;
            var mainMenuHandle = factory.GetMainMenu(window);
            return mainMenuHandle;
        }

        public ContextMenuHandle CreateContextMenu(string id, ContextMenu menu)
        {
            ContextMenuHandle result = new(Native.Menu.CreateContextMenu(id));
            menu.AddHostObject(result);
            return result;
        }

        public MainMenuHandle CreateMainMenu(string id, MainMenu menu)
        {
            MainMenuHandle result = new (Native.Menu.CreateMainMenu(id));
            menu.AddHostObject(result);
            return result;
        }

        public MenuItemHandle CreateMenuItem(MenuItemType itemType, string id, MenuItem menuItem)
        {
            MenuItemHandle result = new(Native.Menu.CreateMenuItem(itemType, id));
            menuItem.AddHostObject(result);
            return result;
        }

        public virtual void DestroyContextMenu(ContextMenuHandle menuHandle)
        {
            var ptr = menuHandle.AsPointer;
            if (ptr == IntPtr.Zero)
                return;
            Native.Menu.DestroyContextMenu(ptr);
        }

        public virtual void DestroyMainMenu(MainMenuHandle menuHandle)
        {
            var ptr = menuHandle.AsPointer;
            if (ptr == IntPtr.Zero)
                return;
            Native.Menu.DestroyMainMenu(ptr);
        }

        public virtual void DestroyMenuItem(MenuItemHandle menuHandle)
        {
            var ptr = menuHandle.AsPointer;
            if (ptr == IntPtr.Zero)
                return;
            Native.Menu.DestroyMenuItem(ptr);
        }

        public MenuItemType GetMenuItemType(MenuItemHandle handle)
        {
            var ptr = handle.AsPointer;
            if (ptr == IntPtr.Zero)
                return MenuItemType.Null;
            return Native.Menu.GetMenuItemType(ptr);
        }

        public virtual void MenuAddItem(ContextMenuHandle handle, MenuItemHandle itemHandle)
        {
            var menuPtr = handle.AsPointer;
            var itemPtr = itemHandle.AsPointer;
            if (menuPtr == IntPtr.Zero || itemPtr == IntPtr.Zero)
                return;
            Native.Menu.MenuAddItem(menuPtr, itemPtr);
        }

        public virtual void MenuRemoveItem(ContextMenuHandle handle, string id)
        {
            var menuPtr = handle.AsPointer;
            if (menuPtr == IntPtr.Zero)
                return;
            Native.Menu.MenuRemoveItem(menuPtr, id);
        }

        public virtual void SetMenuItemBitmap(MenuItemHandle handle, ImageSet? value)
        {
            var itemPtr = handle.AsPointer;
            var nativeImage = (UI.Native.ImageSet?)value?.Handler;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemBitmap(itemPtr, nativeImage);
        }

        public virtual void SetMenuItemRole(MenuItemHandle handle, string role)
        {
            var itemPtr = handle.AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemRole(itemPtr, role);
        }

        public virtual void SetMenuItemChecked(MenuItemHandle handle, bool value)
        {
            var itemPtr = handle.AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemChecked(itemPtr, value);
        }

        public override void Show(
            ContextMenu menu,
            AbstractControl control,
            PointD? position = null,
            Action? onClose = null)
        {
            var menuHandle = menu.GetHostObject<ContextMenuHandle>();

            if (menuHandle is not null)
            {
                menu.RemoveHostObject(menuHandle);
                DestroyContextMenu(menuHandle);
            }

            menuHandle = CreateItemsHandle(menu);

            if (menuHandle is not null)
            {
                menu.AddHostObject(menuHandle);
            }
            else
            {
                return;
            }

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

        public virtual void SetMenuItemEnabled(MenuItemHandle handle, bool value)
        {
            var itemPtr = handle.AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemEnabled(itemPtr, value);
        }

        public virtual void SetMenuItemShortcut(
            MenuItemHandle handle,
            Key key,
            ModifierKeys modifierKeys)
        {
            var itemPtr = handle.AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemShortcut(itemPtr, key, modifierKeys);
        }

        public virtual void SetMenuItemSubMenu(
            MenuItemHandle handle,
            ContextMenuHandle subMenuHandle)
        {
            var itemPtr = handle.AsPointer;
            var subMenuPtr = subMenuHandle.AsPointer;
            if (itemPtr == IntPtr.Zero || subMenuPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemSubMenu(itemPtr, subMenuPtr);
        }

        public virtual void SetMenuItemTextAndShortcut(
            MenuItemHandle handle,
            MenuItem item)
        {
            string? rightText = null;

            if (item.ShortcutInfo is not null)
            {
                ShortcutInfo.FormatOptions options = new()
                {
                    ForUser = true,
                };

                rightText = item.ShortcutInfo.ToString(options);

                var key = item.ShortcutInfo.GetFirstPlatformSpecificKey();
                if (key is not null)
                    SetMenuItemShortcut(handle, key.Key, key.Modifiers);
            }

            SetMenuItemText(handle, item.Text, rightText ?? string.Empty);
        }

        public virtual void SetMenuItemText(
            MenuItemHandle handle,
            string value,
            string rightValue)
        {
            var itemPtr = handle.AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemText(itemPtr, value, rightValue);
        }

        public virtual bool MainMenuAppend(
            MainMenuHandle menuHandle,
            ContextMenuHandle submenu, string text)
        {
            var menuHandlePtr = menuHandle.AsPointer;
            var submenuPtr = submenu.AsPointer;
            if (menuHandlePtr == IntPtr.Zero || submenuPtr == IntPtr.Zero)
                return false;
            return Native.Menu.MainMenuAppend(menuHandlePtr, submenuPtr, text);
        }

        public virtual void MainMenuSetEnabled(MainMenuHandle menuHandle, string childId, bool enable)
        {
            var menuHandlePtr = menuHandle.AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return;
            Native.Menu.MainMenuSetEnabled(menuHandlePtr, childId, enable);
        }

        public virtual ContextMenuHandle MainMenuGetSubMenu(
            MainMenuHandle menuHandle,
            string childId)
        {
            var menuHandlePtr = menuHandle.AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return new ContextMenuHandle(IntPtr.Zero);
            var submenuPtr = Native.Menu.MainMenuGetSubMenu(menuHandlePtr, childId);
            return new ContextMenuHandle(submenuPtr);
        }

        public virtual ContextMenuHandle MainMenuRemove(
            MainMenuHandle menuHandle,
            string childId)
        {
            var menuHandlePtr = menuHandle.AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return new ContextMenuHandle(IntPtr.Zero);
            var submenuPtr = Native.Menu.MainMenuRemove(menuHandlePtr, childId);
            return new ContextMenuHandle(submenuPtr);
        }

        public virtual bool MainMenuInsert(
            MainMenuHandle menuHandle,
            string childId,
            ContextMenuHandle menu,
            string title)
        {
            var menuHandlePtr = menuHandle.AsPointer;
            var submenuPtr = menu.AsPointer;
            if (menuHandlePtr == IntPtr.Zero || submenuPtr == IntPtr.Zero)
                return false;
            return Native.Menu.MainMenuInsert(menuHandlePtr, childId, submenuPtr, title);
        }

        /// <summary>
        /// Creates a context menu handle.
        /// Uses child items for populating the created context menu, if any.
        /// </summary>
        /// <remarks>If the menu contains items, they are added to the created context menu using the
        /// specified or default factory.</remarks>
        /// <returns>A <see cref="ContextMenuHandle"/> representing the created context menu, or
        /// <see langword="null"/> if no factory is available.</returns>
        public virtual ContextMenuHandle? CreateItemsHandle(ContextMenu? menu, bool allowEmpty = false)
        {
            if (menu == null)
                return null;

            if (!menu.HasItems && !allowEmpty)
                return null;

            var result = CreateContextMenu(menu.UniqueId.ToString(), menu);

            if (menu.HasItems)
            {
                foreach (var item in menu.Items)
                {
                    if (!item.Visible)
                        continue;
                    var itemHandle = CreateItemHandle(item);
                    if (itemHandle is not null)
                        MenuAddItem(result, itemHandle);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a handle for the menu item.
        /// </summary>
        /// <remarks>The type of menu item created depends on the state of the object.
        /// Additional properties such as <see cref="MenuItem.Image"/>, <see cref="MenuItem.Enabled"/>,
        /// <see cref="MenuItem.Text"/>, and <see cref="MenuItem.Checked"/> are applied to the created
        /// menu item handle, if applicable.</remarks>
        /// <returns>A <see cref="MenuItemHandle"/> representing the created menu item,
        /// or <see langword="null"/> if the handle could not be created.</returns>
        public virtual MenuItemHandle? CreateItemHandle(MenuItem? item)
        {
            if (item == null)
                return null;

            var itemType = MenuItemType.Standard;
            var isChecked = item.Checked && !item.HasItems;
            var isSeparator = item.IsSeparator;

            if (isSeparator)
                itemType = MenuItemType.Separator;
            else
            if (isChecked)
            {
                itemType = MenuItemType.Check;
            }

            var handle = CreateMenuItem(itemType, item.UniqueId.ToString(), item);
            if (handle is null)
                return null;

            if (item.Image is not null)
                SetMenuItemBitmap(handle, item.Image);

            if (!item.Enabled)
                SetMenuItemEnabled(handle, item.Enabled);

            SetMenuItemTextAndShortcut(handle, item);

            if (isChecked)
                SetMenuItemChecked(handle, isChecked);

            if (item.HasItems && !isSeparator)
            {
                var menuHandle = CreateItemsHandle(item.ItemsMenu);
                if (menuHandle is not null)
                    SetMenuItemSubMenu(handle, menuHandle);
            }

            return handle;
        }

        /// <summary>
        /// Creates a handle for the main menu.
        /// </summary>
        /// <remarks>This method generates a main menu handle based on the current menu configuration.
        /// It iterates through the menu items, appending visible submenus to the main menu handle.
        /// Submenus are created using the provided or default factory.</remarks>
        /// <returns>An instance of <see cref="MainMenuHandle"/> representing the created
        /// main menu handle, or <see langword="null"/> if no factory is available.</returns>
        public virtual MainMenuHandle? CreateMainMenuHandle(MainMenu? menu)
        {
            if (menu == null)
                return null;

            if (!menu.HasItems)
                return null;

            var result = CreateMainMenu(menu.UniqueId.ToString(), menu);

            foreach (var item in menu.Items)
            {
                if (!item.Visible)
                    continue;

                var subMenuHandle = CreateItemsHandle(item.ItemsMenu, allowEmpty: true);
                if (subMenuHandle == null)
                    continue;

                MainMenuAppend(result, subMenuHandle, item.Text);
            }

            return result;
        }

        public MainMenuHandle? GetMainMenu(Window window)
        {
            var handler = window.Handler as WindowHandler;
            if (handler?.NativeControl is not UI.Native.Window nativeWindow)
                return null;
            var menuPtr = Native.Menu.GetMainMenu(nativeWindow);
            if (menuPtr == IntPtr.Zero)
                return null;
            return new MainMenuHandle(menuPtr);
        }

        public override void SetMainMenu(Window window, MainMenu? menu)
        {
            var handler = window.Handler as WindowHandler;
            if (handler?.NativeControl is not UI.Native.Window nativeWindow)
                return;

            if (menu == null)
            {
                Native.Menu.SetMainMenu(nativeWindow, IntPtr.Zero);
                return;
            }

            var menuHandle = CreateMainMenuHandle(menu);

            Native.Menu.SetMainMenu(nativeWindow, menuHandle?.AsPointer ?? IntPtr.Zero);
        }

        public virtual ContextMenuHandle MainMenuReplace(
            MainMenuHandle menuHandle,
            string childId,
            ContextMenuHandle menu,
            string title)
        {
            var menuHandlePtr = menuHandle.AsPointer;
            var submenuPtr = menu.AsPointer;
            if (menuHandlePtr == IntPtr.Zero || submenuPtr == IntPtr.Zero)
                return new ContextMenuHandle(IntPtr.Zero);
            var replacedPtr = Native.Menu.MainMenuReplace(menuHandlePtr, childId, submenuPtr, title);
            return new ContextMenuHandle(replacedPtr);
        }

        public virtual void MainMenuSetText(
            MainMenuHandle menuHandle,
            string childId,
            string label)
        {
            var menuHandlePtr = menuHandle.AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return;
            Native.Menu.MainMenuSetText(menuHandlePtr, childId, label);
        }

        protected virtual void OnNativeMenuDestroying()
        {
            var menu = Menu.MenuFromStringId(Native.Menu.EventMenuItemId);

            if (menu is null)
                return;

            var index = menu.FindHostObjectIndex(MyPredicate);

            if (index >= 0)
            {
                menu.HostObjects?.RemoveAt(index);
            }

            bool MyPredicate(object obj)
            {
                return obj is ContextMenuHandle || obj is MenuItemHandle || obj is MainMenuHandle;
            }
        }

        protected virtual void OnNativeMenuClosed()
        {
            RaiseMenuClosed(new StringEventArgs(Native.Menu.EventMenuItemId));
        }

        protected virtual void OnNativeMenuOpened()
        {
            RaiseMenuOpened(new StringEventArgs(Native.Menu.EventMenuItemId));
        }

        protected virtual void OnNativeMenuHighlight()
        {
            RaiseMenuHighlight(new StringEventArgs(Native.Menu.EventMenuItemId));
        }

        protected virtual void OnNativeMenuClick()
        {
            var menu = Menu.MenuFromStringId(Native.Menu.EventMenuItemId);

            if (menu is MenuItem menuItem)
            {
                menuItem.Checked = Native.Menu.EventMenuItemChecked;
                menuItem.RaiseClick();
                return;
            }
        }

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
            /// Initializes a new instance of the <see cref="MainMenuHandle"/> class
            /// with the specified handle.
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
            /// Initializes a new instance of the <see cref="MenuItemHandle"/> class
            /// with the specified handle.
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
            /// Initializes a new instance of the <see cref="ContextMenuHandle"/> class
            /// with the specified handle.
            /// </summary>
            /// <param name="handle">The native handle object to wrap.</param>
            public ContextMenuHandle(object handle)
                : base(handle)
            {
            }
        }
    }
}