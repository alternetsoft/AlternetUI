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
    [ControlCategory("Hidden")]
    public partial class ValueEditorCustom : TextBoxAndLabel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorCustom"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorCustom(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorCustom"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="text">Default value of the Text property.</param>
        public ValueEditorCustom(string title, string? text = default)
                    : base(title, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorCustom"/> class.
        /// </summary>
        public ValueEditorCustom()
            : base()
        {
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.Options |= TextBoxOptions.DefaultValidation;
        }
    }
}
