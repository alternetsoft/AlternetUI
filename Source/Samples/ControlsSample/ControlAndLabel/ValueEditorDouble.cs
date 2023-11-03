using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorDouble : TextBoxAndLabel
    {
        public ValueEditorDouble(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorDouble()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.UseValidator<double>();
            TextBox.SetErrorText(ValueValidatorKnownError.FloatIsExpected);
        }
    }
}
