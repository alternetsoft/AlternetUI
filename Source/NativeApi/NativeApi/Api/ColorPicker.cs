using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class ColorPicker : Control
    {
        public event EventHandler? ValueChanged { add => throw new Exception(); remove => throw new Exception(); }

        public Color Value { get; set; }
    }
}