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

        private bool editControlOnly = false;

        public bool EditControlOnly
        {
            get
            {
                CheckDisposed();
                return editControlOnly;
            }

            set
            {
                CheckDisposed();
                if (editControlOnly == value)
                    return;

                editControlOnly = value;
                EditControlOnlyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? EditControlOnlyChanged;

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

        protected override ControlHandler CreateHandler()
        {
            if (EditControlOnly)
                return new NativeTextBoxHandler();

            return base.CreateHandler();
        }
    }
}