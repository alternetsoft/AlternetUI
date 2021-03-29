using System;

namespace Alternet.UI
{
    public class Button : Control
    {
        private volatile bool isDisposed;

        private Native.Button nativeButton;

        public Button()
        {
            nativeButton = new Native.Button();
            nativeButton.Click += (o, e) => OnClick(e);
        }

        public event EventHandler? Click;

        public bool IsDisposed { get => isDisposed; private set => isDisposed = value; }

        public string Text
        {
            get
            {
                CheckDisposed();
                return nativeButton.Text;
            }

            set
            {
                CheckDisposed();
                nativeButton.Text = value;
            }
        }

        internal override Native.Control NativeControl => nativeButton;

        protected virtual void OnClick(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            Click?.Invoke(this, e);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!IsDisposed)
            {
                nativeButton.Dispose();
                nativeButton = null!;

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