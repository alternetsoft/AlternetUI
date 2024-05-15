using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class StatusBarHandler : DisposableObject, IStatusBarHandler
    {
        private readonly StatusBar control;
        private bool sizingGripVisible = true;
        private TextEllipsizeType textEllipsize = TextEllipsizeType.End;
        private Window? window;

        public StatusBarHandler(StatusBar control)
        {
            this.control = control;
        }

        public Window? Window
        {
            get
            {
                return window;
            }

            set
            {
                if (window == value)
                    return;
                window = value;
            }
        }

        public TextEllipsizeType TextEllipsize
        {
            get
            {
                return textEllipsize;
            }

            set
            {
                if (textEllipsize == value)
                    return;
                textEllipsize = value;
                RecreateWidget();
            }
        }

        public bool SizingGripVisible
        {
            get => sizingGripVisible;

            set
            {
                if (sizingGripVisible == value)
                    return;
                sizingGripVisible = value;
                RecreateWidget();
            }
        }

        public bool IsOk => StatusBarHandle != IntPtr.Zero && !control.IsDisposed;

        internal IntPtr StatusBarHandle
        {
            get
            {
                var window = control.Window;

                if (window is null || window.IsDisposed)
                {
                    return default;
                }

                return ((WindowHandler)window.Handler).NativeControl.WxStatusBar;
            }

            set
            {
                var window = control.Window;

                if (window is null || window.IsDisposed)
                    return;

                ((WindowHandler)window.Handler).NativeControl.WxStatusBar = value;
            }
        }

        public StatusBarPanelStyle GetStatusStyle(int index)
        {
            return
                (StatusBarPanelStyle)Native.WxStatusBarFactory.GetStatusStyle(StatusBarHandle, index);
        }

        public RectI GetFieldRect(int index)
        {
            var result = Native.WxStatusBarFactory.GetFieldRect(StatusBarHandle, index);
            return result;
        }

        public bool SetMinHeight(int height)
        {
            Native.WxStatusBarFactory.SetMinHeight(StatusBarHandle, height);
            return true;
        }

        public int GetBorderX()
        {
            return Native.WxStatusBarFactory.GetBorderX(StatusBarHandle);
        }

        public int GetBorderY()
        {
            return Native.WxStatusBarFactory.GetBorderY(StatusBarHandle);
        }

        public bool SetStatusStyles(StatusBarPanelStyle[] styles)
        {
            var result = styles.Cast<int>().ToArray();
            Native.WxStatusBarFactory.SetStatusStyles(StatusBarHandle, result);
            return true;
        }

        public bool SetFieldsCount(int count)
        {
            Native.WxStatusBarFactory.SetFieldsCount(StatusBarHandle, count);
            return true;
        }

        public int GetStatusWidth(int index)
        {
            return Native.WxStatusBarFactory.GetStatusWidth(StatusBarHandle, index);
        }

        public bool SetStatusText(string text, int index = 0)
        {
            Native.WxStatusBarFactory.SetStatusText(StatusBarHandle, text, index);
            return true;
        }

        public bool SetStatusWidths(int[] widths)
        {
            Native.WxStatusBarFactory.SetStatusWidths(StatusBarHandle, widths);
            return true;
        }

        public bool PopStatusText(int index = 0)
        {
            Native.WxStatusBarFactory.PopStatusText(StatusBarHandle, index);
            return true;
        }

        public bool PushStatusText(string text, int index = 0)
        {
            Native.WxStatusBarFactory.PushStatusText(StatusBarHandle, text, index);
            return true;
        }

        public string GetStatusText(int index = 0)
        {
            return Native.WxStatusBarFactory.GetStatusText(StatusBarHandle, index);
        }

        public int GetFieldsCount()
        {
            return Native.WxStatusBarFactory.GetFieldsCount(StatusBarHandle);
        }

        public void RecreateWidget()
        {
            const int SIZEGRIP = 0x0010;
            const int SHOWTIPS = 0x0020;
            const int ELLIPSIZESTART = 0x0040;
            const int ELLIPSIZEMIDDLE = 0x0080;
            const int ELLIPSIZEEND = 0x0100;
            const int FULLREPAINTONRESIZE = 0x00010000;

            var handle = StatusBarHandle;
            var window = control.Window;

            if (window != null)
            {
                StatusBarHandle = default;
                if (handle != default)
                    Native.WxStatusBarFactory.DeleteStatusBar(handle);

                StatusBarHandle =
                    Native.WxStatusBarFactory.CreateStatusBar(
                        WxPlatform.WxWidget(window),
                        GetStyle());
                control.ApplyPanels();
            }

            long GetStyle()
            {
                long ellipsizeStyle = 0;
                switch (control.TextEllipsize)
                {
                    case TextEllipsizeType.End:
                        ellipsizeStyle = ELLIPSIZEEND;
                        break;
                    case TextEllipsizeType.Start:
                        ellipsizeStyle = ELLIPSIZESTART;
                        break;
                    case TextEllipsizeType.Middle:
                        ellipsizeStyle = ELLIPSIZEMIDDLE;
                        break;
                    default:
                        break;
                }

                var result = ellipsizeStyle | SHOWTIPS | FULLREPAINTONRESIZE;
                if (control.SizingGripVisible)
                    result |= SIZEGRIP;
                return result;
            }
        }
    }
}
