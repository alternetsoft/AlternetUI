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
            : this(CreateHandle(id))
        {
            menu.AddHostObject(this);
        }

        public static object CreateHandle(string id)
        {
            return NativeStringSpan.InvokeWithResult(id, span => Native.Menu.CreateMainMenu(span));
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

            return NativeStringSpan.InvokeWithResult(text, span =>
            {
                return Native.Menu.MainMenuAppend(menuHandlePtr, submenuPtr, span);
            });
        }

        public virtual void SetEnabled(string childId, bool enable)
        {
            var menuHandlePtr = AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return;
            NativeStringSpan.Invoke(childId, span =>
            {
                Native.Menu.MainMenuSetEnabled(menuHandlePtr, span, enable);
            });
        }

        public virtual WxContextMenu GetSubMenu(string childId)
        {
            var menuHandlePtr = AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return new WxContextMenu(IntPtr.Zero);

            var submenuPtr = NativeStringSpan.InvokeWithResult(childId, span =>
            {
                return Native.Menu.MainMenuGetSubMenu(menuHandlePtr, span);
            });

            return new WxContextMenu(submenuPtr);
        }

        public virtual WxContextMenu Remove(string childId)
        {
            var menuHandlePtr = AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return new WxContextMenu(IntPtr.Zero);
            var submenuPtr = NativeStringSpan.InvokeWithResult(childId, span =>
            {
                return Native.Menu.MainMenuRemove(menuHandlePtr, span);
            });
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
            return NativeStringSpan.InvokeWithResult(childId, title, (childSpan, titleSpan) =>
            {
                return Native.Menu.MainMenuInsert(menuHandlePtr, childSpan, submenuPtr, titleSpan);
            });
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
            var replacedPtr = NativeStringSpan.InvokeWithResult(childId, title, (childSpan, titleSpan) =>
            {
                return Native.Menu.MainMenuReplace(menuHandlePtr, childSpan, submenuPtr, titleSpan);
            });
            return new WxContextMenu(replacedPtr);
        }

        public virtual void SetText(string childId, string label)
        {
            var menuHandlePtr = AsPointer;
            if (menuHandlePtr == IntPtr.Zero)
                return;

            NativeStringSpan.Invoke(childId, label, (childSpan, labelSpan) =>
            {
                Native.Menu.MainMenuSetText(menuHandlePtr, childSpan, labelSpan);
            });
        }
    }
}
