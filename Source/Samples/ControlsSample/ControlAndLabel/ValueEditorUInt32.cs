using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorUInt32 : ValueEditorCustom
    {
        public ValueEditorUInt32(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorUInt32()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<uint>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
