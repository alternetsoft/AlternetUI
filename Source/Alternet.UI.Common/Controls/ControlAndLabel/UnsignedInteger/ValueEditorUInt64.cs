using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="ulong"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ValueEditorUInt64 : ValueEditorCustom
    {
        /// <summary>
        /// Gets or sets whether to use char validator to limit unwanted chars in the input.
        /// Default is False.
        /// </summary>
        public static bool UseCharValidator = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUInt64"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorUInt64(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUInt64"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorUInt64(string title, ulong? value = default)
                    : base(title)
        {
            if (value is not null)
                TextBox.SetTextAsUInt64(value.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUInt64"/> class.
        /// </summary>
        public ValueEditorUInt64()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            if (UseCharValidator)
                TextBox.UseCharValidator<ulong>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
