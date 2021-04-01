using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class Button : Control, IDisposable
    {
        public event EventHandler? Click { add => throw new Exception(); remove => throw new Exception(); }

        void IDisposable.Dispose() => throw new Exception();

        public string? Text { get => throw new Exception(); set => throw new Exception(); }
    }
}