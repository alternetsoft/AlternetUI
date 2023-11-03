using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class TextBoxAndLabel : ControlAndLabel
    {
        public TextBoxAndLabel(string title, string? text = default)
            : this()
        {
            Title = title;
            if (text is not null)
                TextBox.Text = text;
        }

        public TextBoxAndLabel()
            : base()
        {
            MainControl.ValidatorReporter = ErrorPicture;
        }

        public new TextBox MainControl => (TextBox)base.MainControl;

        public TextBox TextBox => (TextBox)base.MainControl;

        /// <inheritdoc/>
        protected override Control CreateControl() => new TextBox();
    }
}
