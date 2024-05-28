#pragma warning disable
using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class ColorPicker : Control
    {
        public event EventHandler? ValueChanged;

        public Color Value { get; set; }
    }
}