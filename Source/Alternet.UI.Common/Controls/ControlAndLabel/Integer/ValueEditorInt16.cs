using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="short"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ValueEditorInt16 : ValueEditorCustom
    {
        /// <summary>
        /// Gets or sets whether to use char validator to limit unwanted chars in the input.
        /// Default is False.
        /// </summary>
        public static bool UseCharValidator = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorInt16"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorInt16(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorInt16"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorInt16(string title, short? value = default)
                    : base(title)
        {
            if (value is not null)
                TextBox.SetTextAsInt16(value.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorInt16"/> class.
        /// </summary>
        public ValueEditorInt16()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            if(UseCharValidator)
                TextBox.UseCharValidator<short>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
