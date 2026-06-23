using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Used as a container for other controls.
    /// </summary>
    [ControlCategory(KnownControlCategory.Containers)]
    public partial class Panel : ContainerControl
    {
        /// <summary>
        /// Specifies the default corner radius,
        /// in device-independent units or in percentage, used for rendering the corners of panel elements.
        /// Use of percentage is determined by the value of <see cref="DefaultCornerRadiusIsPercent"/> field.
        /// If percentage is used, the actual corner radius will be calculated as a percentage of the control's size.
        /// </summary>
        internal static float DefaultCornerRadius = 25;

        /// <summary>
        /// Indicates whether the default corner radius is specified as a percentage of the element's dimensions.
        /// </summary>
        internal static bool DefaultCornerRadiusIsPercent = true;

        /// <summary>
        /// Gets or sets whether the panel border has rounded corners by default.
        /// If set to true, the panel will have rounded corners; otherwise, it will have sharp corners.
        /// </summary>
        internal static bool DefaultUseRoundedCorners = true;

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
        protected override BorderCornerRadius? GetDefaultCornerRadius()
        {
            return null;
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreatePanelHandler(this);
        }
    }
}