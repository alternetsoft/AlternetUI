using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorUInt64 : ValueEditorCustom
    {
        public ValueEditorUInt64(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorUInt64()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<ulong>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
