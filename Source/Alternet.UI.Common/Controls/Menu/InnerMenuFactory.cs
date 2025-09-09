using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides functionality for creating, managing, and interacting with menus,
    /// including context menus, main menus, and menu items.
    /// </summary>
    /// <remarks>The <see cref="InnerMenuFactory"/> class implements the <see cref="IMenuFactory"/> interface,
    /// offering methods to create and destroy menus and menu items,
    /// as well as to configure their properties such as
    /// text, shortcuts, and submenus. It also provides event handling for menu interactions, such as clicks,
    /// highlights, and open/close events. <para> This class is designed to be extensible, allowing derived
    /// classes to override its virtual methods to customize behavior.
    /// It also inherits from <see cref="DisposableObject"/>,
    /// ensuring proper resource management. </para></remarks>
    public class InnerMenuFactory : DisposableObject, IMenuFactory
    {
        /// <inheritdoc/>
        public event EventHandler<StringEventArgs>? MenuClick;

        /// <inheritdoc/>
        public event EventHandler<StringEventArgs>? MenuHighlight;

        /// <inheritdoc/>
        public event EventHandler<StringEventArgs>? MenuOpened;

        /// <inheritdoc/>
        public event EventHandler<StringEventArgs>? MenuClosed;

        /// <inheritdoc/>
        public virtual IMenuFactory.ContextMenuHandle CreateContextMenu(string id, ContextMenu? menu)
        {
            object? menuHandle = menu;
            return new IMenuFactory.ContextMenuHandle(menuHandle ?? IntPtr.Zero);
        }

        /// <inheritdoc/>
        public virtual IMenuFactory.MainMenuHandle CreateMainMenu(string id, MainMenu? menu)
        {
            object? menuHandle = menu;
            return new IMenuFactory.MainMenuHandle(menuHandle ?? IntPtr.Zero);
        }

        /// <inheritdoc/>
        public virtual IMenuFactory.MenuItemHandle CreateMenuItem(
            MenuItemType itemType,
            string id,
            MenuItem? menuItem)
        {
            object? menuItemHandle = menuItem;
            return new IMenuFactory.MenuItemHandle(menuItemHandle ?? IntPtr.Zero);
        }

        /// <inheritdoc/>
        public void DestroyContextMenu(IMenuFactory.ContextMenuHandle menuHandle)
        {
        }

        /// <inheritdoc/>
        public void DestroyMainMenu(IMenuFactory.MainMenuHandle menuHandle)
        {
        }

        /// <inheritdoc/>
        public void DestroyMenuItem(IMenuFactory.MenuItemHandle menuHandle)
        {
        }

        /// <inheritdoc/>
        public string GetId(CustomNativeHandle handle)
        {
            if(handle.Handle is BaseObjectWithId baseObject)
                return baseObject.UniqueId.ToString();
            return string.Empty;
        }

        /// <inheritdoc/>
        public MenuItemType GetMenuItemType(IMenuFactory.MenuItemHandle handle)
        {
            if (handle.Handle is not MenuItem menuItem)
                return MenuItemType.Null;

            if (menuItem.IsSeparator)
                return MenuItemType.Separator;

            if (menuItem.Checked)
                return MenuItemType.Check;

            return MenuItemType.Standard;
        }

        /// <inheritdoc/>
        public virtual void MenuAddItem(
            IMenuFactory.ContextMenuHandle handle,
            IMenuFactory.MenuItemHandle itemHandle)
        {
        }

        /// <inheritdoc/>
        public virtual void MenuRemoveItem(
            IMenuFactory.ContextMenuHandle handle,
            IMenuFactory.MenuItemHandle itemHandle)
        {
        }

        /// <inheritdoc/>
        public virtual void SetMenuItemBitmap(IMenuFactory.MenuItemHandle handle, ImageSet? value)
        {
        }

        /// <inheritdoc/>
        public void SetMenuItemChecked(IMenuFactory.MenuItemHandle handle, bool value)
        {
        }

        /// <inheritdoc/>
        public virtual void SetMenuItemEnabled(IMenuFactory.MenuItemHandle handle, bool value)
        {
        }

        /// <inheritdoc/>
        public virtual void SetMenuItemShortcut(
            IMenuFactory.MenuItemHandle handle,
            Key key,
            ModifierKeys modifierKeys)
        {
        }

        /// <inheritdoc/>
        public virtual void SetMenuItemSubMenu(
            IMenuFactory.MenuItemHandle handle,
            IMenuFactory.ContextMenuHandle subMenuHandle)
        {
        }

        /// <inheritdoc/>
        public virtual void SetMenuItemText(IMenuFactory.MenuItemHandle handle, string value, string rightValue)
        {
        }

        /// <inheritdoc/>
        public virtual void Show(
            IMenuFactory.ContextMenuHandle menuHandle,
            AbstractControl control,
            PointD? position = null,
            Action? onClose = null)
        {
            if (menuHandle.Handle is not ContextMenu menu)
                return;

            var pos = Mouse.CoercePosition(position, control);

            while (!control.IsPlatformControl)
            {
                if (control.Parent == null)
                    return;
                pos += control.Location;
                control = control.Parent;
            }

            menu?.ShowInsideControl(control, menu.RelatedControl, pos, onClose);
        }

        /// <inheritdoc/>
        public virtual bool MainMenuAppend(
            IMenuFactory.MainMenuHandle menuHandle,
            IMenuFactory.ContextMenuHandle menu,
            string text)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual int MainMenuGetCount(IMenuFactory.MainMenuHandle menuHandle)
        {
            if (menuHandle.Handle is MainMenu mainMenu)
                return mainMenu.Items.Count;
            return 0;
        }

        /// <inheritdoc/>
        public virtual void MainMenuSetEnabled(
            IMenuFactory.MainMenuHandle menuHandle,
            int pos,
            bool enable)
        {
        }

        /// <inheritdoc/>
        public virtual IMenuFactory.ContextMenuHandle MainMenuGetSubMenu(
            IMenuFactory.MainMenuHandle menuHandle,
            int menuIndex)
        {
            if (menuHandle.Handle is not MainMenu mainMenu)
                return new(IntPtr.Zero);
            var item = mainMenu.Items[menuIndex];
            IMenuFactory.ContextMenuHandle result = new(item.ItemsMenu);
            return result;
        }

        /// <inheritdoc/>
        public virtual IMenuFactory.ContextMenuHandle MainMenuRemove(
            IMenuFactory.MainMenuHandle menuHandle,
            int pos)
        {
            return new(IntPtr.Zero);
        }

        /// <inheritdoc/>
        public virtual bool MainMenuInsert(
            IMenuFactory.MainMenuHandle menuHandle,
            int pos,
            IMenuFactory.ContextMenuHandle menu,
            string title)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual IMenuFactory.ContextMenuHandle MainMenuReplace(
            IMenuFactory.MainMenuHandle menuHandle,
            int pos,
            IMenuFactory.ContextMenuHandle menu,
            string title)
        {
            return new(IntPtr.Zero);
        }

        /// <inheritdoc/>
        public virtual void MainMenuSetText(IMenuFactory.MainMenuHandle menuHandle, int pos, string label)
        {
            if (menuHandle.Handle is not MainMenu mainMenu)
                return;
            mainMenu.Items[pos].Text = label;
        }

        /// <summary>
        /// Raises the <see cref="MenuClick"/> event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuClick(StringEventArgs args)
        {
            MenuClick?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="MenuHighlight"/> event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuHighlight(StringEventArgs args)
        {
            MenuHighlight?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="MenuOpened"/> event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuOpened(StringEventArgs args)
        {
            MenuOpened?.Invoke(this, args);
        }

        /// <summary>
        /// Raises the <see cref="MenuClosed"/> event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void RaiseMenuClosed(StringEventArgs args)
        {
            MenuClosed?.Invoke(this, args);
        }
    }
}
