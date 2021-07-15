using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class RadioButton : Control
    {
        public string Text { get => throw new Exception(); set => throw new Exception(); }

        public bool IsChecked { get; set; }

        public event EventHandler? CheckedChanged { add => throw new Exception(); remove => throw new Exception(); }
    }
}