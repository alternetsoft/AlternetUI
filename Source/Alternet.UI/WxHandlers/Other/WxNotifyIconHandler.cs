using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class NotifyIcon : INotifyIconHandler
    {
        private WxMenuFactory.ContextMenuHandle? menuHandle;
        private Alternet.UI.ContextMenu? menu;

        public Alternet.UI.ContextMenu? Menu
        {
            set
            {
                if (menu == value)
                    return;

                if (menu is not null)
                {
                    menu.ItemChanged -= OnMenuItemChanged;
                    menu.Disposed -= OnMenuDisposed;
                }

                menu = value;

                UpdateNativeMenu(value);

                if (value is not null)
                {
                    value.ItemChanged += OnMenuItemChanged;
                    value.Disposed += OnMenuDisposed;
                }
            }
        }

        Action? INotifyIconHandler.Click
        {
            get => Click;
            set => Click = value;
        }

        Action? INotifyIconHandler.DoubleClick
        {
            get => DoubleClick;
            set => DoubleClick = value;
        }

        Alternet.Drawing.Image? INotifyIconHandler.Icon
        {
            set
            {
                Icon = (UI.Native.Image?)value?.Handler ?? null;
            }
        }

        protected void OnMenuDisposed(object sender, EventArgs e)
        {
            Menu = null;
        }

        protected void OnMenuItemChanged(object sender, MenuChangeEventArgs e)
        {
            UpdateNativeMenu(menu);
        }

        protected void UpdateNativeMenu(Alternet.UI.ContextMenu? value)
        {
            if (MenuUtils.Factory is not WxMenuFactory factory)
                return;

            if (menuHandle != null)
            {
                SetMenu(IntPtr.Zero);
                factory.DestroyContextMenu(menuHandle);
                menuHandle = null;
            }

            if (value == null)
            {
                return;
            }

            menuHandle = factory.CreateItemsHandle(value);

            if (menuHandle == null)
                SetMenu(IntPtr.Zero);
            else
                SetMenu(menuHandle.AsPointer);
        }

        protected override void DisposeManaged()
        {
            Menu = null;
            base.DisposeManaged();
        }
    }
}