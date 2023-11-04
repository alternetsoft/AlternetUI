using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorUDouble : ValueEditorCustom
    {
        public ValueEditorUDouble(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorUDouble()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.MinValue = 0d;
            TextBox.UseValidator<double>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedFloatIsExpected);
        }
    }
}
