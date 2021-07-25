using System;

namespace Alternet.UI.Native
{
    internal class NativeEventArgs<T> : EventArgs where T : class
    {
        public NativeEventArgs(T data)
        {
            Data = data;
        }

        public T Data { get; }
    }

    internal delegate void NativeEventHandler<T>(object? sender, NativeEventArgs<T> e) where T : class;
}