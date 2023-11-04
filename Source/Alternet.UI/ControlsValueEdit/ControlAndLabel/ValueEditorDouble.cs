using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements <see cref="double"/> editor with validation.
    /// </summary>
    public class ValueEditorDouble : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorDouble"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorDouble(string title, double? value = default)
                    : base(title)
        {
            if (value is not null)
                TextBox.SetTextAsDouble(value.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorDouble"/> class.
        /// </summary>
        public ValueEditorDouble()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<double>();
            TextBox.SetErrorText(ValueValidatorKnownError.FloatIsExpected);
        }
    }
}
