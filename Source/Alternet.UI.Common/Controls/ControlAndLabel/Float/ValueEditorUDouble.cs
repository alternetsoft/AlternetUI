using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements unsigned <see cref="double"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ValueEditorUDouble : ValueEditorCustom
    {
        /// <summary>
        /// Gets or sets whether to use char validator to limit unwanted chars in the input.
        /// Default is False.
        /// </summary>
        public static bool UseCharValidator = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUDouble"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorUDouble(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUDouble"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorUDouble(string title, double? value = default)
                    : base(title)
        {
            if (value is not null)
            {
                if (value.Value < 0)
                {
                    throw new ArgumentException(
                        ErrorMessages.Default.ValidationUnsignedFloatIsExpected,
                        nameof(value));
                }

                TextBox.SetTextAsDouble(value.Value);
            }
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
            if (UseCharValidator)
                TextBox.UseCharValidator<double>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedFloatIsExpected);
        }
    }
}
