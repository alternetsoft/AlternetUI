using System;

namespace Alternet.UI.Native
{
    internal abstract class ManagedServerObject : IDisposable
    {
        ~ManagedServerObject() => Dispose(disposing: false);

        public bool IsDisposed { get; private set; }

        public IntPtr Handle { get; private set; }

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

                if (Handle != IntPtr.Zero)
                {
                    Handle = IntPtr.Zero;
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