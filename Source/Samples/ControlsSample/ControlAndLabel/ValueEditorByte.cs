using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorByte : TextBoxAndLabel
    {
        public ValueEditorByte(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorByte()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.UseValidator<byte>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
