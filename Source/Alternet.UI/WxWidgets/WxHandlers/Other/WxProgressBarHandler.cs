using System;

namespace Alternet.UI
{
    internal class WxProgressBarHandler : WxControlHandler<ProgressBar>
    {
        public WxProgressBarHandler()
        {
        }

        public new Native.ProgressBar NativeControl => (Native.ProgressBar)base.NativeControl!;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.ProgressBar();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (Control is null)
                return;

            NativeControl.Minimum = Control.Minimum;
            NativeControl.Maximum = Control.Maximum;
            NativeControl.Value = Control.Value;
            NativeControl.Orientation = Control.Orientation;
            NativeControl.IsIndeterminate = Control.IsIndeterminate;

            Control.MinimumChanged += Control_MinimumChanged;
            Control.MaximumChanged += Control_MaximumChanged;
            Control.ValueChanged += Control_ValueChanged;
            Control.OrientationChanged += Control_OrientationChanged;
            Control.IsIndeterminateChanged += Control_IsIndeterminateChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (Control is null)
                return;

            Control.MinimumChanged -= Control_MinimumChanged;
            Control.MaximumChanged -= Control_MaximumChanged;
            Control.ValueChanged -= Control_ValueChanged;
            Control.OrientationChanged -= Control_OrientationChanged;
            Control.IsIndeterminateChanged -= Control_IsIndeterminateChanged;
        }

        private void Control_IsIndeterminateChanged(object? sender, EventArgs e)
        {
            if(Control is not null)
                NativeControl.IsIndeterminate = Control.IsIndeterminate;
        }

        private void Control_OrientationChanged(object? sender, EventArgs e)
        {
            if (Control is not null)
                NativeControl.Orientation = Control.Orientation;
        }

        private void Control_ValueChanged(object? sender, System.EventArgs e)
        {
            if (Control is not null)
                NativeControl.Value = Control.Value;
        }

        private void Control_MaximumChanged(object? sender, System.EventArgs e)
        {
            if (Control is not null)
                NativeControl.Maximum = Control.Maximum;
        }

        private void Control_MinimumChanged(object? sender, System.EventArgs e)
        {
            if (Control is not null)
                NativeControl.Minimum = Control.Minimum;
        }
    }
}