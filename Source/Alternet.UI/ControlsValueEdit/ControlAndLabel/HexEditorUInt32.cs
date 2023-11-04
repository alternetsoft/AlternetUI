using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements hexadecimal <see cref="uint"/> editor with validation.
    /// </summary>
    public class HexEditorUInt32 : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HexEditorUInt32"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public HexEditorUInt32(string title, uint? value = default)
                    : base(title)
        {
            if(value is not null)
                TextBox.SetTextAsUInt32(value.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HexEditorUInt32"/> class.
        /// </summary>
        public HexEditorUInt32()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.NumberStyles = NumberStyles.HexNumber;
            TextBox.DefaultFormat = "X";
            TextBox.Validator = TextBox.CreateValidator(ValueValidatorKind.UnsignedHex);
            TextBox.DataType = typeof(uint);
            TextBox.SetErrorText(ValueValidatorKnownError.HexNumberIsExpected);
        }
    }
}
