using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements transparent panel which is handled inside the library.
    /// This control doesn't have native handle, border, or background.
    /// It can be used to layout other generic controls.
    /// Do not add <see cref="Control"/> descendants to this panel, only generic control are allowed.
    /// </summary>
    public partial class TransparentPanel : HiddenGenericBorder
    {
        private bool ignoreTransparency;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransparentPanel"/> class.
        /// </summary>
        public TransparentPanel()
        {
            UserPaint = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control should ignore transparency and paint its background.
        /// Default is false, which means the control will be transparent and not paint its background.
        /// </summary>
        [Browsable(true)]
        public virtual bool IgnoreTransparency
        {
            get
            {
                return ignoreTransparency;
            }

            set
            {
                if(ignoreTransparency == value)
                    return;
                ignoreTransparency = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Sets the <see cref="IgnoreTransparency"/> property and returns
        /// the current instance of <see cref="TransparentPanel"/>.
        /// </summary>
        /// <param name="value">The value to set for the <see cref="IgnoreTransparency"/> property.</param>
        /// <returns>The current instance of <see cref="TransparentPanel"/>.</returns>
        public TransparentPanel SetIgnoreTransparency(bool value = true)
        {
            IgnoreTransparency = value;
            return this;
        }

        /// <summary>
        /// Adds the specified child controls to the container.
        /// </summary>
        /// <param name="children">The child controls to add.</param>
        /// <returns>The current instance of <see cref="TransparentPanel"/>.</returns>
        public virtual TransparentPanel WithChildren(params GenericControl[] children)
        {
            DoInsideLayout(() =>
            {
                Children.AddRange(children);
            });

            return this;
        }

        /// <inheritdoc/>
        public sealed override void DefaultPaint(PaintEventArgs e)
        {
            if (IgnoreTransparency)
                base.DefaultPaint(e);
        }

        /// <inheritdoc/>
        protected sealed override void OnPaint(PaintEventArgs e)
        {
            if (IgnoreTransparency)
                base.OnPaint(e);
        }

        /// <inheritdoc/>
        protected sealed override void OnPaintBackground(PaintEventArgs e)
        {
            if (IgnoreTransparency)
                base.OnPaintBackground(e);
        }
    }

    /// <summary>
    /// Implements a generic panel with rounded corners and a border.
    /// </summary>
    public partial class GenericRoundBorder : TransparentPanel
    {
        /// <summary>
        /// Gets or sets the default margin between child controls in the <see cref="GenericRoundBorder"/> control.
        /// </summary>
        public static Thickness DefaultMinChildMargin = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRoundBorder"/> class.
        /// </summary>
        public GenericRoundBorder()
        {
            SetIgnoreTransparency().RoundCorners().SetHasBorder(true)
            .SetLayout(LayoutStyle.Vertical).SetMinChildMargin(DefaultMinChildMargin);
        }
    }

    /// <summary>
    /// Implements a generic panel with rounded corners, a border, and a title.
    /// </summary>
    public partial class GenericSectionPanel : GenericRoundBorder
    {
        private readonly SectionHeaderToolBar toolBar;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSectionPanel"/> class.
        /// </summary>
        public GenericSectionPanel()
        {
            toolBar = new ();
            toolBar.Parent = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSectionPanel"/>
        /// class with the specified title.
        /// </summary>
        /// <param name="labelTitle">The title of the label.</param>
        public GenericSectionPanel(string labelTitle)
            : this()
        {
            Title = labelTitle;
        }

        /// <summary>
        /// Gets the header toolbar of the <see cref="GenericSectionPanel"/> control.
        /// </summary>
        public SectionHeaderToolBar HeaderToolBar => toolBar;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SectionPanel"/> control is collapsed.
        /// </summary>
        public bool IsCollapsed
        {
            get => toolBar.IsCollapsed;
            set => toolBar.IsCollapsed = value;
        }

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
            if(string.IsNullOrEmpty(TitleFormat) || TitleFormatArgs is null)
                toolBar.Label.Text = Title;
            else
                toolBar.Label.Text = string.Format(TitleFormat, TitleFormatArgs);
        }
    }

    /// <summary>
    /// Implements a generic panel with square corners, a border, and a title.
    /// </summary>
    public partial class GenericSquareBorderWithTitle : GenericSectionPanel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSquareBorderWithTitle"/> class.
        /// </summary>
        public GenericSquareBorderWithTitle()
        {
            SquareCorners();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSquareBorderWithTitle"/>
        /// class with the specified title.
        /// </summary>
        /// <param name="labelTitle">The title of the label.</param>
        public GenericSquareBorderWithTitle(string labelTitle)
            : this()
        {
            HeaderToolBar.Label.Text = labelTitle;
        }
    }
}
