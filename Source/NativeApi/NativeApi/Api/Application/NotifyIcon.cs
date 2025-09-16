#pragma warning disable
using System;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.3/classwx_task_bar_icon.html
    public class NotifyIcon
    {
        public event EventHandler? LeftMouseButtonUp;
        public event EventHandler? LeftMouseButtonDown;
        public event EventHandler? LeftMouseButtonDoubleClick;

        public event EventHandler? RightMouseButtonUp;
        public event EventHandler? RightMouseButtonDown;
        public event EventHandler? RightMouseButtonDoubleClick;

        public event EventHandler? Click;
        public event EventHandler? Created;

        public string? Text { get; set; }
        public Image? Icon { get; set; }
        public void ShowPopup(IntPtr menuHandle) { }

        public void SetPopupMenu(IntPtr menuHandle) { }

        public bool Visible { get; set; }

        public static bool IsAvailable { get; }
        public bool IsIconInstalled { get; }
        public bool IsOk { get; }
    }
}