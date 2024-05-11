﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class RichToolTipHandler : DisposableObject, IRichToolTipHandler
    {
        public RichToolTipHandler(string title, string message)
        {
            Handle = Native.WxOtherFactory.CreateRichToolTip(title, message);
        }

        public void SetTipKind(RichToolTipKind tipKind)
        {
            Native.WxOtherFactory.RichToolTipSetTipKind(Handle, (int)tipKind);
        }

        public void SetIcon(MessageBoxIcon icon)
        {
            const int wxICON_WARNING = 0x00000100;
            const int wxICON_ERROR = 0x00000200;
            const int wxICON_INFORMATION = 0x00000800;
            const int wxICON_NONE = 0x00040000;

            int style;

            switch (icon)
            {
                default:
                case MessageBoxIcon.None:
                    style = wxICON_NONE;
                    break;
                case MessageBoxIcon.Information:
                    style = wxICON_INFORMATION;
                    break;
                case MessageBoxIcon.Warning:
                    style = wxICON_WARNING;
                    break;
                case MessageBoxIcon.Error:
                    style = wxICON_ERROR;
                    break;
            }

            Native.WxOtherFactory.RichToolTipSetIcon2(Handle, style);
        }

        public void Show(Control control, RectI? rect = null)
        {
            var wxWidget = WxPlatformControl.WxWidget(control);
            Native.WxOtherFactory.RichToolTipShowFor(Handle, wxWidget, rect ?? RectI.Empty);
        }

        public void SetTitleFont(Font? font)
        {
            Native.WxOtherFactory.RichToolTipSetTitleFont(Handle, (UI.Native.Font?)font?.NativeObject);
        }

        public void SetIcon(ImageSet? bitmap)
        {
            Native.WxOtherFactory.RichToolTipSetIcon(Handle, (UI.Native.ImageSet?)bitmap?.NativeObject);
        }

        public void SetTimeout(uint milliseconds, uint millisecondsShowdelay = 0)
        {
            Native.WxOtherFactory.RichToolTipSetTimeout(Handle, milliseconds, millisecondsShowdelay);
        }

        public void SetForegroundColor(Color color)
        {
            Native.WxOtherFactory.RichToolTipSetFgColor(Handle, color);
        }

        public void SetTitleForegroundColor(Color color)
        {
            Native.WxOtherFactory.RichToolTipSetTitleFgColor(Handle, color);
        }

        public void SetBackgroundColor(Color color, Color endColor)
        {
            Native.WxOtherFactory.RichToolTipSetBkColor(Handle, color, endColor);
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanagedResources()
        {
            Native.WxOtherFactory.DeleteRichToolTip(Handle);
        }
    }
}
