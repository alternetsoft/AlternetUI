using System;
using System.ComponentModel;
using ApiCommon;

namespace NativeApi.Api
{
    [ManagedExternName("Alternet.UI.WindowState")]
    [ManagedName("Alternet.UI.WindowState")]
    public enum WindowState
    {
        Normal,
        Minimized,
        Maximized
    }
}