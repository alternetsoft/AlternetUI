using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements <see cref="short"/> editor with validation.
    /// </summary>
    public class ValueEditorInt16 : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorInt16"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorInt16(string title, short? value = default)
                    : base(title)
        {
            if (value is not null)
                TextBox.SetTextAsInt16(value.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorInt16"/> class.
        /// </summary>
        public ValueEditorInt16()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<short>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
