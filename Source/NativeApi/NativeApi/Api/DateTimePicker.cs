using System;
using Alternet.UI;

namespace NativeApi.Api
{
    public class DateTimePicker : Control
    {
        public event EventHandler? ValueChanged { add => throw new Exception(); remove => throw new Exception(); }
        
        public System.DateTime Value { get; set; }
    }
}