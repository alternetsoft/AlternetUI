using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class RichToolTipHandler : DisposableObject<IntPtr>, IRichToolTipHandler
    {
        public SizeI SizeInPixels
        {
            get => Native.WxOtherFactory.RichToolTipGetSize(Handle);
        }

        public RichToolTipHandler(string title, string message)
            : base(Native.WxOtherFactory.CreateRichToolTip(title, message), true)
        {
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

        public void Show(AbstractControl control, RectI? rect = null, bool adjustPos = true)
        {
            var wxWidget = WxApplicationHandler.WxWidget(control);
            Native.WxOtherFactory.RichToolTipShowFor(Handle, wxWidget, rect ?? RectI.Empty, adjustPos);
        }

        public void SetTitleFont(Font? font)
        {
            Native.WxOtherFactory.RichToolTipSetTitleFont(Handle, (UI.Native.Font?)font?.Handler);
        }

        public void SetIcon(ImageSet? bitmap)
        {
            Native.WxOtherFactory.RichToolTipSetIcon(Handle, (UI.Native.ImageSet?)bitmap?.Handler);
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
        protected override void DisposeUnmanaged()
        {
            Native.WxOtherFactory.DeleteRichToolTip(Handle);
        }

        public void SetLocationDecrement(bool decrementX, bool decrementY)
        {
            Native.WxOtherFactory.RichToolTipSetLocationDecrement(Handle, decrementX, decrementY);
        }
    }
}
