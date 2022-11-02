using System;

namespace NativeApi.Api
{
    public class NotifyIcon
    {
        public event EventHandler? Click { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? DoubleClick { add => throw new Exception(); remove => throw new Exception(); }

        public string? Text { get; set; }

        public Image? Icon { get; set; }

        public Menu? Menu { get; set; }

        public bool Visible { get; set; }
    }
}