#pragma warning disable
using System;
using Alternet.Drawing;
using ApiCommon;

namespace NativeApi.Api
{
    public class Window : Control
    {
        public static IntPtr CreateEx(int kind) => throw new Exception();

        public static void SetDefaultBounds(RectD bounds) { }

        public static void SetParkingWindowFont(Font? font) { }

        [NativeEvent(cancellable: true)]
        public event EventHandler? Closing;

        public event EventHandler StateChanged;

        public void SetMinSize(SizeD size) { }

        public void SetMaxSize(SizeD size) { }

        public string Title { get; set; }
        public bool ShowInTaskbar { get; set; }
        public bool MinimizeEnabled { get; set; }
        public bool MaximizeEnabled { get; set; }
        public bool CloseEnabled { get; set; }
        public bool AlwaysOnTop { get; set; }
        public bool IsToolWindow { get; set; }
        public bool Resizable { get; set; }
        public bool HasBorder { get; set; }
        public bool HasTitleBar { get; set; }
        public bool HasSystemMenu { get; set; }
        public bool IsPopupWindow { get; set; }
        public void Close() { }

        public void Activate() { }
        public static Window ActiveWindow { get; }
        public WindowState State { get; set; }
        public IconSet? Icon { get; set; }
        public IntPtr WxStatusBar { get; set; }
    }
}