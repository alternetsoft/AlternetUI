using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorDouble : ValueEditorCustom
    {
        public ValueEditorDouble(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorDouble()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<double>();
            TextBox.SetErrorText(ValueValidatorKnownError.FloatIsExpected);
        }
    }
}
