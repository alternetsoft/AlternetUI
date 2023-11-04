using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements unsigned <see cref="double"/> editor with validation.
    /// </summary>
    public class ValueEditorUDouble : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUDouble"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="text">Default value of the Text property.</param>
        public ValueEditorUDouble(string title, string? text = default)
                    : base(title, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUDouble"/> class.
        /// </summary>
        public ValueEditorUDouble()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.MinValue = 0d;
            TextBox.UseValidator<double>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedFloatIsExpected);
        }
    }
}
