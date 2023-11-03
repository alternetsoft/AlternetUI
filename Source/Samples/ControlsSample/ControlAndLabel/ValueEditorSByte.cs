using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorSByte : TextBoxAndLabel
    {
        public ValueEditorSByte(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorSByte()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.UseValidator<sbyte>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
