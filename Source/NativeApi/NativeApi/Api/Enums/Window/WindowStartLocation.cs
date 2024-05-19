using System;
using System.ComponentModel;
using ApiCommon;

namespace NativeApi.Api
{
    [ManagedExternName("Alternet.UI.WindowStartLocation")]
    [ManagedName("Alternet.UI.WindowStartLocation")]
    public enum WindowStartLocation
    {
        Default,
        Manual,
        CenterScreen,
        CenterOwner
    }
}