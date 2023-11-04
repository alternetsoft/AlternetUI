using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorSByte : ValueEditorCustom
    {
        public ValueEditorSByte(string title, string? text = default)
                    : base(title, text)
        {
        }

        public ValueEditorSByte()
            : base()
        {
        }

        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<sbyte>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
