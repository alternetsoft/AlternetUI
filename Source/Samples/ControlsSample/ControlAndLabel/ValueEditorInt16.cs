using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorInt16 : TextBoxAndLabel
    {
        public ValueEditorInt16(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorInt16()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.UseValidator<short>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
