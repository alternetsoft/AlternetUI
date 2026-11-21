using System;

namespace Alternet.UI
{
    internal class WxSliderHandler : WxControlHandler<Slider>, ISliderHandler
    {
        public WxSliderHandler()
        {
        }

        public new Native.Slider NativeControl => (Native.Slider)base.NativeControl!;

        public virtual void ClearTicks()
        {
            if (Application.IsWindowsOS || Application.IsLinuxOS)
                NativeControl.ClearTicks();
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Slider();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (Control is null)
                return;

            NativeControl.Orientation = Control.Orientation;
            NativeControl.TickStyle = Control.TickStyle;

            Control.MinimumChanged += Control_MinimumChanged;
            Control.MaximumChanged += Control_MaximumChanged;
            Control.ValueChanged += Control_ValueChanged;
            Control.SmallChangeChanged += Control_SmallChangeChanged;
            Control.LargeChangeChanged += Control_LargeChangeChanged;
            Control.TickFrequencyChanged += Control_TickFrequencyChanged;
            Control.OrientationChanged += Control_OrientationChanged;
            Control.TickStyleChanged += Control_TickStyleChanged;
        }

        public override void OnHandleCreated()
        {
            base.OnHandleCreated();

            if (Control is null)
                return;

            NativeControl.Minimum = Control.Minimum;
            NativeControl.Maximum = Control.Maximum;
            NativeControl.Value = Control.Value;
            NativeControl.SmallChange = Control.SmallChange;
            NativeControl.LargeChange = Control.LargeChange;
            NativeControl.TickFrequency = Control.TickFrequency;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (Control is null)
                return;

            Control.MinimumChanged -= Control_MinimumChanged;
            Control.MaximumChanged -= Control_MaximumChanged;
            Control.ValueChanged -= Control_ValueChanged;
            Control.SmallChangeChanged -= Control_SmallChangeChanged;
            Control.LargeChangeChanged -= Control_LargeChangeChanged;
            Control.TickFrequencyChanged -= Control_TickFrequencyChanged;
            Control.OrientationChanged -= Control_OrientationChanged;
            Control.TickStyleChanged -= Control_TickStyleChanged;
        }

        private void Control_TickStyleChanged(object? sender, EventArgs e)
        {
            if (Control is null)
                return;

            var v = Control.TickStyle;
            NativeControl.TickStyle = v;
            if (v == SliderTickStyle.None)
                Control.ClearTicks();
        }

        private void Control_OrientationChanged(object? sender, EventArgs e)
        {
            if (Control is null)
                return;
            NativeControl.Orientation = Control.Orientation;
        }

        private void Control_ValueChanged(object? sender, System.EventArgs e)
        {
            if (Control is null)
                return;
            NativeControl.Value = Control.Value;
        }

        private void Control_SmallChangeChanged(object? sender, System.EventArgs e)
        {
            if (Control is null)
                return;
            NativeControl.SmallChange = Control.SmallChange;
        }

        private void Control_LargeChangeChanged(object? sender, System.EventArgs e)
        {
            if (Control is null)
                return;
            NativeControl.LargeChange = Control.LargeChange;
        }

        private void Control_TickFrequencyChanged(object? sender, System.EventArgs e)
        {
            if (Control is null)
                return;
            NativeControl.TickFrequency = Control.TickFrequency;
        }

        private void Control_MaximumChanged(object? sender, System.EventArgs e)
        {
            if (Control is null)
                return;
            NativeControl.Maximum = Control.Maximum;
        }

        private void Control_MinimumChanged(object? sender, System.EventArgs e)
        {
            if (Control is null)
                return;
            NativeControl.Minimum = Control.Minimum;
        }
    }
}