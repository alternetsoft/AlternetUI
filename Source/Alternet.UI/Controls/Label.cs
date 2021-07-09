using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a text label control.
    /// </summary>
    /// <remarks>
    /// <see cref="Label" /> controls are typically used to provide descriptive text for a control.
    /// For example, you can use a <see cref="Label" /> to add descriptive text for a <see cref="TextBox"/> control to inform the
    /// user about the type of data expected in the control. <see cref="Label" /> controls can also be used
    /// to add descriptive text to a <see cref="Window"/> to provide the user with helpful information.
    /// For example, you can add a <see cref="Label" /> to the top of a <see cref="Window"/> that provides instructions
    /// to the user on how to input data in the controls on the form. <see cref="Label" /> controls can be
    /// also used to display run time information on the status of an application. For example,
    /// you can add a <see cref="Label" /> control to a form to display the status of each file as a list of files is processed.
    /// </remarks>
    public class Label : Control
    {
        private string? text;

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> property changes.
        /// </summary>
        public event EventHandler? TextChanged;

        /// <summary>
        /// Gets or sets the text displayed on this label.
        /// </summary>
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
                RaiseTextChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="Text"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnTextChanged(EventArgs e)
        {
        }

        private void RaiseTextChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
            TextChanged?.Invoke(this, e);
        }
    }
}