using System;
using System.ComponentModel;
using ApiCommon;

namespace NativeApi.Api
{
    public class Application
    {
        public Application() => throw new Exception();

        public void Run(Window window) => throw new Exception();

        public event EventHandler Idle;
    }
}