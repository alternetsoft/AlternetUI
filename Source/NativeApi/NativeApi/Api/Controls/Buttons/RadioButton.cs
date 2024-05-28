#pragma warning disable
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class RadioButton : Control
    {
        public bool IsChecked { get; set; }

        public event EventHandler? CheckedChanged;
    }
}