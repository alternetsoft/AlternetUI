using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements unsigned <see cref="float"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public class ValueEditorUSingle : ValueEditorCustom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUSingle"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="value">Default value.</param>
        public ValueEditorUSingle(string title, float? value = default)
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

                TextBox.SetTextAsSingle(value.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUSingle"/> class.
        /// </summary>
        public ValueEditorUSingle()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.MinValue = 0f;
            TextBox.UseValidator<float>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedFloatIsExpected);
        }
    }
}
