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

        public bool Handled { get => Result != IntPtr.Zero; set => Result = (value ? new IntPtr(1) : IntPtr.Zero); }

        public IntPtr Result { get; set; }
    }

    internal delegate void NativeEventHandler<T>(object? sender, NativeEventArgs<T> e) where T : class;
}