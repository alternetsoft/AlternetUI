#pragma warning disable
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class NumericUpDown : Control
    {
        public bool HasBorder { get; set; }

        public event EventHandler? ValueChanged;

        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Value { get; set; }
    }
}