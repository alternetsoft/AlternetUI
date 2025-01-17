using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class RichTextBox
    {
        public void OnPlatformEventTextEnter()
        {
            (UIControl as UI.RichTextBox)?.OnEnterPressed();
        }
        
        public void OnPlatformEventTextUrl()
        {
            var url = ReportedUrl;
            (UIControl as UI.RichTextBox)?.OnTextUrl(new UrlEventArgs(url));
        }
    }
}