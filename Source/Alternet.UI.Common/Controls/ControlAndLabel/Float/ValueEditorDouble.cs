using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="double"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ValueEditorDouble : ValueEditorCustom
    {
        /// <summary>
        /// Gets or sets whether to use char validator to limit unwanted chars in the input.
        /// Default is False.
        /// </summary>
        public static bool UseCharValidator = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorDouble"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorDouble(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

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
            if (UseCharValidator)
                TextBox.UseCharValidator<double>();
            TextBox.SetErrorText(ValueValidatorKnownError.FloatIsExpected);
        }
    }
}
