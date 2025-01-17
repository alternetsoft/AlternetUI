using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class MenuItem
    {
        public void OnPlatformEventClick()
        {
            (UIControl as UI.MenuItem)?.RaiseClick();
        }

        public void OnPlatformEventHighlight()
        {
            (UIControl as UI.MenuItem)?.RaiseHighlighted();
        }

        public void OnPlatformEventOpened()
        {
            (UIControl as UI.MenuItem)?.RaiseOpened();
        }

        public void OnPlatformEventClosed()
        {
            (UIControl as UI.MenuItem)?.RaiseClosed();
        }
    }
}