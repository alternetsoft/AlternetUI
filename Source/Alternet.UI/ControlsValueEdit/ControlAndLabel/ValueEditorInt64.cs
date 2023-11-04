using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements <see cref="long"/> editor with validation.
    /// </summary>
    public class ValueEditorInt64 : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorInt64"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorInt64(string title, long? value = default)
                    : base(title)
        {
            if (value is not null)
                TextBox.SetTextAsInt64(value.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorInt64"/> class.
        /// </summary>
        public ValueEditorInt64()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<long>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
