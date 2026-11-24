using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for the custom value editors.
    /// </summary>
    [ControlCategory(KnownControlCategory.Hidden)]
    public partial class ValueEditorGeneric : TextPickerAndLabel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorGeneric"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorGeneric(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorGeneric"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="text">Default value of the Text property.</param>
        public ValueEditorGeneric(string title, string? text = default)
                    : base(title, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorGeneric"/> class.
        /// </summary>
        public ValueEditorGeneric()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.ValueHelper.Options |= TextBoxOptions.DefaultValidation;
        }
    }
}
