using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements <see cref="ulong"/> editor with validation.
    /// </summary>
    public class ValueEditorUInt64 : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUInt64"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="text">Default value of the Text property.</param>
        public ValueEditorUInt64(string title, string? text = default)
                    : base(title, text)
        {
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
            TextBox.UseValidator<ulong>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
