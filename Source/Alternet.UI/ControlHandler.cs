using System;
using System.Drawing;

namespace Alternet.UI
{
    public abstract class ControlHandler
    {
        protected ControlHandler(Control control)
        {
            Control = control;

            Control.Controls.ItemInserted += Controls_ItemInserted;
            Control.Controls.ItemRemoved += Controls_ItemRemoved;
        }

        private void Controls_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e) => OnControlInserted(e.Index, e.Item);

        private void Controls_ItemRemoved(object? sender, CollectionChangeEventArgs<Control> e) => OnControlRemoved(e.Index, e.Item);

        protected virtual void OnControlInserted(int index, Control control)
        {
        }

        protected virtual void OnControlRemoved(int index, Control control)
        {
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
    }
}