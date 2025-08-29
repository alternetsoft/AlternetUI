using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class NotifyIcon : INotifyIconHandler
    {
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

        Alternet.UI.ContextMenu? INotifyIconHandler.Menu
        {
            set
            {
                if (value == null)
                    Menu = null;
                else
                    Menu = value.Handler as Native.Menu;
            }
        }
    }
}