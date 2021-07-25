using System;

namespace ApiCommon
{
    public class NativeEventArgs<T> : EventArgs where T : NativeEventData
    {
        public NativeEventArgs(T data)
        {
            Data = data;
        }

        public T Data { get; }
    }

    public delegate void NativeEventHandler<T>(object? sender, NativeEventArgs<T> e) where T : NativeEventData;
}