using System;

namespace Alternet.UI
{
    public class CheckBox : Control
    {
        private string? text;
        private bool isChecked;

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

        public event EventHandler? CheckedChanged;

        void InvokeCheckedChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnCheckedChanged(e);
            CheckedChanged?.Invoke(this, e);
        }

        protected virtual void OnCheckedChanged(EventArgs e)
        {
        }

        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (isChecked == value)
                    return;

                isChecked = value;
                InvokeCheckedChanged(EventArgs.Empty);
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