using System;

namespace Alternet.UI
{
    public class TabPage : Control
    {
        private string title = "";

        public string Title
        {
            get
            {
                CheckDisposed();
                return title;
            }

            set
            {
                CheckDisposed();
                if (title == value)
                    return;

                title = value;
                InvokeTitleChanged(EventArgs.Empty);
            }
        }

        public event EventHandler? TitleChanged;

        private void InvokeTitleChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTitleChanged(e);
            TitleChanged?.Invoke(this, e);
        }

        protected virtual void OnTitleChanged(EventArgs e)
        {
        }
    }
}