using System;
using System.Runtime.InteropServices;

namespace Alternet.UI.Native
{
    internal abstract class ManagedServerObject : IDisposable
    {
        GCHandle handle;

        protected ManagedServerObject()
        {
            handle = GCHandle.Alloc(this);
        }

        ~ManagedServerObject() => Dispose(disposing: false);

        public bool IsDisposed { get; private set; }

        public IntPtr NativePointer => (IntPtr)handle;

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

                handle.Free();

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