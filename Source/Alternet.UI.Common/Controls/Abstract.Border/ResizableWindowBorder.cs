using System;
using System.Collections.Generic;
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
        public static Thickness DefaultIconMargin = (5, 0, 0, 0);
        
        public static Thickness DefaultLabelMargin = (5, 0, 5, 0);

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

            toolBar.ParentBackColorChanged += (s, e) =>
            {
            };

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
            label.Margin = DefaultLabelMargin;
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

        public GripControl GripControl => gripControl;

        public Label TitleLabel => label;

        public SpeedButton MinimizeButton => minimizeButton;

        public SpeedButton MaximizeButton => maximizeButton;

        public SpeedButton CloseButton => closeButton;

        public ImageSizeFallbackOptions IconSizeFallbackOptions => fallbackOptions;

        public ToolBar ToolBar => toolBar;

        public PictureBox IconPictureBox => icon;

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

        protected override void OnTitleChanged(EventArgs e)
        {
            base.OnTitleChanged(e);
            label.Text = Title;
        }

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

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            AssignDefaultColors();
        }
    }
}
