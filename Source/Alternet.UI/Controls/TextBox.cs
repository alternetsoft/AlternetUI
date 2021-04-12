using System;

namespace Alternet.UI
{
    public class TextBox : Control
    {
        private string text = "";

        public string Text
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

        public void InvokeTextChanged(EventArgs e)
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