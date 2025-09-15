using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a handle to a menu item.
    /// </summary>
    internal class WxMenuItem : CustomNativeHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WxMenuItem"/> class.
        /// </summary>
        public WxMenuItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WxMenuItem"/> class
        /// with the specified handle.
        /// </summary>
        /// <param name="handle">The native handle object to wrap.</param>
        public WxMenuItem(object handle)
            : base(handle)
        {
        }

        public WxMenuItem(MenuItem item)
            : this()
        {
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

            Handle = Native.Menu.CreateMenuItem(
                itemType,
                item.UniqueId.ToString(),
                item.Text,
                string.Empty,
                IntPtr.Zero);

            item.AddHostObject(this);

            if (item.Image is not null)
                Bitmap = item.Image;

            if (!item.Enabled)
                Enabled = item.Enabled;

            if(item.ShortcutInfo is not null)
                SetTextAndShortcut(item);

            if (isChecked)
                Checked = isChecked;

            if (item.HasItems && !isSeparator)
            {
                var menuHandle = new WxContextMenu(item.ItemsMenu);
                if (menuHandle is not null)
                    SubMenu = menuHandle;
            }
        }

        public virtual string Id
        {
            get
            {
                var ptr = AsPointer;
                if (ptr == IntPtr.Zero)
                    return string.Empty;
                return Native.Menu.GetMenuId(ptr);
            }
        }

        public virtual ImageSet? Bitmap
        {
            set
            {
                var itemPtr = AsPointer;
                var nativeImage = (UI.Native.ImageSet?)value?.Handler;
                if (itemPtr == IntPtr.Zero)
                    return;
                Native.Menu.SetMenuItemBitmap(itemPtr, nativeImage);
            }
        }

        public virtual string Role
        {
            set
            {
                var itemPtr = AsPointer;
                if (itemPtr == IntPtr.Zero)
                    return;
                Native.Menu.SetMenuItemRole(itemPtr, value);
            }
        }

        public virtual bool Checked
        {
            set
            {
                var itemPtr = AsPointer;
                if (itemPtr == IntPtr.Zero)
                    return;
                Native.Menu.SetMenuItemChecked(itemPtr, value);
            }
        }

        public MenuItemType ItemType
        {
            get
            {
                var ptr = AsPointer;
                if (ptr == IntPtr.Zero)
                    return MenuItemType.Null;
                return Native.Menu.GetMenuItemType(ptr);
            }
        }

        public virtual bool Enabled
        {
            set
            {
                var itemPtr = AsPointer;
                if (itemPtr == IntPtr.Zero)
                    return;
                Native.Menu.SetMenuItemEnabled(itemPtr, value);
            }
        }

        public virtual void SetShortcut(Key key, ModifierKeys modifiers)
        {
            var itemPtr = AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemShortcut(itemPtr, key, modifiers);
        }

        public virtual WxContextMenu SubMenu
        {
            set
            {
                var itemPtr = AsPointer;
                var subMenuPtr = value.AsPointer;
                if (itemPtr == IntPtr.Zero || subMenuPtr == IntPtr.Zero)
                    return;
                Native.Menu.SetMenuItemSubMenu(itemPtr, subMenuPtr);
            }
        }

        public virtual void SetTextAndShortcut(MenuItem item)
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
                    SetShortcut(key.Key, key.Modifiers);
                else
                    SetShortcut(Key.None, ModifierKeys.None);
            }

            SetText(item.Text, rightText ?? string.Empty);
        }

        public virtual void SetText(string value, string rightValue)
        {
            var itemPtr = AsPointer;
            if (itemPtr == IntPtr.Zero)
                return;
            Native.Menu.SetMenuItemText(itemPtr, value, rightValue);
        }

        public virtual void Delete()
        {
            var ptr = AsPointer;
            if (ptr == IntPtr.Zero)
                return;
            Native.Menu.DestroyMenuItem(ptr);
            Handle = IntPtr.Zero;
        }
    }
}
