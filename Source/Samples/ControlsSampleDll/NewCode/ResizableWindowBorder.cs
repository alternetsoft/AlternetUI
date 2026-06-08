using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that can be used as a resizable window border
    /// with title, icon and caption buttons.
    /// </summary>
    public partial class ResizableWindowBorder : ResizableBorder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResizableWindowBorder"/> class.
        /// </summary>
        public ResizableWindowBorder()
        {
            this.HasBorder = false;

            bool isDark = this.IsDarkBackground;
            bool isActive = true;

            this.UseWindowBorderSize(Window.FrameMetrics.BorderMetricKind.ThickFrame);
            this.UseWindowBorderColors(isDark, isActive);

            var toolBar = new ToolBar();
            toolBar.ResetSuggestedSize();
            toolBar.Dock = DockStyle.Top;
            toolBar.MinHeight = Coord.Max(Window.FrameMetrics.GetCaptionAreaHeight(App.SafeWindow), ToolBar.DefaultMinItemSize);
            toolBar.Parent = this.FillPanel;
            toolBar.ParentBackColor = false;
            toolBar.BackColor = DefaultColors.GetEffectiveWindowCaptionColor(isDark, isActive);
            toolBar.ParentForeColor = false;
            toolBar.ForeColor = DefaultColors.GetEffectiveWindowCaptionTextColor(isDark, isActive);
            toolBar.AutoUpdateColors = false;

            ImageSizeFallbackOptions fallbackOptions = new();
            fallbackOptions.AllowScaled = true;

            var icon = toolBar.AddIcon(KnownIcons.Default, fallbackOptions);
            icon.Margin = (5, 0, 0, 0);
            icon.Click += (sender, e) =>
            {
                App.Log("Icon clicked");
            };

            var gripControl = new GripControl();
            gripControl.ConfigureAsMovingGrip();
            gripControl.VerticalAlignment = VerticalAlignment.Stretch;
            gripControl.Target = this;
            gripControl.HorizontalAlignment = HorizontalAlignment.Fill;

            var label = new Label("This is title");
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Margin = (5, 0, 5, 0);
            label.InputTransparent = true;
            label.Parent = gripControl;

            toolBar.AddControl(gripControl);

            var minimizeButton = toolBar.AddSpeedBtnCore(null, KnownSvgImages.ImgWindowMinimize, "Minimize", (s, e) =>
            {
                App.Log("Minimize button clicked");
            });

            minimizeButton.HorizontalAlignment = HorizontalAlignment.Right;

            var maximizeButton = toolBar.AddSpeedBtnCore(null, KnownSvgImages.ImgWindowMaximize, "Maximize", (s, e) =>
            {
                App.Log("Maximize button clicked");
            });

            maximizeButton.HorizontalAlignment = HorizontalAlignment.Right;

            var closeButton = toolBar.AddSpeedBtnCore(null, KnownSvgImages.ImgClose, "Close", (s, e) =>
            {
                App.Log("Close button clicked");
            });

            closeButton.HorizontalAlignment = HorizontalAlignment.Right;
        }
    }
}
