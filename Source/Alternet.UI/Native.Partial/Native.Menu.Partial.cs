using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class Menu
    {
        internal MenuItemHandler? OwnerHandler;

        public void OnPlatformEventOpened()
        {
            (OwnerHandler?.Control as UI.MenuItem)?.RaiseOpened();
        }

        public void OnPlatformEventClosed()
        {
            (OwnerHandler?.Control as UI.MenuItem)?.RaiseClosed();
        }
    }
}