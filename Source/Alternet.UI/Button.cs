using System;

namespace Alternet.UI
{
    public class Button : Control
    {
        private string? text;

        public string? Text
        {
            get
            {
                CheckDisposed();
                return text;
            }

            set
            {
                CheckDisposed();
                if (text == value)
                    return;

                text = value;
                InvokeTextChanged(EventArgs.Empty);
            }
        }

        public event EventHandler? Click;

        public void InvokeClick(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnClick(e);
            Click?.Invoke(this, e);
        }

        protected virtual void OnClick(EventArgs e)
        {
        }

        public event EventHandler? TextChanged;

        private void InvokeTextChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
            TextChanged?.Invoke(this, e);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
        }

        protected override ControlHandler CreateHandler()
        {
            return new NativeButtonHandler(this);
        }
    }
}