using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements unsigned <see cref="float"/> editor with validation.
    /// </summary>
    public class ValueEditorUSingle : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUSingle"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="text">Default value of the Text property.</param>
        public ValueEditorUSingle(string title, string? text = default)
                    : base(title, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUSingle"/> class.
        /// </summary>
        public ValueEditorUSingle()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.MinValue = 0f;
            TextBox.UseValidator<float>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedFloatIsExpected);
        }
    }
}
