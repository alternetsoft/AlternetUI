using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorSingle : TextBoxAndLabel
    {
        public ValueEditorSingle(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorSingle()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.UseValidator<float>();
            TextBox.SetErrorText(ValueValidatorKnownError.FloatIsExpected);
        }
    }
}
