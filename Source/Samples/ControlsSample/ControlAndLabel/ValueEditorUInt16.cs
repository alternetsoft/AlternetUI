using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorUInt16 : TextBoxAndLabel
    {
        public ValueEditorUInt16(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorUInt16()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.UseValidator<ushort>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
