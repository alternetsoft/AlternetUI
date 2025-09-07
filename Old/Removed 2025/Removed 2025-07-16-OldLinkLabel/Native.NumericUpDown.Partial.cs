using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class NumericUpDown
    {
        public void OnPlatformEventValueChanged()
        {
            if (UIControl is not UI.NumericUpDown uiControl)
                return;
            uiControl.Value = Value;
        }
    }
}