using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="float"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ValueEditorSingle : ValueEditorCustom
    {
        /// <summary>
        /// Gets or sets whether to use char validator to limit unwanted chars in the input.
        /// Default is False.
        /// </summary>
        public static bool UseCharValidator = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorSingle"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorSingle(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorSingle"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorSingle(string title, float? value = default)
                    : base(title)
        {
            if (value is not null)
                TextBox.SetTextAsSingle(value.Value);
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
            if (UseCharValidator)
                TextBox.UseCharValidator<float>();
            TextBox.SetErrorText(ValueValidatorKnownError.FloatIsExpected);
        }
    }
}
