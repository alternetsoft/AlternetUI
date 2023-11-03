using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorUInt64 : TextBoxAndLabel
    {
        public ValueEditorUInt64(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorUInt64()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.UseValidator<ulong>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
