﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Imlements <see cref="uint"/> editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ValueEditorUInt32 : ValueEditorCustom
    {
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
            TextBox.UseValidator<uint>();
            TextBox.SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
        }
    }
}
