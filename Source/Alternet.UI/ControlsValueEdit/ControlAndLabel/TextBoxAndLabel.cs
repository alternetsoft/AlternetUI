using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class TextBoxAndLabel : ControlAndLabel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxAndLabel"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="text">Default value of the <see cref="Text"/> property.</param>
        public TextBoxAndLabel(string title, string? text = default)
            : this()
        {
            Title = title;
            if (text is not null)
                TextBox.Text = text;
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxAndLabel"/> class.
        /// </summary>
        public TextBoxAndLabel()
            : base()
        {
            Init();
        }

        public event EventHandler? TextChanged;

        /// <summary>
        /// Gets main child control.
        /// </summary>
        public new TextBox MainControl => (TextBox)base.MainControl;

        /// <summary>
        /// Gets main child control, same as <see cref="MainControl"/>.
        /// </summary>
        public TextBox TextBox => (TextBox)base.MainControl;

        /// <summary>
        /// Gets or sets <see cref="TextBox.Text"/> property of the main child control.
        /// </summary>
        public string Text
        {
            get
            {
                return TextBox.Text;
            }

            set
            {
                TextBox.Text = value;
            }
        }

        /// <inheritdoc/>
        protected override Control CreateControl() => new TextBox();

        /// <summary>
        /// Initializes main child control.
        /// </summary>
        /// <remarks>
        /// Override this method to do custom initialization. Do not call this method
        /// directly, it is called automatically from constructor. When overriding you
        /// must call base method before any other initializations.
        /// </remarks>
        protected virtual void Init()
        {
            MainControl.ValidatorReporter = ErrorPicture;
            MainControl.TextChanged += MainControl_TextChanged;
        }

        /// <summary>
        /// Called when text is changed in the main child control.
        /// </summary>
        protected virtual void MainControlTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MainControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainControlTextChanged();
        }
    }
}
