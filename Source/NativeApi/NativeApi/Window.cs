using System;
using ApiCommon;

namespace NativeApi.Api
{
    public class Window : Control, IDisposable
    {
        public string Title { get => throw new Exception(); set => throw new Exception(); }

        void IDisposable.Dispose() => throw new Exception();
    }
}