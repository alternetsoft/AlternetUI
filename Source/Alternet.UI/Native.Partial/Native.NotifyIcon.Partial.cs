using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class NotifyIcon : INotifyIconHandler
    {
        private ContextMenu? contextMenu;

        Action? INotifyIconHandler.LeftMouseButtonUp
        {
            get => LeftMouseButtonUp;
            set => LeftMouseButtonUp = value;
        }

        Action? INotifyIconHandler.Created
        {
            get => Created;
            set => Created = value;
        }

        Action? INotifyIconHandler.LeftMouseButtonDown
        {
            get => LeftMouseButtonDown;
            set => LeftMouseButtonDown = value;
        }

        Action? INotifyIconHandler.LeftMouseButtonDoubleClick
        { 
            get => LeftMouseButtonDoubleClick;
            set => LeftMouseButtonDoubleClick = value;
        }

        Action? INotifyIconHandler.RightMouseButtonUp
        {
            get => RightMouseButtonUp;
            set => RightMouseButtonUp = value;
        }

        Action? INotifyIconHandler.RightMouseButtonDown
        {
            get => RightMouseButtonDown;
            set => RightMouseButtonDown = value;
        }

        Action? INotifyIconHandler.RightMouseButtonDoubleClick
        {
            get => RightMouseButtonDoubleClick;
            set => RightMouseButtonDoubleClick = value;
        }

        Action? INotifyIconHandler.Click
        {
            get => Click;
            set
            {
                Click = value;
            }
        }

        Alternet.Drawing.Image? INotifyIconHandler.Icon
        {
            set
            {
                Icon = (UI.Native.Image?)value?.Handler ?? null;
            }
        }

        public void SetContextMenu(ContextMenu? menu)
        {
            MenuUtils.Required();

            contextMenu = menu;

            if (menu == null)
            {
                SetPopupMenu(IntPtr.Zero);
                return;
            }

            var wxMenu = new WxContextMenu(menu);
            SetPopupMenu(wxMenu.AsPointer);
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }
    }
}