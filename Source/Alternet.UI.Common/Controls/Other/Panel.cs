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
            ParentBackColor = true;
            ParentForeColor = true;
        }

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

    /// <summary>
    /// Represents a <see cref="ScrollViewer{T}"/> descendant that
    /// contains a <see cref="Panel"/> instance.
    /// <see cref="Panel"/> is added as a child and can be accessed using
    /// <see cref="ScrollViewer{Panel}.ScrolledControl"/> property.
    /// Do not add any child controls to this class, use
    /// <see cref="ScrollViewer{Panel}.ScrolledControl"/> instead.
    /// </summary>
    public partial class ScrollablePanel : ScrollViewer<Panel>
    {
    }

    /// <summary>
    /// Implements a panel with rounded corners and a border.
    /// </summary>
    public partial class RoundBorder : Panel
    {
        /// <summary>
        /// Gets or sets the default margin between child controls in the <see cref="RoundBorder"/> control.
        /// </summary>
        public static Thickness DefaultMinChildMargin = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundBorder"/> class.
        /// </summary>
        public RoundBorder()
        {
            RoundCorners().SetHasBorder(true)
            .SetLayout(LayoutStyle.Vertical).SetMinChildMargin(DefaultMinChildMargin);
        }
    }

    /// <summary>
    /// Implements a panel with rounded corners, a border, and a title.
    /// </summary>
    public partial class RoundBorderWithTitle : RoundBorder
    {
        private readonly BoldLabel label;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundBorderWithTitle"/> class.
        /// </summary>
        public RoundBorderWithTitle()
        {
            label = Add<BoldLabel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundBorderWithTitle"/> class with the specified title.
        /// </summary>
        /// <param name="labelTitle">The title of the label.</param>
        public RoundBorderWithTitle(string labelTitle)
            : this()
        {
            label.Text = labelTitle;
        }

        /// <summary>
        /// Gets the title label of the <see cref="RoundBorderWithTitle"/> control.
        /// </summary>
        public BoldLabel Label => label;
    }
}