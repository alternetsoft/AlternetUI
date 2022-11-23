namespace Alternet.UI
{
    internal class NativeProgressBarHandler : ProgressBarHandler
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.ProgressBar();
        }

        public new Native.ProgressBar NativeControl => (Native.ProgressBar)base.NativeControl!;

        public override bool IsIndeterminate
        {
            get => NativeControl.IsIndeterminate;
            set => NativeControl.IsIndeterminate = value;
        }
        
        public override ProgressBarOrientation Orientation
        {
            get => (ProgressBarOrientation)NativeControl.Orientation;
            set => NativeControl.Orientation = (Native.ProgressBarOrientation)value;
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Minimum = Control.Minimum;
            NativeControl.Maximum = Control.Maximum;
            NativeControl.Value = Control.Value;

            Control.MinimumChanged += Control_MinimumChanged;
            Control.MaximumChanged += Control_MaximumChanged;
            Control.ValueChanged += Control_ValueChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.MinimumChanged -= Control_MinimumChanged;
            Control.MaximumChanged -= Control_MaximumChanged;
            Control.ValueChanged -= Control_ValueChanged;
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