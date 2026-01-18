using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="uint"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ValueEditorUInt32 : ValueEditorCustom
    {
        /// <summary>
        /// Gets or sets whether to use char validator to limit unwanted chars in the input.
        /// Default is False.
        /// </summary>
        public static bool UseCharValidator = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUInt32"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorUInt32(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUInt32"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorUInt32(string title, uint? value = default)
                    : base(title)
        {
            if (value is not null)
                TextBox.SetTextAsUInt32(value.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUInt32"/> class.
        /// </summary>
        public ValueEditorUInt32()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            if (UseCharValidator)
                TextBox.UseCharValidator<uint>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
