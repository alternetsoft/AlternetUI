#pragma warning disable
using System;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_task_bar_icon.html
    public class NotifyIcon
    {
        public event EventHandler? Click;
        public event EventHandler? DoubleClick;

        public string? Text { get; set; }
        public Image? Icon { get; set; }
        public void SetMenu(IntPtr menuHandle) { }
        public bool Visible { get; set; }

        public static bool IsAvailable { get; }
        public bool IsIconInstalled { get; }
        public bool IsOk { get; }
    }
}