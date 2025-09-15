using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class NotifyIcon : INotifyIconHandler
    {
        Action? INotifyIconHandler.LeftMouseButtonUp
        {
            get => LeftMouseButtonUp;
            set => LeftMouseButtonUp = value;
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
            set => Click = value;
        }

        Alternet.Drawing.Image? INotifyIconHandler.Icon
        {
            set
            {
                Icon = (UI.Native.Image?)value?.Handler ?? null;
            }
        }

        public void ShowContextMenu(ContextMenu menu)
        {
            MenuUtils.Required();

            var wxMenu = menu.GetHostObject<WxContextMenu>();
            if (wxMenu == null)
            {
                wxMenu = new WxContextMenu(menu);
                menu.AddHostObject(wxMenu);
            }

            if (wxMenu == null || wxMenu.AsPointer == IntPtr.Zero)
                return;

            this.ShowPopup(wxMenu.AsPointer);
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }
    }
}