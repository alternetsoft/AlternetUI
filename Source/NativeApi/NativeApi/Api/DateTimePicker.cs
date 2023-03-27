using System;
using Alternet.DateTime;

namespace NativeApi.Api
{
    public class DateTimePicker : Control
    {
        public event EventHandler? ValueChanged { add => throw new Exception(); remove => throw new Exception(); }
        public Alternet.DateTime.DateTime Value { get; set; }
    }
}