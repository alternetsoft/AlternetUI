using System;

namespace Alternet.UI
{
    public abstract class ButtonBase : Control
    {
        private string text = "";

        public string Text // todo: maybe rename to Title? like in cocoa.
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
    }
}