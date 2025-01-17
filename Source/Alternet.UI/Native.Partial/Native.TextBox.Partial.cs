using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class TextBox
    {
        public void OnPlatformEventTextEnter()
        {
            (UIControl as UI.TextBox)?.OnEnterPressed();
        }

        public void OnPlatformEventTextUrl()
        {
            var url = ReportedUrl;
            (UIControl as UI.TextBox)?.OnTextUrl(new UrlEventArgs(url));
        }

        public void OnPlatformEventTextMaxLength()
        {
            (UIControl as UI.TextBox)?.OnTextMaxLength();
        }
    }
}