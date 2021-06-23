using System;

namespace Alternet.UI
{
    internal class NativeNumericUpDownHandler : NativeControlHandler<NumericUpDown, Native.NumericUpDown>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.NumericUpDown();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Minimum = (int)Control.Minimum; // todo: support non-int values.
            NativeControl.Maximum = (int)Control.Maximum;
            NativeControl.Value = (int)Control.Value;

            Control.MinimumChanged += Control_MinimumChanged;
            Control.MaximumChanged += Control_MaximumChanged;
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

            Control.MinimumChanged -= Control_MinimumChanged;
            Control.MaximumChanged -= Control_MaximumChanged;
            Control.ValueChanged -= Control_ValueChanged;

            NativeControl.ValueChanged -= NativeControl_ValueChanged;
        }

        private void Control_ValueChanged(object? sender, System.EventArgs e)
        {
            NativeControl.Value = (int)Control.Value;
        }

        private void Control_MaximumChanged(object? sender, System.EventArgs e)
        {
            NativeControl.Minimum = (int)Control.Minimum;
        }

        private void Control_MinimumChanged(object? sender, System.EventArgs e)
        {
            NativeControl.Maximum = (int)Control.Maximum;
        }
    }
}