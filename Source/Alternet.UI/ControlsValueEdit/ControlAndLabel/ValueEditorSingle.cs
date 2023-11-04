using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements <see cref="float"/> editor with validation.
    /// </summary>
    public class ValueEditorSingle : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorSingle"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="text">Default value of the Text property.</param>
        public ValueEditorSingle(string title, string? text = default)
                    : base(title, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorSingle"/> class.
        /// </summary>
        public ValueEditorSingle()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<float>();
            TextBox.SetErrorText(ValueValidatorKnownError.FloatIsExpected);
        }
    }
}
