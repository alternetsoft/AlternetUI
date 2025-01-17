using System;
using System.Runtime.InteropServices;

namespace Alternet.UI
{
    internal class NumericUpDownHandler :
        WxControlHandler<NumericUpDown, Native.NumericUpDown>
    {
        public NumericUpDownHandler()
        {
        }

        public override bool HasBorder
        {
            get => NativeControl.HasBorder;
            set => NativeControl.HasBorder = value;
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.NumericUpDown();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (App.IsWindowsOS)
                UserPaint = true;

            if (Control is null)
                return;

            NativeControl.Minimum = (int)Control.Minimum;
            NativeControl.Maximum = (int)Control.Maximum;
            NativeControl.Value = (int)Control.Value;

            Control.MinimumChanged += Control_MinimumChanged;
            Control.MaximumChanged += Control_MaximumChanged;
            Control.ValueChanged += Control_ValueChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (Control is null)
                return;
            Control.MinimumChanged -= Control_MinimumChanged;
            Control.MaximumChanged -= Control_MaximumChanged;
            Control.ValueChanged -= Control_ValueChanged;
        }

        private void Control_ValueChanged(object? sender, System.EventArgs e)
        {
            if(Control is not null)
                NativeControl.Value = (int)Control.Value;
        }

        private void Control_MaximumChanged(object? sender, System.EventArgs e)
        {
            if (Control is not null)
                NativeControl.Maximum = (int)Control.Maximum;
        }

        private void Control_MinimumChanged(object? sender, System.EventArgs e)
        {
            if (Control is not null)
                NativeControl.Minimum = (int)Control.Minimum;
        }
    }
}