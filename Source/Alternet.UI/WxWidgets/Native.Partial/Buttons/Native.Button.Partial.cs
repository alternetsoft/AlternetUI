using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class Button
    {
        public void OnPlatformEventClick()
        {
            (UIControl as UI.Button)?.RaiseClick();
        }
    }
}