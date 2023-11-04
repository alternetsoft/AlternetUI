using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements <see cref="byte"/> editor with validation.
    /// </summary>
    public class ValueEditorByte : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorByte"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorByte(string title, byte? value = default)
                    : base(title)
        {
            if (value is not null)
                TextBox.SetTextAsByte(value.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorByte"/> class.
        /// </summary>
        public ValueEditorByte()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<byte>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
