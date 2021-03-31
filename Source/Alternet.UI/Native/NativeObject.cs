using System;
using System.Collections.Generic;

namespace Alternet.UI.Native
{
    internal class NativeObject : IDisposable
    {
        private static readonly Dictionary<IntPtr, NativeObject> instancesByNativePointers = new Dictionary<IntPtr, NativeObject>();

        ~NativeObject() => Dispose(disposing: false);

        public bool IsDisposed { get; private set; }

        public IntPtr NativePointer { get; private set; }

        protected static NativeObject? TryGetFromNativePointer(IntPtr pointer)
        {
            if (!instancesByNativePointers.TryGetValue(pointer, out var w))
                return null;
            
            return w;
        }

        protected void SetNativePointer(IntPtr value)
        {
            if (value == null)
                instancesByNativePointers.Remove(NativePointer);

            NativePointer = value;

            if (value != null)
                instancesByNativePointers.Add(NativePointer, this);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
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