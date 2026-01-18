using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="string"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ValueEditorString : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorString"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorString(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorString"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="text">Default value.</param>
        public ValueEditorString(string title, string? text = default)
                    : base(title, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorString"/> class.
        /// </summary>
        public ValueEditorString()
            : base()
        {
        }

        /// <summary>
        /// Occurs when text validation need to be performed.
        /// </summary>
        public event ValidationEventHandler? TextValidation;

        /// <summary>
        /// Gets whether text is valid. Override to provide validation.
        /// </summary>
        protected virtual bool IsValidText => true;

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.Options &= ~TextBoxOptions.DefaultValidation;
        }

        /// <inheritdoc/>
        protected override void MainControlTextChanged()
        {
            base.MainControlTextChanged();

            if (TextBox.ReportErrorEmptyText())
                return;

            if (TextValidation is not null)
            {
                ValidationEventArgs e = new(Text);
                TextValidation(this, e);
                if (e.IsValid is not null)
                {
                    TextBox.ReportValidatorError(!e.IsValid.Value);
                    return;
                }
            }

            TextBox.ReportValidatorError(!(IsNullOrEmpty || IsValidText));
        }
    }
}
