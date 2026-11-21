using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class RadioButton
    {
        public void OnPlatformEventCheckedChanged()
        {
            (UIControl as UI.RadioButton)?.RaiseCheckedChanged();
        }
    }
}