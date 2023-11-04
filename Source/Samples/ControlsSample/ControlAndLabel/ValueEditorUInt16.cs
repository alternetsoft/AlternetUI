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
        }

        public ValueEditorUInt16()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<ushort>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
