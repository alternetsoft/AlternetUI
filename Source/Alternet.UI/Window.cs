using System;
using System.ComponentModel;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Window : Component
    {
        private volatile bool isDisposed;

        internal Native.Window NativeWindow { get; private set; }

        public Window()
        {
            NativeWindow = new Native.Window();
        }

        public bool IsDisposed { get => isDisposed; private set => isDisposed = value; }

        public string? Title
        {
            get
            {
                CheckDisposed();
                return NativeWindow.Title;
            }

            set
            {
                CheckDisposed();
                NativeWindow.Title = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!IsDisposed)
            {
                NativeWindow.Dispose();
                NativeWindow = null!;

                IsDisposed = true;
            }
        }

        private void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }

        public void AddControl(Control control)
        {
            CheckDisposed();
            NativeWindow.AddControl(((NativeControlHandler)control.Handler).NativeControl);
        }
    }
}