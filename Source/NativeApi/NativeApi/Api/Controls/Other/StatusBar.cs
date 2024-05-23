#pragma warning disable
using System;

namespace NativeApi.Api
{
    public class StatusBar : Control
    {
        public IntPtr RealHandle { get; }

        public bool SizingGripVisible { get; set; }

        public event EventHandler? ControlRecreated;
    }
}