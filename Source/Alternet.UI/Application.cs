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
            current = this;
        }

        public bool IsDisposed { get => isDisposed; private set => isDisposed = value; }

        public VisualTheme VisualTheme { get; set; } = StockVisualThemes.Native;

        static Application? current;

        public static Application Current
        {
            get
            {
                // todo: maybe make it thread static?
                // todo: maybe move this to native?
                return current ?? throw new InvalidOperationException(ErrorMessages.CurrentApplicationIsNotSet);
            }
        }

        public void Run(Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            CheckDisposed();
            nativeApplication.Run(((NativeWindowHandler)window.Handler).NativeControl);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!IsDisposed)
            {
                nativeApplication.Dispose();
                nativeApplication = null!;

                current = null;

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