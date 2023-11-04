using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorEMail : ValueEditorCustom
    {
        public ValueEditorEMail(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorEMail()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.SetErrorText(ValueValidatorKnownError.EMailIsExpected);
            TextBox.TextChanged += TextBox_TextChanged;
            TextBox.Options &= ~TextBoxOptions.DefaultValidation;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox.ReportErrorEmptyText())
                return;

            var text = TextBox.Text;

            if (string.IsNullOrEmpty(text) || ValueValidatorFactory.IsValidMailAddress(text))
                TextBox.ReportValidatorError(false);
            else
                TextBox.ReportValidatorError(true);
        }
    }
}
