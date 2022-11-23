using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class ProgressBar : Control
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Value { get; set; }

        public bool IsIndeterminate { get; set; }

        public ProgressBarOrientation Orientation { get; set; }
    }
}