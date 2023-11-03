using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class HexEditorUInt32 : TextBoxAndLabel
    {
        public HexEditorUInt32(string title, string? text = default)
            : base(title, text)
        {
            Init();
        }

        public HexEditorUInt32()
            : base()
        {
            Init();
        }

        private void Init()
        {
            TextBox.NumberStyles = NumberStyles.HexNumber;
            TextBox.Validator = TextBox.CreateValidator(ValueValidatorKind.UnsignedHex);
            TextBox.DataType = typeof(uint);
            TextBox.SetErrorText(ValueValidatorKnownError.HexNumberIsExpected);
        }
    }
}
