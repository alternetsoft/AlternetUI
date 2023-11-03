using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorUDouble : TextBoxAndLabel
    {
        public ValueEditorUDouble(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorUDouble()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.MinValue = 0d;
            TextBox.UseValidator<double>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedFloatIsExpected);
        }
    }
}
