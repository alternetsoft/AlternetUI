using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class NumericUpDown : Control
    {
        public event EventHandler? ValueChanged { add => throw new Exception(); remove => throw new Exception(); }

        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Value { get; set; }
    }
}