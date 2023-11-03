using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorInt64 : TextBoxAndLabel
    {
        public ValueEditorInt64(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorInt64()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.UseValidator<long>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
