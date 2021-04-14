using System;
using System.Collections.Generic;

namespace Alternet.UI.Native
{
    internal class NativeObject : IDisposable
    {
        protected NativeObject()
        {
        }

        protected NativeObject(IntPtr nativePointer)
        {
            SetNativePointer(nativePointer);
        }

        private static readonly Dictionary<IntPtr, NativeObject> instancesByNativePointers = new Dictionary<IntPtr, NativeObject>();

        ~NativeObject() => Dispose(disposing: false);

        public bool IsDisposed { get; private set; }

        public IntPtr NativePointer { get; private set; }

        public static T? GetFromNativePointer<T>(IntPtr pointer, Func<IntPtr, T>? fromPointerFactory) where T : NativeObject
        {
            if (pointer == IntPtr.Zero)
                return null;

            if (!instancesByNativePointers.TryGetValue(pointer, out var w))
            {
                if (fromPointerFactory != null)
                    return fromPointerFactory(pointer);
                else
                    throw new InvalidOperationException();
            }

            return (T)w;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected void SetNativePointer(IntPtr value)
        {
            if (value == IntPtr.Zero)
                instancesByNativePointers.Remove(NativePointer);

            NativePointer = value;

            if (value != IntPtr.Zero)
                instancesByNativePointers.Add(NativePointer, this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                }

                IsDisposed = true;
            }
        }

        protected void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }
    }
}