using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that can be used as a resizable window border
    /// with title, icon and caption buttons.
    /// </summary>
    public partial class ResizableWindowBorder : ResizableBorder
    {
        /// <summary>
        /// Gets or sets the default icon margin.
        /// </summary>
        public static Thickness DefaultIconMargin = (5, 0, 0, 0);
        
        /// <summary>
        /// Gets or sets the default title margin.
        /// </summary>
        public static Thickness DefaultTitleMargin = (5, 0, 5, 0);

        private readonly GripControl gripControl;
        private readonly Label label;
        private readonly SpeedButton minimizeButton;
        private readonly SpeedButton maximizeButton;
        private readonly SpeedButton closeButton;
        private readonly ImageSizeFallbackOptions fallbackOptions;
        private readonly ToolBar toolBar;
        private readonly PictureBox icon;

        private bool isActiveBorder = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizableWindowBorder"/> class.
        /// </summary>
        public ResizableWindowBorder()
        {
            HasBorder = false;
            AutoUpdateColors = true;

            this.UseWindowBorderSize(Window.FrameMetrics.BorderMetricKind.ThickFrame);

            toolBar = new ToolBar();
            toolBar.AutoUpdateColors = false;
            toolBar.ResetSuggestedSize();
            toolBar.Dock = DockStyle.Top;
            toolBar.MinHeight = Coord.Max(Window.FrameMetrics.GetCaptionAreaHeight(App.SafeWindow), ToolBar.DefaultMinItemSize);
            toolBar.Parent = this.FillPanel;

            AssignDefaultColors();

            fallbackOptions = new ImageSizeFallbackOptions();
            fallbackOptions.AllowScaled = true;

            icon = toolBar.AddIcon(KnownIcons.Default, fallbackOptions);
            icon.Margin = DefaultIconMargin;

            gripControl = new GripControl();
            gripControl.ConfigureAsMovingGrip();
            gripControl.VerticalAlignment = VerticalAlignment.Stretch;
            gripControl.Target = this;
            gripControl.HorizontalAlignment = HorizontalAlignment.Fill;

            label = new Label();
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Margin = DefaultTitleMargin;
            label.InputTransparent = true;
            label.Parent = gripControl;

            toolBar.AddControl(gripControl);

            minimizeButton = toolBar.AddSpeedBtnCore(null, KnownSvgImages.ImgWindowMinimize, CommonStrings.Default.ButtonMinimize);

            minimizeButton.HorizontalAlignment = HorizontalAlignment.Right;

            maximizeButton = toolBar.AddSpeedBtnCore(null, KnownSvgImages.ImgWindowMaximize, CommonStrings.Default.ButtonMaximize);

            maximizeButton.HorizontalAlignment = HorizontalAlignment.Right;

            closeButton = toolBar.AddSpeedBtnCore(null, KnownSvgImages.ImgClose, CommonStrings.Default.ButtonClose);

            closeButton.HorizontalAlignment = HorizontalAlignment.Right;
        }

        /// <summary>
        /// Occurs when the minimize button is clicked.
        /// </summary>
        public event EventHandler MinimizeButtonClick
        {
            add
            {
                minimizeButton.Click += value;
            }
            remove
            {
                minimizeButton.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when the close button is clicked.
        /// </summary>
        public event EventHandler CloseButtonClick
        {
            add
            {
                closeButton.Click += value;
            }
            remove
            {
                closeButton.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when the maximize button is clicked.
        /// </summary>
        public event EventHandler MaximizeButtonClick
        {
            add
            {
                maximizeButton.Click += value;
            }
            remove
            {
                maximizeButton.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when the icon is clicked.
        /// </summary>
        public event EventHandler IconClick
        {
            add
            {
                icon.Click += value;
            }

            remove
            {
                icon.Click -= value;
            }
        }

        /// <summary>
        /// Gets grip control used for moving the target control.
        /// </summary>
        [Browsable(false)]
        public GripControl GripControl => gripControl;

        /// <summary>
        /// Gets the label used for displaying the title.
        /// </summary>
        [Browsable(false)]
        public Label TitleLabel => label;

        /// <summary>
        /// Gets the button used for minimizing.
        /// </summary>
        [Browsable(false)]
        public SpeedButton MinimizeButton => minimizeButton;

        /// <summary>
        /// Gets the button used for maximizing.
        /// </summary>
        [Browsable(false)]
        public SpeedButton MaximizeButton => maximizeButton;

        /// <summary>
        /// Gets the button used for closing.
        /// </summary>
        [Browsable(false)]
        public SpeedButton CloseButton => closeButton;

        /// <summary>
        /// Gets icon size fallback options.
        /// </summary>
        [Browsable(false)]
        public ImageSizeFallbackOptions IconSizeFallbackOptions => fallbackOptions;

        /// <summary>
        /// Gets the toolbar used in the title bar.
        /// </summary>
        [Browsable(false)]
        public ToolBar ToolBar => toolBar;

        /// <summary>
        /// Gets the picture box used for displaying the icon.
        /// </summary>
        [Browsable(false)]
        public PictureBox IconPictureBox => icon;

        /// <summary>
        /// Gets or sets a value indicating whether this border is active.
        /// </summary>
        public virtual bool IsActiveBorder
        {
            get => isActiveBorder;
            set
            {
                if (isActiveBorder != value)
                {
                    isActiveBorder = value;
                    AssignDefaultColors();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether close button is visible and enabled.
        /// </summary>
        public virtual bool CloseEnabled
        {
            get => closeButton.Visible;

            set
            {
                closeButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether maximize button is visible and enabled.
        /// </summary>
        public virtual bool MaximizeEnabled
        {
            get => maximizeButton.Visible;

            set
            {
                maximizeButton.Visible = value;
            }
        }


        /// <summary>
        /// Gets or sets whether system menu is visible.
        /// </summary>
        public virtual bool HasSystemMenu
        {
            get
            {
                return IconPictureBox.IsVisible;
            }

            set
            {
                IconPictureBox.IsVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating whether title bar is visible.
        /// </summary>
        public virtual bool HasTitleBar
        {
            get => toolBar.Visible;
            set => toolBar.Visible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether minimize button is visible and enabled.
        /// </summary>
        public virtual bool MinimizeEnabled
        {
            get => minimizeButton.Visible;

            set
            {
                minimizeButton.Visible = value;
            }
        }

        /// <inheritdoc/>
        protected override void OnTitleChanged(EventArgs e)
        {
            base.OnTitleChanged(e);
            label.Text = Title;
        }

        /// <summary>
        /// Assigns default colors to the control based on the active state and color theme.
        /// </summary>
        protected virtual void AssignDefaultColors()
        {
            if (!AutoUpdateColors)
                return;

            var isDark = IsDarkBackground;

            toolBar.DoInsideUpdate(() =>
            {
                toolBar.ParentBackColor = false;
                toolBar.ParentForeColor = false;
                toolBar.BackColor = DefaultColors.GetEffectiveWindowCaptionColor(isDark, isActiveBorder);
                toolBar.ForeColor = DefaultColors.GetEffectiveWindowCaptionTextColor(isDark, isActiveBorder);
            });

            this.UseWindowBorderColors(isDark, isActiveBorder);
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            AssignDefaultColors();
        }
    }
}
