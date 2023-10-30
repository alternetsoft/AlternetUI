#pragma warning disable
using ApiCommon;
using System;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_slider.html
    public class Slider : Control
    {
        public event EventHandler? ValueChanged;

        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Value { get; set; }

        public int SmallChange { get; set; }
        public int LargeChange { get; set; }
        public int TickFrequency { get; set; }

        public SliderOrientation Orientation { get; set; }
        public SliderTickStyle TickStyle { get; set; }

        public void ClearTicks() { }
    }
}