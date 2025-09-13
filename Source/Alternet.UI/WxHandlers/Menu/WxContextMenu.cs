using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a handle to a context menu.
    /// </summary>
    internal class WxContextMenu : CustomNativeHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WxContextMenu"/> class.
        /// </summary>
        public WxContextMenu()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WxContextMenu"/> class
        /// with the specified handle.
        /// </summary>
        /// <param name="handle">The native handle object to wrap.</param>
        public WxContextMenu(object handle)
            : base(handle)
        {
        }

        public WxContextMenu(ContextMenu menu)
        {
            Handle = Native.Menu.CreateContextMenu(menu.UniqueId.ToString());

            if (menu.HasItems)
            {
                foreach (var item in menu.Items)
                {
                    if (!item.Visible)
                        continue;
                    var itemHandle = new WxMenuItem(item);
                    if (itemHandle is not null)
                        Add(itemHandle);
                }
            }
        }

        public WxContextMenu(string id, ContextMenu menu)
            : this(Native.Menu.CreateContextMenu(id))
        {
            menu.AddHostObject(this);
        }

        public virtual void Insert(string posId, WxMenuItem itemHandle)
        {
            var menuPtr = AsPointer;
            var itemPtr = itemHandle.AsPointer;
            if (menuPtr == IntPtr.Zero || itemPtr == IntPtr.Zero)
                return;
            Native.Menu.MenuInsertItem(menuPtr, posId, itemPtr);
        }

        public virtual void Add(WxMenuItem itemHandle)
        {
            var menuPtr = AsPointer;
            var itemPtr = itemHandle.AsPointer;
            if (menuPtr == IntPtr.Zero || itemPtr == IntPtr.Zero)
                return;
            Native.Menu.MenuAddItem(menuPtr, itemPtr);
        }

        public virtual void Remove(string id)
        {
            var menuPtr = AsPointer;
            if (menuPtr == IntPtr.Zero)
                return;
            Native.Menu.MenuRemoveItem(menuPtr, id);
        }

        public virtual void Delete()
        {
            var ptr = AsPointer;
            if (ptr == IntPtr.Zero)
                return;
            Native.Menu.DestroyContextMenu(ptr);
            Handle = IntPtr.Zero;
        }
    }
}
