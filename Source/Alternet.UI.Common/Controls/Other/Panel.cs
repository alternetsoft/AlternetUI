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
    public partial class SectionPanel : RoundBorder
    {
        private readonly SectionHeaderToolBar toolBar;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionPanel"/> class.
        /// </summary>
        public SectionPanel()
        {
            toolBar = new();
            toolBar.Parent = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionPanel"/> class with the specified title.
        /// </summary>
        /// <param name="labelTitle">The title of the label.</param>
        public SectionPanel(string labelTitle)
            : this()
        {
            Title = labelTitle;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SectionPanel"/> control is collapsed.
        /// </summary>
        public bool IsCollapsed
        {
            get => toolBar.IsCollapsed;
            set => toolBar.IsCollapsed = value;
        }

        /// <summary>
        /// Gets the header toolbar of the <see cref="SectionPanel"/> control.
        /// </summary>
        public SectionHeaderToolBar HeaderToolBar => toolBar;

        /// <summary>
        /// Gets or sets the title format of the <see cref="GenericSectionPanel"/> control.
        /// This format is used to construct the title in the header toolbar.
        /// Title format is specified in the same way as in <see cref="string.Format(string, object[])"/>.
        /// </summary>
        public virtual string? TitleFormat
        {
            get;

            set
            {
                if (field == value)
                    return;
                field = value;
                UpdateToolBarTitle();
            }
        }

        /// <summary>
        /// Gets or sets the format arguments for the title of the <see cref="GenericSectionPanel"/> control.
        /// These arguments are used in conjunction with the <see cref="TitleFormat"/> property to construct the title.
        /// </summary>
        public virtual object[]? TitleFormatArgs
        {
            get;
            set
            {
                if (field == value)
                    return;
                field = value;
                UpdateToolBarTitle();
            }
        }

        /// <inheritdoc/>
        protected override void OnTitleChanged(EventArgs e)
        {
            base.OnTitleChanged(e);
            UpdateToolBarTitle();
        }

        /// <summary>
        /// Updates the title of the <see cref="GenericSectionPanel"/>
        /// control based on the current title and format arguments.
        /// By default, <see cref="TitleFormat"/> and <see cref="TitleFormatArgs"/> are used to construct the title
        /// using the same formatting rules as in <see cref="string.Format(string, object[])"/>.
        /// Override this method to customize the title update behavior.
        /// </summary>
        protected virtual void UpdateToolBarTitle()
        {
            if (string.IsNullOrEmpty(TitleFormat) || TitleFormatArgs is null)
                toolBar.Label.Text = Title;
            else
                toolBar.Label.Text = string.Format(TitleFormat, TitleFormatArgs);
        }
    }

    /// <summary>
    /// Implements a panel with square corners, a border, and a title.
    /// </summary>
    public partial class SquareBorderWithTitle : SectionPanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SquareBorderWithTitle"/> class.
        /// </summary>
        public SquareBorderWithTitle()
        {
            SquareCorners();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SquareBorderWithTitle"/>
        /// class with the specified title.
        /// </summary>
        /// <param name="labelTitle">The title of the label.</param>
        public SquareBorderWithTitle(string labelTitle)
            : this()
        {
            HeaderToolBar.Label.Text = labelTitle;
        }
    }
}