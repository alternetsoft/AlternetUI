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
            if(App.IsMacOS)
                ShowViaContextMenu();
            else
                ShowViaNotifyIcon();

            void ShowViaContextMenu()
            {
                menu.ShowAtMouse();
            }

            void ShowViaNotifyIcon()
            {
                MenuUtils.Required();

                var wxMenus = menu.GetHostObjects<WxContextMenu>().ToArray();
                foreach (var m in wxMenus)
                {
                    menu.RemoveHostObject(m);
                    m.Delete();
                }

                var wxMenu = new WxContextMenu(menu);
                menu.AddHostObject(wxMenu);
                this.ShowPopup(wxMenu.AsPointer);
            }
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }
    }
}