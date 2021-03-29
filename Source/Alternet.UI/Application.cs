using System;
using System.ComponentModel;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class Application : Component
    {
        private volatile bool isDisposed;

        private Native.Application nativeApplication;

        public Application()
        {
            nativeApplication = new Native.Application();
        }

        public bool IsDisposed { get => isDisposed; private set => isDisposed = value; }

        public void Run(Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            CheckDisposed();
            nativeApplication.Run(window.NativeWindow);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!IsDisposed)
            {
                nativeApplication.Dispose();
                nativeApplication = null!;

                IsDisposed = true;
            }
        }

        private void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }
    }
}