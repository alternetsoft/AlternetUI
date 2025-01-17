using System;
using Alternet.Drawing;

namespace Alternet.UI
{
#pragma warning disable
    internal class ColorPickerHandler
        : WxControlHandler<ColorPicker, Native.ColorPicker>
#pragma warning enable
    {
        public ColorPickerHandler()
        {
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.ColorPicker();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Value = Control.Value;

            Control.ValueChanged += Control_ValueChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.ValueChanged -= Control_ValueChanged;
        }

        private void Control_ValueChanged(object? sender, System.EventArgs e)
        {
            NativeControl.Value = Control.Value;
        }
    }
}