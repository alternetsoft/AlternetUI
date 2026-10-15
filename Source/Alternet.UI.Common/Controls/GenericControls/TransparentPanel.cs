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
    public partial class GenericRoundBorderWithTitle : GenericRoundBorder
    {
        /// <summary>
        /// Gets or sets the default image for the collapse button in the <see cref="GenericRoundBorderWithTitle"/> control.
        /// </summary>
        public static SvgImage? DefaultCollapseButtonImage = KnownSvgImages.ImgAnglesDown;
        
        /// <summary>
        /// Gets or sets the default image for the expand button in the <see cref="GenericRoundBorderWithTitle"/> control.
        /// </summary>
        public static SvgImage? DefaultExpandButtonImage = KnownSvgImages.ImgAnglesRight;

        private readonly ToolBar toolBar;
        private readonly AbstractControl label;
        private readonly SpeedButton collapseButton;
        private SvgImage? collapseButtonImage = DefaultCollapseButtonImage;
        private SvgImage? expandButtonImage = DefaultExpandButtonImage;
        private bool isCollapsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRoundBorderWithTitle"/> class.
        /// </summary>
        public GenericRoundBorderWithTitle()
        {
            toolBar = new ToolBar();

            label = toolBar.AddTextCore();
            label.IsBold = true;

            collapseButton = toolBar.AddSpeedBtnCore(null, GetCollapseButtonImage(), null, OnCollapseButtonClick);
            collapseButton.HorizontalAlignment = HorizontalAlignment.Right;

            toolBar.Parent = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRoundBorderWithTitle"/>
        /// class with the specified title.
        /// </summary>
        /// <param name="labelTitle">The title of the label.</param>
        public GenericRoundBorderWithTitle(string labelTitle)
            : this()
        {
            label.Text = labelTitle;
        }

        /// <summary>
        /// Gets the title label of the <see cref="GenericRoundBorderWithTitle"/> control.
        /// </summary>
        [Browsable(false)]
        public AbstractControl Label => label;

        /// <summary>
        /// Gets the collapse button of the <see cref="GenericRoundBorderWithTitle"/> control.
        /// </summary>
        [Browsable(false)]
        public SpeedButton CollapseButton => collapseButton;

        /// <summary>
        /// Gets or sets a value indicating whether
        /// the <see cref="GenericRoundBorderWithTitle"/> control can be collapsed.
        /// </summary>
        public virtual bool CanCollapse { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether
        /// the <see cref="GenericRoundBorderWithTitle"/> control can be expanded.
        /// </summary>
        public virtual bool CanExpand { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="GenericRoundBorderWithTitle"/> control is collapsed.
        /// </summary>
        public virtual bool IsCollapsed
        {
            get => isCollapsed;
            set
            {
                if (!CanCollapse && value)
                    return;
                if (!CanExpand && !value)
                    return;
                if (isCollapsed == value)
                    return;
                isCollapsed = value;
                collapseButton.SvgImage = GetCollapseButtonImage();

                DoInsideLayout(() =>
                {
                    foreach (var child in Children)
                    {
                        if (child != toolBar)
                            child.Visible = !isCollapsed;
                    }
                });

                OnCollapsedChanged();
            }
        }

        /// <summary>
        /// Gets the image for the collapse button based on the current state of the <see cref="IsCollapsed"/> property.
        /// </summary>
        /// <returns>The <see cref="SvgImage"/> to be used for the collapse button.</returns>
        protected virtual SvgImage? GetCollapseButtonImage()
        {
            return IsCollapsed
                ? expandButtonImage
                : collapseButtonImage;
        }

        /// <summary>
        /// Called when the <see cref="IsCollapsed"/> property changes.
        /// Override this method to handle the collapse state change.
        /// </summary>
        protected virtual void OnCollapsedChanged()
        {
        }

        /// <summary>
        /// Called when the label is clicked. Override this method to handle the click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnCollapseButtonClick(object? sender, EventArgs e)
        {
            IsCollapsed = !IsCollapsed;
        }
    }
}
