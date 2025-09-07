using System;
using System.ComponentModel;
using Alternet.Drawing;
using ApiCommon;

namespace NativeApi.Api
{
    [ManagedExternName("Alternet.UI.MouseButtonState")]
    [ManagedName("Alternet.UI.MouseButtonState")]
    public enum MouseButtonState
    {
        Released,
        Pressed,
    }
}