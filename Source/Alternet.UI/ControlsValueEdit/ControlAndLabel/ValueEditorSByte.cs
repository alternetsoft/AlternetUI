using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements <see cref="sbyte"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public class ValueEditorSByte : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorSByte"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorSByte(string title, sbyte? value = default)
                    : base(title)
        {
            if (value is not null)
                TextBox.SetTextAsSByte(value.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorSByte"/> class.
        /// </summary>
        public ValueEditorSByte()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<sbyte>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
