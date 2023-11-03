using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorUInt32 : TextBoxAndLabel
    {
        public ValueEditorUInt32(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorUInt32()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.UseValidator<uint>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
