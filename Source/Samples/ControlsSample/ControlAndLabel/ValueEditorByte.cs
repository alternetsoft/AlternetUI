using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorByte : ValueEditorCustom
    {
        public ValueEditorByte(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorByte()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<byte>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
