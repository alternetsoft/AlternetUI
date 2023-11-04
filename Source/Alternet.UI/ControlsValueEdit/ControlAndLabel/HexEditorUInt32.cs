using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class HexEditorUInt32 : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HexEditorUInt32"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="text">Default value of the Text property.</param>
        public HexEditorUInt32(string title, string? text = default)
                    : base(title, text)
        {
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
            TextBox.Validator = TextBox.CreateValidator(ValueValidatorKind.UnsignedHex);
            TextBox.DataType = typeof(uint);
            TextBox.SetErrorText(ValueValidatorKnownError.HexNumberIsExpected);
        }
    }
}
