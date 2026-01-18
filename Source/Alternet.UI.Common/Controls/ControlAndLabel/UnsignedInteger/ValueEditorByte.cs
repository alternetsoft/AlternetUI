using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="byte"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ValueEditorByte : ValueEditorCustom
    {
        /// <summary>
        /// Gets or sets whether to use char validator to limit unwanted chars in the input.
        /// Default is False.
        /// </summary>
        public static bool UseCharValidator = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorByte"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorByte(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

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
            if (UseCharValidator)
                TextBox.UseCharValidator<byte>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
