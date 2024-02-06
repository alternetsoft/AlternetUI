using System;

namespace Alternet.UI
{
    internal class NativeSliderHandler : SliderHandler
    {
        public new Native.Slider NativeControl => (Native.Slider)base.NativeControl!;

        public override SliderOrientation Orientation
        {
            get => (SliderOrientation)NativeControl.Orientation;
            set => NativeControl.Orientation = (Native.SliderOrientation)value;
        }

        public override SliderTickStyle TickStyle
        {
            get => (SliderTickStyle)NativeControl.TickStyle;
            set
            {
                NativeControl.TickStyle = (Native.SliderTickStyle)value;
                if (value == SliderTickStyle.None && Application.IsLinuxOS)
                    NativeControl.ClearTicks();
            }
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Slider();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Minimum = Control.Minimum;
            NativeControl.Maximum = Control.Maximum;
            NativeControl.Value = Control.Value;
            NativeControl.SmallChange = Control.SmallChange;
            NativeControl.LargeChange = Control.LargeChange;
            NativeControl.TickFrequency = Control.TickFrequency;

            Control.MinimumChanged += Control_MinimumChanged;
            Control.MaximumChanged += Control_MaximumChanged;
            Control.ValueChanged += Control_ValueChanged;
            Control.SmallChangeChanged += Control_SmallChangeChanged;
            Control.LargeChangeChanged += Control_LargeChangeChanged;
            Control.TickFrequencyChanged += Control_TickFrequencyChanged;

            NativeControl.ValueChanged = NativeControl_ValueChanged;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.MinimumChanged -= Control_MinimumChanged;
            Control.MaximumChanged -= Control_MaximumChanged;
            Control.ValueChanged -= Control_ValueChanged;
            Control.SmallChangeChanged -= Control_SmallChangeChanged;
            Control.LargeChangeChanged -= Control_LargeChangeChanged;
            Control.TickFrequencyChanged -= Control_TickFrequencyChanged;

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

        private void Control_SmallChangeChanged(object? sender, System.EventArgs e)
        {
            NativeControl.SmallChange = Control.SmallChange;
        }

        private void Control_LargeChangeChanged(object? sender, System.EventArgs e)
        {
            NativeControl.LargeChange = Control.LargeChange;
        }

        private void Control_TickFrequencyChanged(object? sender, System.EventArgs e)
        {
            NativeControl.TickFrequency = Control.TickFrequency;
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