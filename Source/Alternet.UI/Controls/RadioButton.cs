using System;

namespace Alternet.UI
{
    /// <summary>
    /// Enables the user to select a single option from a group of choices when paired with other <see cref="RadioButton"/> controls.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When the user selects one radio button (also known as an option button) within a group, the others clear automatically.
    /// All <see cref="RadioButton"/> controls in a given container, such as a <see cref="Window"/>, constitute a group.
    /// To create multiple groups on one window, place each group in its own container, such as a <see cref="GroupBox"/>.
    /// </para>
    /// <para>
    /// <see cref="RadioButton"/> and <see cref="CheckBox"/> controls have a similar function: they offer choices a user can select or clear.
    /// The difference is that multiple <see cref="CheckBox"/> controls can be selected at the same time, but option buttons are mutually exclusive.
    /// </para>
    /// <para>
    /// Use the <see cref="IsChecked"/> property to get or set the state of a <see cref="RadioButton"/>.
    /// </para>
    /// </remarks>
    public class RadioButton : ButtonBase
    {
        private bool isChecked;

        /// <summary>
        /// Occurs when the value of the <see cref="IsChecked"/> property changes.
        /// </summary>
        public event EventHandler? CheckedChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value><c>true</c> if the radio button is checked; otherwise, <c>false</c>.</value>
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