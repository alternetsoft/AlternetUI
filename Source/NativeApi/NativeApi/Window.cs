using System;
using ApiCommon;

namespace NativeApi.Api
{
    public class Window : IDisposable
    {
        public string Title { get => throw new Exception(); set => throw new Exception(); }

        public void Show() => throw new Exception();

        public void AddControl(Control control) => throw new Exception();

        void IDisposable.Dispose() => throw new Exception();
    }
}