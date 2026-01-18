using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements multiline text editor.
    /// </summary>
    [ControlCategory("Common")]
    public partial class MultilineTextBox : TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultilineTextBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public MultilineTextBox(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultilineTextBox"/> class.
        /// </summary>
        public MultilineTextBox()
        {
            WantTab = true;
            base.Multiline = true;

            bool? hasBorder = AllPlatformDefaults.GetHasBorderOverride(ControlKind);

            if (hasBorder is not null)
                HasBorder = hasBorder.Value;
        }

        /// <summary>
        /// Always returns <c>false</c>.
        /// </summary>
        public override bool CanUserPaint => false;

        /// <summary>
        /// Always returns <c>true</c>.
        /// </summary>
        public override bool Multiline
        {
            get => true;

            set
            {
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.MultilineTextBox;
    }
}
