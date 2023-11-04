using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements <see cref="int"/> editor with validation.
    /// </summary>
    public class ValueEditorInt32 : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorInt32"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorInt32(string title, int? value = default)
                    : base(title)
        {
            if (value is not null)
                TextBox.SetTextAsInt32(value.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorInt32"/> class.
        /// </summary>
        public ValueEditorInt32()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.UseValidator<int>();
            TextBox.SetErrorText(ValueValidatorKnownError.NumberIsExpected);
        }
    }
}
