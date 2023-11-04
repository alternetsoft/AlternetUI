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

        public new TextBox MainControl => (TextBox)base.MainControl;

        public TextBox TextBox => (TextBox)base.MainControl;

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

        protected virtual void Init()
        {
            MainControl.ValidatorReporter = ErrorPicture;
            MainControl.TextChanged += MainControl_TextChanged;
        }

        protected virtual void MainControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }
    }
}
