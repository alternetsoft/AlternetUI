using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class ValueEditorInt32 : TextBoxAndLabel
    {
        public ValueEditorInt32(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public ValueEditorInt32()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.UseValidator<int>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
