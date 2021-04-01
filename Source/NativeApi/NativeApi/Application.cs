using System;
using ApiCommon;

namespace NativeApi.Api
{
    public class Application : IDisposable
    {
        public Application() => throw new Exception();

        void IDisposable.Dispose() => throw new Exception();

        public void Run(Window window) => throw new Exception();
    }
}