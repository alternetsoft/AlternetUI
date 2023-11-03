using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorUSingle : TextBoxAndLabel
    {
        public ValueEditorUSingle(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorUSingle()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.MinValue = 0f;
            TextBox.UseValidator<float>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedFloatIsExpected);
        }
    }
}
