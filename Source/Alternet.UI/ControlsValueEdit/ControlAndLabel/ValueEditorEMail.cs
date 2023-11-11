using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements e-mail editor with validation.
    /// </summary>
    public class ValueEditorEMail : ValueEditorString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorEMail"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="email">Default e-mail value.</param>
        public ValueEditorEMail(string title, string? email = default)
                    : base(title, email)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorEMail"/> class.
        /// </summary>
        public ValueEditorEMail()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override bool IsValidText => IsValidMail;

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.SetErrorText(ValueValidatorKnownError.EMailIsExpected);
        }
    }
}
