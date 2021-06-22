using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class Slider : Control
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Value { get; set; }

        public int SmallChange { get; set; }
        public int LargeChange { get; set; }
        public int TickFrequency { get; set; }
    }
}