using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class DateTimePicker : Control
    {
        public event EventHandler? ValueChanged { add => throw new Exception(); remove => throw new Exception(); }

        public DateTime Value { get; set; }
    }
}