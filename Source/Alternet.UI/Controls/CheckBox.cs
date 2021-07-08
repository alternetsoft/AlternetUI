using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a check box control.
    /// </summary>
    /// <remarks>
    /// Use a <see cref="CheckBox"/> to give the user an option, such as true/false or yes/no.
    /// The <see cref="CheckBox"/> control can display an image or text or both.
    /// <see cref="CheckBox"/> and <see cref="RadioButton"/> controls have a similar function: they allow the user to
    /// choose from a list of options. <see cref="CheckBox"/> controls let the user pick a combination of options.
    /// In contrast, <see cref="RadioButton"/> controls allow a user to choose from mutually exclusive options.
    /// </remarks>
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
        /// <value><c>true</c> if the <see cref="CheckBox"/> is in the checked state; otherwise, <c>false</c>.
        /// The default value is <c>false</c>.</value>
        /// <remarks>When the value is <c>true</c>, the <see cref="CheckBox"/> portion of the control displays a check mark.</remarks>
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (isChecked == value)
                    return;

                isChecked = value;
                RaiseCheckedChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="IsChecked"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="CheckedChanged"/> event and calls <see cref="OnCheckedChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        private void RaiseCheckedChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnCheckedChanged(e);
            CheckedChanged?.Invoke(this, e);
        }
    }
}