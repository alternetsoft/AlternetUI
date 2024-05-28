using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ColorPickerHandler
        : WxControlHandler<ColorPicker, Native.ColorPicker>, IColorPickerHandler
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

            NativeControl.ValueChanged = NativeControl_ValueChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.ValueChanged -= Control_ValueChanged;

            NativeControl.ValueChanged = null;
        }

        private void NativeControl_ValueChanged()
        {
            Control.Value = NativeControl.Value;
        }

        private void Control_ValueChanged(object? sender, System.EventArgs e)
        {
            NativeControl.Value = Control.Value;
        }
    }
}