using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class Window
    {
        public override void OnPlatformEventBeforeHandleDestroyed()
        {
            base.OnPlatformEventBeforeHandleDestroyed();
        }

        public override void OnPlatformEventHandleDestroyed()
        {
            base.OnPlatformEventHandleDestroyed();
        }

        public override void OnPlatformEventHandleCreated()
        {
            base.OnPlatformEventHandleCreated();
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