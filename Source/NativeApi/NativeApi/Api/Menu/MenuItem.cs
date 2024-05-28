#pragma warning disable
using System;

namespace NativeApi.Api
{
    public class MenuItem : Control
    {
        public string ManagedCommandId { get; set; }

        public string Text { get; set; }

        public string Role { get; set; }

        public bool Checked { get; set; }

        public void SetShortcut(Key key, ModifierKeys modifierKeys) {}

        public event EventHandler? Click;

        public Menu? Submenu { get; set; }

        public ImageSet? NormalImage { get; set; }
        public ImageSet? DisabledImage { get; set; }

    }
}