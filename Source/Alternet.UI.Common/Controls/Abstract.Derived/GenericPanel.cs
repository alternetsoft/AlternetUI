using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Used as a container for generic other controls.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class GenericPanel : GenericContainerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericPanel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public GenericPanel(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericPanel"/> class.
        /// </summary>
        public GenericPanel()
        {
            TabStop = false;
            CanSelect = false;
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Panel;

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }
    }
}