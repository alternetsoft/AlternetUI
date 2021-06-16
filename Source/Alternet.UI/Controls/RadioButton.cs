using System;

namespace Alternet.UI
{
    public class RadioButton : ButtonBase
    {
        private bool isChecked;

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
    }
}