using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements <see cref="ushort"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public class ValueEditorUInt16 : TextBoxAndLabel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUInt16"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorUInt16(string title, ushort? value = default)
                   : base(title)
        {
            if (value is not null)
                TextBox.SetTextAsUInt16(value.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUInt16"/> class.
        /// </summary>
        public ValueEditorUInt16()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<ushort>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
