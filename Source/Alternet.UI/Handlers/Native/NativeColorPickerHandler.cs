using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeColorPickerHandler : NativeControlHandler<ColorPicker, Native.ColorPicker>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.ColorPicker();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Value = (Color)Control.Value;

            Control.ValueChanged += Control_ValueChanged;

            NativeControl.ValueChanged += NativeControl_ValueChanged;
        }

        private void NativeControl_ValueChanged(object? sender, EventArgs e)
        {
            Control.Value = NativeControl.Value;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.ValueChanged -= Control_ValueChanged;

            NativeControl.ValueChanged -= NativeControl_ValueChanged;
        }

        private void Control_ValueChanged(object? sender, System.EventArgs e)
        {
            NativeControl.Value = Control.Value;
        }
    }
}