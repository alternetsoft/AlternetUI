using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class Slider
    {
        public void OnPlatformEventValueChanged()
        {
            if (UIControl is not UI.Slider uiControl)
                return;
            uiControl.Value = Value;
        }
    }
}