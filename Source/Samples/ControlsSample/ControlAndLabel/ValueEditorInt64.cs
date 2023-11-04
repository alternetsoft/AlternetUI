using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorInt64 : ValueEditorCustom
    {
        public ValueEditorInt64(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorInt64()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<long>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
