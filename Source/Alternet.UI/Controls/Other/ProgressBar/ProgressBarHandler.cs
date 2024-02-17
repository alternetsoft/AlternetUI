namespace Alternet.UI
{
    internal class ProgressBarHandler : ControlHandler<ProgressBar>
    {
        public new Native.ProgressBar NativeControl => (Native.ProgressBar)base.NativeControl!;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.ProgressBar();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Minimum = Control.Minimum;
            NativeControl.Maximum = Control.Maximum;
            NativeControl.Value = Control.Value;
            NativeControl.Orientation = (Native.ProgressBarOrientation)Control.Orientation;
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

            Control.MinimumChanged -= Control_MinimumChanged;
            Control.MaximumChanged -= Control_MaximumChanged;
            Control.ValueChanged -= Control_ValueChanged;
            Control.OrientationChanged -= Control_OrientationChanged;
            Control.IsIndeterminateChanged -= Control_IsIndeterminateChanged;
        }

        private void Control_IsIndeterminateChanged(object? sender, EventArgs e)
        {
            NativeControl.IsIndeterminate = Control.IsIndeterminate;
        }

        private void Control_OrientationChanged(object? sender, EventArgs e)
        {
            NativeControl.Orientation = (Native.ProgressBarOrientation)Control.Orientation;
        }

        private void Control_ValueChanged(object? sender, System.EventArgs e)
        {
            NativeControl.Value = Control.Value;
        }

        private void Control_MaximumChanged(object? sender, System.EventArgs e)
        {
            NativeControl.Maximum = Control.Maximum;
        }

        private void Control_MinimumChanged(object? sender, System.EventArgs e)
        {
            NativeControl.Minimum = Control.Minimum;
        }
    }
}