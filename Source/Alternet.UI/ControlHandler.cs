using System;

namespace Alternet.UI
{
    public abstract class ControlHandler
    {
        protected ControlHandler(Control control)
        {
            Control = control;
        }

        ~ControlHandler() => Dispose(disposing: false);

        public bool IsDisposed { get; private set; }

        public Control Control { get; }

        public void Dispose()
        {
            Dispose(disposing: true);
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