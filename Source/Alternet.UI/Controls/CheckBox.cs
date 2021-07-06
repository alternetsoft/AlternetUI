using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a check box control.
    /// </summary>
    public class CheckBox : ButtonBase
    {
        private bool isChecked;

        /// <summary>
        /// Occurs when the value of the <see cref="IsChecked"/> property changes.
        /// </summary>
        public event EventHandler? CheckedChanged;

        /// <summary>
        /// Gets or set a value indicating whether the <see cref="CheckBox"/> is in the checked state.
        /// </summary>
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

        /// <summary>
        /// Raises the <see cref="CheckedChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
        }

        private void InvokeCheckedChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnCheckedChanged(e);
            CheckedChanged?.Invoke(this, e);
        }
    }
}