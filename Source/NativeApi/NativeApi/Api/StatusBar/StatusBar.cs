#pragma warning disable
using System;

namespace NativeApi.Api
{
    public class StatusBar : Control
    {
        public int PanelCount { get; }

        public IntPtr RealHandle { get; }

        public bool SizingGripVisible { get; set; }

        public event EventHandler? ControlRecreated;
    }
}