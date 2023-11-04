using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorInt16 : ValueEditorCustom
    {
        public ValueEditorInt16(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorInt16()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<short>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
