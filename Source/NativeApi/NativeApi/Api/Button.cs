using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class Button : Control
    {
        public event EventHandler? Click { add => throw new Exception(); remove => throw new Exception(); }

        public string Text { get => throw new Exception(); set => throw new Exception(); }

        public bool IsDefault { get; set; }

        public bool IsCancel { get; set; }
    }
}