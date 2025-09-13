using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a handle to a main menu.
    /// </summary>
    internal class WxMainMenu : CustomNativeHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WxMainMenu"/> class.
        /// </summary>
        public WxMainMenu()
        {
        }

        public WxMainMenu(string id, MainMenu menu)
            : this(Native.Menu.CreateMainMenu(id))
        {
            menu.AddHostObject(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WxMainMenu"/> class
        /// with the specified handle.
        /// </summary>
        /// <param name="handle">The native handle object to wrap.</param>
        public WxMainMenu(object handle)
            : base(handle)
        {
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

        public virtual bool Append(WxContextMenu submenu, string text)
        {
            var menuHandlePtr = AsPointer;
            var submenuPtr = submenu.AsPointer;
            if (menuHandlePtr == IntPtr.Zero || submenuPtr == IntPtr.Zero)
                return false;
            return Native.Menu.MainMenuAppend(menuHandlePtr, submenuPtr, text);
        }

        public virtual void SetEnabled(string childId, bool enable)
        {
            var menuHandlePtr = AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return;
            Native.Menu.MainMenuSetEnabled(menuHandlePtr, childId, enable);
        }

        public virtual WxContextMenu GetSubMenu(string childId)
        {
            var menuHandlePtr = AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return new WxContextMenu(IntPtr.Zero);
            var submenuPtr = Native.Menu.MainMenuGetSubMenu(menuHandlePtr, childId);
            return new WxContextMenu(submenuPtr);
        }

        public virtual WxContextMenu Remove(string childId)
        {
            var menuHandlePtr = AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return new WxContextMenu(IntPtr.Zero);
            var submenuPtr = Native.Menu.MainMenuRemove(menuHandlePtr, childId);
            return new WxContextMenu(submenuPtr);
        }

        public virtual bool Insert(
            string childId,
            WxContextMenu menu,
            string title)
        {
            var menuHandlePtr = AsPointer;
            var submenuPtr = menu.AsPointer;
            if (menuHandlePtr == IntPtr.Zero || submenuPtr == IntPtr.Zero)
                return false;
            return Native.Menu.MainMenuInsert(menuHandlePtr, childId, submenuPtr, title);
        }

        public virtual void Delete()
        {
            var ptr = AsPointer;
            if (ptr == IntPtr.Zero)
                return;
            Handle = IntPtr.Zero;
            Native.Menu.DestroyMainMenu(ptr);
        }

        public virtual WxContextMenu Replace(
            string childId,
            WxContextMenu menu,
            string title)
        {
            var menuHandlePtr = AsPointer;
            var submenuPtr = menu.AsPointer;
            if (menuHandlePtr == IntPtr.Zero || submenuPtr == IntPtr.Zero)
                return new WxContextMenu(IntPtr.Zero);
            var replacedPtr = Native.Menu.MainMenuReplace(menuHandlePtr, childId, submenuPtr, title);
            return new WxContextMenu(replacedPtr);
        }

        public virtual void SetText(string childId, string label)
        {
            var menuHandlePtr = AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return;
            Native.Menu.MainMenuSetText(menuHandlePtr, childId, label);
        }
    }
}
