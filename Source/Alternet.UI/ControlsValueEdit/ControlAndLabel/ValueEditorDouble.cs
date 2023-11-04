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
        /// <param name="text">Default value of the Text property.</param>
        public ValueEditorDouble(string title, string? text = default)
                    : base(title, text)
        {
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
