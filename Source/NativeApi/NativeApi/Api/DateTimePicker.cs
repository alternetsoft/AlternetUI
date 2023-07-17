using System;
using Alternet.UI;
using ApiCommon;

namespace NativeApi.Api
{
    public class DateTimePicker : Control
    {
        public event EventHandler? ValueChanged { add => throw new Exception(); remove => throw new Exception(); }

        public Alternet.UI.DateTime Value { get; set; }

        public int ValueKind { get; set; }
    }
}