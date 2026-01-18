using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Used as a container for other controls.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class Panel : ContainerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public Panel(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        public Panel()
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

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            base.DefaultPaint(e);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreatePanelHandler(this);
        }
    }
}