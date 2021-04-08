using System;
using System.Drawing;

namespace Alternet.UI
{
    public abstract class ControlHandler
    {
        private int layoutSuspendCount;

        protected ControlHandler(Control control)
        {
            Control = control;

            Control.Controls.ItemInserted += Controls_ItemInserted;
            Control.Controls.ItemRemoved += Controls_ItemRemoved;
        }

        ~ControlHandler() => Dispose(disposing: false);

        public bool IsDisposed { get; private set; }

        public Control Control { get; }

        public abstract RectangleF Bounds { get; set; }

        private bool IsLayoutSuspended => layoutSuspendCount != 0;

        public void Dispose()
        {
            Dispose(disposing: true);
        }

        public abstract void Update();

        public virtual void OnPaint(DrawingContext drawingContext)
        {
        }

        public virtual void OnLayout()
        {
        }

        public virtual SizeF GetPreferredSize(SizeF availableSize)
        {
            return new SizeF();
        }

        public void SuspendLayout()
        {
            layoutSuspendCount++;
        }

        public void ResumeLayout(bool performLayout = true)
        {
            layoutSuspendCount--;
            if (layoutSuspendCount < 0)
                throw new InvalidOperationException();

            if (!IsLayoutSuspended)
            {
                if (performLayout)
                    PerformLayout();
            }
        }

        bool inLayout;

        public void PerformLayout()
        {
            if (IsLayoutSuspended)
                return;

            if (inLayout)
                return;
            
            inLayout = true;
            try
            {
                var parent = Control.Parent;
                if (parent != null)
                    parent.PerformLayout();

                OnLayout();

            }
            finally
            {
                inLayout = false;
            }
        }

        protected virtual void OnControlInserted(int index, Control control)
        {
        }

        protected virtual void OnControlRemoved(int index, Control control)
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    Control.Controls.ItemInserted -= Controls_ItemInserted;
                    Control.Controls.ItemRemoved -= Controls_ItemRemoved;
                }

                IsDisposed = true;
            }
        }

        protected void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }

        private void Controls_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e)
        {
            OnControlInserted(e.Index, e.Item);
            PerformLayout();
        }

        private void Controls_ItemRemoved(object? sender, CollectionChangeEventArgs<Control> e)
        {
            OnControlRemoved(e.Index, e.Item);
        }
    }
}