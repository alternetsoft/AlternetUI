using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class Window
    {
        public void OnPlatformEventClosing(CancelEventArgs e)
        {
            if (e.Cancel)
                return;

            var uiControl = UIControl as UI.Window;

            var canClose = uiControl?.CanClose(true) ?? false;

            e.Cancel = e.Cancel || !canClose;

            if (e.Cancel)
                return;

            uiControl?.RaiseClosed();
        }

        public void OnPlatformEventStateChanged()
        {
            var uiControl = UIControl as UI.Window;
            uiControl?.RaiseStateChanged();
        }
    }
}