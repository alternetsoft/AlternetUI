using System;
using System.ComponentModel;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class Control : Component
    {
        public Control()
        {
            Handler = CreateHandler();
        }

        public ControlHandler Handler { get; }

        public bool IsDisposed { get; private set; }

        protected abstract ControlHandler CreateHandler();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!IsDisposed)
            {
                IsDisposed = true;
            }
        }

        protected void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }

        public Collection<Control> Controls { get; } = new Collection<Control>();
    }
}