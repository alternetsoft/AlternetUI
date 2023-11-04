using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorUSingle : ValueEditorCustom
    {
        public ValueEditorUSingle(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorUSingle()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.MinValue = 0f;
            TextBox.UseValidator<float>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedFloatIsExpected);
        }
    }
}
