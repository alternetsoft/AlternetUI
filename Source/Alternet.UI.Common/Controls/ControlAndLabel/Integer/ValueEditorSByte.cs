using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="sbyte"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ValueEditorSByte : ValueEditorCustom
    {
        /// <summary>
        /// Gets or sets whether to use char validator to limit unwanted chars in the input.
        /// Default is False.
        /// </summary>
        public static bool UseCharValidator = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorSByte"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorSByte(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

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
            if (UseCharValidator)
                TextBox.UseCharValidator<sbyte>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
