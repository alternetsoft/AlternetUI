using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that can be used to display or edit unformatted text.
    /// </summary>
    /// <remarks>
    /// With the <see cref="TextBox"/> control, the user can enter text in an application.
    /// </remarks>
    public class TextBox : Control
    {
        private string text = "";

        private bool editControlOnly = false;

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> property changes.
        /// </summary>
        public event EventHandler? TextChanged;

        /// <summary>
        /// todo: use BorderStyle for this purpose later.
        /// </summary>
        public event EventHandler? EditControlOnlyChanged;

        /// <summary>
        /// Gets or sets the text contents of the text box.
        /// </summary>
        /// <value>A string containing the text contents of the text box. The default is an empty string ("").</value>
        /// <remarks>
        /// Getting this property returns a string copy of the contents of the text box. Setting this property replaces the contents of the text box with the specified string.
        /// </remarks>
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
                RaiseTextChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// todo: use BorderStyle for this purpose later.
        /// </summary>
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

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event and calls <see cref="OnTextChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseTextChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
            TextChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the value of the <see cref="Text"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnTextChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            if (EditControlOnly)
                return new NativeTextBoxHandler();

            return base.CreateHandler();
        }
    }
}