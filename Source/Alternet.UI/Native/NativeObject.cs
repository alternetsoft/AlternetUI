using System;

namespace Alternet.UI.Native
{
    internal class NativeObject : IDisposable
    {
        ~NativeObject() => Dispose(disposing: false);

        public bool IsDisposed { get; private set; }

        public IntPtr NativePointer { get; protected set; }

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