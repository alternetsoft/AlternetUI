using System;
using System.ComponentModel;
using System.Drawing;

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

        public void SuspendLayout()
        {
            Handler.SuspendLayout();
        }

        public void ResumeLayout(bool performLayout = true)
        {
            Handler.ResumeLayout();
            if (performLayout)
                PerformLayout();
        }

        public void PerformLayout()
        {
            Handler.PerformLayout();
        }

        public SizeF GetPreferredSize(SizeF availableSize)
        {
            return Handler.GetPreferredSize(availableSize);
        }
    }
}