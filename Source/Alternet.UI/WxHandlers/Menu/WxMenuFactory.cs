using System;
using System.Collections.Generic;
using System.Linq;
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

                if (item.HasMainMenuParent)
                {
                    var menuBarHandles = item.MenuBar?.GetHostObjects<WxMainMenu>() ?? [];
                    foreach (var menuBarHandle in menuBarHandles)
                    {
                        menuBarHandle.SetEnabled(
                            item.ItemsMenu.UniqueId.ToString(),
                            item.Enabled);
                    }
                }

                var itemHandles = GetItemHandles(item).ToArray();

                foreach (var itemHandle in itemHandles)
                    itemHandle.Enabled = item.Enabled;
            };

            StaticMenuEvents.ItemTextChanged += (s, e) =>
            {
                if (s is not MenuItem item)
                    return;

                if (item.HasMainMenuParent)
                {
                    var menuBarHandles = item.MenuBar?.GetHostObjects<WxMainMenu>() ?? [];
                    foreach (var menuBarHandle in menuBarHandles)
                    {
                        menuBarHandle.SetText(
                            item.ItemsMenu.UniqueId.ToString(),
                            item.Text);
                    }
                }

                var itemHandles = GetItemHandles(item).ToArray();

                foreach (var itemHandle in itemHandles)
                {
                    var isSeparator = itemHandle.ItemType == MenuItemType.Separator;
                    if (isSeparator == item.IsSeparator)
                    {
                        itemHandle.SetTextAndShortcut(item);
                    }
                    else
                    {
                        RecreateItem(item);
                    }
                }
            };

            StaticMenuEvents.ItemShortcutChanged += (s, e) =>
            {
                if (s is not MenuItem item)
                    return;
                var itemHandles = GetItemHandles(item);
                foreach (var itemHandle in itemHandles)
                    itemHandle.SetTextAndShortcut(item);
            };

            StaticMenuEvents.ItemCheckedChanged += (s, e) =>
            {
                if (s is not MenuItem item)
                    return;
                var itemHandles = GetItemHandles(item);
                var recreateItem = false;

                foreach (var itemHandle in itemHandles)
                {
                    var isCheckable = itemHandle.ItemType == MenuItemType.Check;

                    if (!isCheckable)
                    {
                        recreateItem = true;
                        break;
                    }

                    itemHandle.Checked = item.Checked && !item.HasItems;
                }

                if(recreateItem)
                    RecreateItem(item);
            };

            StaticMenuEvents.ItemRoleChanged += (s, e) =>
            {
                if (s is not MenuItem item)
                    return;
                var itemHandles = GetItemHandles(item);
                foreach (var itemHandle in itemHandles)
                    itemHandle.Role = item.Role?.Name ?? string.Empty;
            };

            StaticMenuEvents.ItemImageChanged += (s, e) =>
            {
                if (s is not MenuItem item)
                    return;
                var itemHandles = GetItemHandles(item);
                foreach (var itemHandle in itemHandles)
                    itemHandle.Bitmap = item.Image;
            };

            StaticMenuEvents.ItemVisibleChanged += (s, e) =>
            {
                if (s is not MenuItem item || item.LogicalParent is not Menu menu)
                    return;

                if (item.Visible)
                {
                    InsertItem(item);
                }
                else
                {
                    RemoveItem(menu, item);
                }
            };

            StaticMenuEvents.MainMenuItemRemoved += (s, e) =>
            {
                if (s is not Menu parent)
                    return;
                if (e.Item is not MenuItem item)
                    return;
                RemoveItemFromMainMenu(parent, item);
            };

            StaticMenuEvents.ContextMenuItemRemoved += (s, e) =>
            {
                if (s is not Menu parent)
                    return;
                if (e.Item is not MenuItem item)
                    return;
                RemoveItemFromContextMenu(parent, item);
            };

            StaticMenuEvents.ItemInserted += (s, e) =>
            {
                if (e.Item is not MenuItem item)
                    return;
                if(item.LogicalParent != s)
                    return;
                InsertItem(item);
            };

            StaticMenuEvents.ItemChanged += (s, e) =>
            {
            };

            void InsertItem(MenuItem item)
            {
                if (item.HasMainMenuParent)
                {
                    InsertToMainMenu();
                }
                else
                {
                    InsertToContextMenu();
                }

                void InsertToMainMenu()
                {
                    var menuBarHandles = item.MenuBar?.GetHostObjects<WxMainMenu>() ?? [];

                    if (item.IsLastVisibleInParent)
                    {
                        foreach (var menuBarHandle in menuBarHandles)
                        {
                            CurrentFactory?.AppendItemToMainMenu(menuBarHandle, item);
                        }
                    }
                    else
                    {
                        var nextSiblingId = item.NextVisibleSibling?.ItemsMenu.UniqueId.ToString();
                        if(nextSiblingId is null)
                            return;

                        foreach (var menuBarHandle in menuBarHandles)
                        {
                            var subMenuHandle = new WxContextMenu(item.ItemsMenu);
                            if (subMenuHandle is null)
                                continue;
                            menuBarHandle.Insert(nextSiblingId, subMenuHandle, item.Text);
                        }
                    }
                }

                void InsertToContextMenu()
                {
                    if (item.LogicalParent is not MenuItem parent)
                        return;
                    var contextMenuHandles = parent.ItemsMenu.GetHostObjects<WxContextMenu>();

                    if (item.IsLastInParent)
                    {
                        foreach (var menuHandle in contextMenuHandles)
                        {
                            var itemHandle = new WxMenuItem(item);
                            if (itemHandle is not null)
                                menuHandle.Add(itemHandle);
                        }
                    }
                    else
                    {
                        var nextSiblingId = item.NextVisibleSibling?.UniqueId.ToString();
                        if (nextSiblingId is null)
                            return;
                        foreach (var menuHandle in contextMenuHandles)
                        {
                            var itemHandle = new WxMenuItem(item);
                            if (itemHandle is null)
                                continue;
                            menuHandle.Insert(nextSiblingId, itemHandle);
                        }
                    }
                }
            }

            void RemoveItemFromMainMenu(Menu parent, MenuItem item)
            {
                var menuBarHandles = parent.GetHostObjects<WxMainMenu>() ?? [];
                var id = item.ItemsMenu.UniqueId.ToString();
                foreach (var menuBarHandle in menuBarHandles)
                {
                    menuBarHandle.Remove(id);
                }
            }

            void RemoveItemFromContextMenu(Menu parent, MenuItem item)
            {
                ContextMenu? parentMenu;

                if (parent is MenuItem parentItem)
                {
                    parentMenu = parentItem.ItemsMenu;
                }
                else
                {
                    parentMenu = parent as ContextMenu;
                }

                if (parentMenu is null)
                    return;

                var contextMenuHandles = parentMenu.GetHostObjects<WxContextMenu>();
                var id = item.UniqueId.ToString();

                foreach (var menuHandle in contextMenuHandles)
                {
                    menuHandle.Remove(id);
                }
            }

            void RemoveItem(Menu parent, MenuItem item)
            {
                if (item.HasMainMenuParent)
                {
                    RemoveItemFromMainMenu(parent, item);
                }
                else
                {
                    RemoveItemFromContextMenu(parent, item);
                }
            }

            void RemoveItemSimple(MenuItem item)
            {
                if (item.LogicalParent is not Menu parent)
                    return;
                RemoveItem(parent, item);
            }

            void RecreateItem(MenuItem item)
            {
                RemoveItemSimple(item);
                InsertItem(item);
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

        public static Native.Menu? NativeMenu => CurrentFactory?.nativeMenu;

        public static WxMenuItem? MenuItemHandleFromId(Window window, string id)
        {
            if (window is null)
                return null;
            var handler = window.Handler as WindowHandler;
            if (handler?.NativeControl is not UI.Native.Window nativeWindow)
                return null;
            var itemPtr = Native.Menu.FindMenuItem(nativeWindow, id);
            if (itemPtr == IntPtr.Zero)
                return null;
            return new WxMenuItem(itemPtr);
        }

        public static WxMenuItem? FindMenuItem(Window window, string id)
        {
            var handler = window.Handler as WindowHandler;
            if (handler?.NativeControl is not UI.Native.Window nativeWindow)
                return null;
            var itemPtr = Native.Menu.FindMenuItem(nativeWindow, id);
            if (itemPtr == IntPtr.Zero)
                return null;
            return new WxMenuItem(itemPtr);
        }

        public static IEnumerable<WxMenuItem> GetItemHandles(MenuItem item)
        {
            if (item is null || item.HostObjects is null)
                yield break;

            foreach (var hostObject in item.HostObjects)
            {
                if (hostObject is WxMenuItem itemHandle)
                    yield return itemHandle;
            }
        }

        public static WxMainMenu? MainMenuHandleFromItem(MenuItem? item)
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

        public override void Show(
            ContextMenu menu,
            AbstractControl control,
            PointD? position = null,
            Action? onClose = null)
        {
            var menuHandle = menu.GetHostObject<WxContextMenu>();

            if (menuHandle is not null)
            {
                menu.RemoveHostObject(menuHandle);
                menuHandle.Delete();
            }

            menuHandle = new WxContextMenu(menu);

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

                if (control.Parent == null)
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

        /// <summary>
        /// Creates a handle for the main menu.
        /// </summary>
        /// <remarks>This method generates a main menu handle based on the current menu configuration.
        /// It iterates through the menu items, appending visible submenus to the main menu handle.
        /// Submenus are created using the provided or default factory.</remarks>
        /// <returns>An instance of <see cref="WxMainMenu"/> representing the created
        /// main menu handle, or <see langword="null"/> if no factory is available.</returns>
        public virtual WxMainMenu? CreateMainMenuHandle(MainMenu? menu)
        {
            if (menu == null)
                return null;

            if (!menu.HasItems)
                return null;

            var result = new WxMainMenu(menu.UniqueId.ToString(), menu);

            foreach (var item in menu.Items)
            {
                if (!item.Visible)
                    continue;
                AppendItemToMainMenu(result, item);
            }

            return result;
        }

        public void AppendItemToMainMenu(WxMainMenu menuHandle, MenuItem item)
        {
            var subMenuHandle = new WxContextMenu(item.ItemsMenu);
            menuHandle.Append(subMenuHandle, item.Text);
        }

        public WxMainMenu? GetMainMenu(Window window)
        {
            var handler = window.Handler as WindowHandler;
            if (handler?.NativeControl is not UI.Native.Window nativeWindow)
                return null;
            var menuPtr = Native.Menu.GetMainMenu(nativeWindow);
            if (menuPtr == IntPtr.Zero)
                return null;
            return new WxMainMenu(menuPtr);
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
                return obj is WxContextMenu || obj is WxMenuItem || obj is WxMainMenu;
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
    }
}