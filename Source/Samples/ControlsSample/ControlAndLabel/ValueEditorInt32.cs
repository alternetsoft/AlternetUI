using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorInt32 : ValueEditorCustom
    {
        public ValueEditorInt32(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorInt32()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<int>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
