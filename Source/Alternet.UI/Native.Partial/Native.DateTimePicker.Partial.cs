using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class DateTimePicker
    {
        public void OnPlatformEventValueChanged()
        {
#pragma warning disable
            if (UIControl is not UI.DateTimePicker uiControl)
                return;
#pragma warning restore
            uiControl.Value = Value;
        }
    }
}