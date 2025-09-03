using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class Window
    {
        public override void OnPlatformEventHandleDestroyed()
        {
            base.OnPlatformEventHandleDestroyed();
        }

        public override void OnPlatformEventHandleCreated()
        {
            base.OnPlatformEventHandleCreated();

            if(Handler is WindowHandler windowHandler)
            {
                var statusBar = windowHandler.StatusBar as StatusBar;
                if (statusBar is not null)
                {
                    var handler = statusBar.Handler as StatusBarHandler;
                    handler?.RecreateWidget();
                }
            }
        }

        public void OnPlatformEventClosing(CancelEventArgs e)
        {
            if (e.Cancel)
                return;

            var uiControl = UIControl as UI.Window;

            WindowClosingEventArgs args = new();
            uiControl?.RaiseClosing(args);

            e.Cancel = args.Cancel;

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