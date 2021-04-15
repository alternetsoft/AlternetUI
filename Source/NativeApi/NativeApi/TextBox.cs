using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class TextBox : Control
    {
        public event EventHandler? TextChanged { add => throw new Exception(); remove => throw new Exception(); }

        public string Text { get => throw new Exception(); set => throw new Exception(); }
    }
}