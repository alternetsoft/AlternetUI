using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a panel with <see cref="LayoutStyle.Dock"/> layout style.
    /// </summary>
    /// <remarks>
    /// Use <see cref="AbstractControl.Dock"/> property of the child control to specify its dock style.
    /// If dock style of the child control is not specified, control is positioned absolutely using
    /// its <see cref="AbstractControl.Bounds"/> property.
    /// </remarks>
    [ControlCategory(KnownControlCategory.Containers)]
    public partial class LayoutPanel : ContainerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutPanel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public LayoutPanel(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutPanel"/> class.
        /// </summary>
        public LayoutPanel()
        {
            CanSelect = false;
            TabStop = false;
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.LayoutPanel;

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        /// <inheritdoc/>
        protected override LayoutStyle GetDefaultLayout()
        {
            return LayoutStyle.Dock;
        }
    }
}
