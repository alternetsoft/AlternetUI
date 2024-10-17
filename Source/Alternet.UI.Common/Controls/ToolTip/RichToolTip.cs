using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to show a tool tip with more customizations than a standard tooltip.
    /// Additionally to the tooltip message <see cref="RichToolTip"/> allows to
    /// specify title, image and other options.
    /// </summary>
    public class RichToolTip : UserControl, IRichToolTip, IToolTipProvider
    {
        private IRichToolTipHandler? tooltip;

        /// <summary>
        /// Initializes a new instance of the <see cref="RichToolTip"/> class.
        /// </summary>
        public RichToolTip()
        {
        }

        /// <summary>
        /// Gets or sets default background color of the tooltip.
        /// </summary>
        public static Color? DefaultBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets default foreground color of the tooltip.
        /// </summary>
        public static Color? DefaultForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets default foreground color of the tooltip.
        /// </summary>
        public static Color? DefaultTitleForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets default background end color of the tooltip.
        /// </summary>
        public static Color? DefaultBackgroundColorEnd { get; set; }

        /// <summary>
        /// Gets or sets whether to draw point at center under the debug environment.
        /// </summary>
        public bool ShowDebugRectangleAtCenter { get; set; }

        /// <summary>
        /// Gets <see cref="IRichToolTipHandler"/> provider used to work with tooltip.
        /// </summary>
        private IRichToolTipHandler ToolTipHandler
        {
            get
            {
                tooltip ??= Create();
                return tooltip;

                IRichToolTipHandler Create()
                {
                    var tooltip = ToolTipFactory.Handler.CreateRichToolTipHandler(
                        Title ?? string.Empty,
                        Text ?? string.Empty,
                        true);

                    var platform = AllPlatformDefaults.PlatformCurrent;

                    var backgroundColor
                        = DefaultBackgroundColor ?? platform.RichToolTipBackgroundColor;
                    var backgroundColorEnd
                        = DefaultBackgroundColorEnd ?? platform.RichToolTipBackgroundColorEnd;
                    var foregroundColor = DefaultForegroundColor ?? platform.RichToolTipForegroundColor;
                    var titleForegroundColor = DefaultTitleForegroundColor
                        ?? platform.RichToolTipTitleForegroundColor;

                    if (backgroundColor is not null)
                        SetBackgroundColor(backgroundColor, backgroundColorEnd);
                    if (foregroundColor is not null)
                        SetForegroundColor(foregroundColor);
                    if (titleForegroundColor is not null)
                        SetTitleForegroundColor(titleForegroundColor);
                    tooltip.SetTipKind(RichToolTipKind.None);
                    return tooltip;
                }
            }
        }

        /// <summary>
        /// Sets simple tooltip contents.
        /// </summary>
        /// <param name="message">Tooltip message.</param>
        /// <param name="systemColors">If <c>true</c>, sets tooltip colors
        /// to <see cref="FontAndColor.SystemColorInfo"/>.</param>
        public virtual IRichToolTip SetToolTip(object message, bool systemColors = true)
        {
            HideToolTip();
            Text = message?.ToString() ?? string.Empty;
            Title = string.Empty;

            if (systemColors)
            {
                var colors = FontAndColor.SystemColorInfo;
                SetBackgroundColor(colors.BackgroundColor);
                SetForegroundColor(colors.ForegroundColor);
            }

            return this;
        }

        /// <summary>
        /// Hides tooltip.
        /// </summary>
        public new virtual IRichToolTip HideToolTip()
        {
            try
            {
                SafeDisposeObject(ref tooltip);
                return this;
            }
            catch
            {
                return this;
            }
        }

        /// <summary>
        /// Sets tooltip contents.
        /// </summary>
        /// <param name="title">Tooltip title.</param>
        /// <param name="message">Tooltip message.</param>
        /// <param name="icon">Tooltip standard icon.</param>
        /// <param name="timeoutMilliseconds">
        /// Timeout in milliseconds after which tooltip will be hidden. Optional. If not specified,
        /// default timeout value is used. If 0 is specified, tooltip will not be hidden after timeout.
        /// </param>
        public virtual IRichToolTip SetToolTip(
            object? title,
            object? message,
            MessageBoxIcon? icon = null,
            uint? timeoutMilliseconds = null)
        {
            HideToolTip();
            TitleAsObject = title ?? string.Empty;
            Text = message?.ToString() ?? string.Empty;

            if (icon is not null)
            {
                if (string.IsNullOrEmpty(Title))
                    icon = MessageBoxIcon.None;
                SetIcon(icon.Value);
            }

            if(timeoutMilliseconds is not null)
            {
                SetTimeout(timeoutMilliseconds.Value);
            }

            return this;
        }

        /// <summary>
        /// Sets the background color: if two colors are specified, the background
        /// is drawn using a gradient from top to bottom, otherwise a single solid
        /// color is used.
        /// </summary>
        /// <param name="color">Background color.</param>
        /// <param name="endColor">Second background color.</param>
        public virtual IRichToolTip SetBackgroundColor(Color? color, Color? endColor = null)
        {
            if (color is null)
                return this;

            Color color2;
            if (endColor is null)
                color2 = Color.Empty;
            else
                color2 = endColor;
            ToolTipHandler.SetBackgroundColor(color, color2);

            return this;
        }

        /// <summary>
        /// Sets foreground color of the tooltip message.
        /// </summary>
        /// <param name="color">Foreground color of the message.</param>
        public virtual IRichToolTip SetForegroundColor(Color? color)
        {
            if (color is null)
                return this;
            ToolTipHandler.SetForegroundColor(color);
            return this;
        }

        /// <summary>
        /// Sets foreground color of the tooltip title.
        /// </summary>
        /// <param name="color">Foreground color of the title.</param>
        public virtual IRichToolTip SetTitleForegroundColor(Color? color)
        {
            if (color is null)
                return this;
            ToolTipHandler.SetTitleForegroundColor(color);
            return this;
        }

        /// <summary>
        /// Sets timeout after which the tooltip should disappear, in milliseconds.
        /// If 0 is specified, tooltip will not be hidden automatically.
        /// Optionally specify a show delay.
        /// </summary>
        /// <remarks>
        /// By default the tooltip is hidden after system-dependent interval of time
        /// elapses but this method can be used to change this or also disable
        /// hiding the tooltip automatically entirely by passing 0 in this parameter
        /// (but doing this can result in native version not being used).
        /// </remarks>
        /// <param name="milliseconds">Timeout value.</param>
        /// <param name="millisecondsShowdelay">Show delay value.</param>
        public virtual IRichToolTip SetTimeout(uint milliseconds, uint millisecondsShowdelay = 0)
        {
            ToolTipHandler.SetTimeout(milliseconds, millisecondsShowdelay);
            return this;
        }

        /// <summary>
        /// Sets the small icon to show in the tooltip.
        /// </summary>
        /// <param name="bitmap">Icon of the tooltip.</param>
        public virtual IRichToolTip SetIcon(ImageSet? bitmap)
        {
            ToolTipHandler.SetIcon(bitmap);
            return this;
        }

        /// <summary>
        /// Sets the title text font.
        /// </summary>
        /// <remarks>
        /// By default it's emphasized using the font style
        /// or colour appropriate for the current platform.
        /// </remarks>
        /// <param name="font">Font of the title.</param>
        public virtual IRichToolTip SetTitleFont(Font? font)
        {
            ToolTipHandler.SetTitleFont(font);
            return this;
        }

        /// <summary>
        /// Sets tooltip with contents filled from the template data.
        /// </summary>
        /// <param name="template">Template with tooltip data.</param>
        /// <param name="backColor">Background color. Optional. If not specified, background color
        /// of the template control is used.</param>
        /// <returns></returns>
        public virtual IRichToolTip SetToolTipFromTemplate(
            TemplateControl template,
            Color? backColor = null)
        {
            HideToolTip();
            backColor ??= template.BackgroundColor;

            var image = TemplateUtils.GetTemplateAsImage(template, backColor);
            OnlyImage((ImageSet)image, backColor);
            return this;
        }

        /// <summary>
        /// Sets text to an empty string.
        /// </summary>
        /// <returns></returns>
        public virtual IRichToolTip ResetText()
        {
            Text = string.Empty;
            return this;
        }

        /// <summary>
        /// Sets title to an empty string.
        /// </summary>
        /// <returns></returns>
        public virtual IRichToolTip ResetTitle()
        {
            Title = string.Empty;
            return this;
        }

        /// <summary>
        /// Sets tooltip to show only the specified image.
        /// </summary>
        /// <param name="image">Image to show.</param>
        /// <param name="backColor">Background color of the tooltip.</param>
        /// <returns></returns>
        public virtual IRichToolTip OnlyImage(ImageSet? image, Color? backColor = null)
        {
            HideToolTip().ResetTitle().ResetText();

            if(backColor is not null)
            {
                SetBackgroundColor(backColor);
            }

            SetIcon(image);
            return this;
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return base.GetPreferredSize(availableSize);
        }

        /// <summary>
        /// Shows the tooltip at the specified location.
        /// Location coordinates are in device-independent units.
        /// </summary>
        /// <param name="location">Location where tooltip will be shown.</param>
        public virtual IRichToolTip ShowToolTip(PointD? location = null)
        {
            location ??= (0, 0);
            var pxLocation = PixelFromDip(location.Value);
            var area = (pxLocation.X - 1, pxLocation.Y - 1, 2, 2);
            ToolTipHandler.Show(this, area, false);
            return this;
        }

        /// <inheritdoc/>
        public override void DefaultPaint(Graphics dc, RectD rect)
        {
            DrawDefaultBackground(dc, rect);

            if(DebugUtils.IsDebugDefined && ShowDebugRectangleAtCenter)
            {
                dc.FillRectangleAtCenter(
                    LightDarkColors.Red.AsBrush,
                    ClientRectangle,
                    3);
            }
        }

        /// <summary>
        /// Sets the standard icon to show in the tooltip.
        /// </summary>
        /// <param name="icon">One of the standard information/warning/error icons
        /// (the question icon doesn't make sense for a tooltip)</param>
        public virtual IRichToolTip SetIcon(MessageBoxIcon icon)
        {
            ToolTipHandler.SetIcon(icon);
            return this;
        }

        /// <summary>
        /// Shows simple tooltip on the screen.
        /// </summary>
        /// <param name="message">Tooltip message.</param>
        /// <param name="systemColors">If <c>true</c>, sets tooltip colors
        /// to <see cref="FontAndColor.SystemColorInfo"/>.</param>
        /// <param name="location">Location where tooltip will be shown.</param>
        public IRichToolTip ShowToolTip(
            object message,
            bool systemColors = true,
            PointD? location = null)
        {
            return SetToolTip(message, systemColors).ShowToolTip(location);
        }

        /// <summary>
        /// Shows tooltip on the screen.
        /// </summary>
        /// <param name="title">Tooltip title.</param>
        /// <param name="message">Tooltip message.</param>
        /// <param name="icon">Tooltip standard icon.</param>
        /// <param name="timeoutMilliseconds">
        /// Timeout in milliseconds after which tooltip will be hidden. Optional. If not specified,
        /// default timeout value is used. If 0 is specified, tooltip will not be hidden after timeout.
        /// </param>
        /// <param name="location">Location where tooltip will be shown.</param>
        public virtual IRichToolTip ShowToolTip(
            object? title,
            object? message,
            MessageBoxIcon? icon = null,
            uint? timeoutMilliseconds = null,
            PointD? location = null)
        {
            return SetToolTip(title, message, icon, timeoutMilliseconds).ShowToolTip(location);
        }

        /// <summary>
        /// Shows tooltip with contents filled from the template data.
        /// </summary>
        /// <param name="template">Template with tooltip data.</param>
        /// <param name="backColor">Background color. Optional. If not specified, background color
        /// of the template control is used.</param>
        /// <returns></returns>
        /// <param name="location">Location where tooltip will be shown.</param>
        public virtual IRichToolTip ShowToolTipFromTemplate(
            TemplateControl template,
            Color? backColor = null,
            PointD? location = null)
        {
            return SetToolTipFromTemplate(template, backColor).ShowToolTip(location);
        }

        IRichToolTip? IToolTipProvider.Get(object? sender)
        {
            return this;
        }

        /// <inheritdoc/>
        protected override void OnTitleChanged(EventArgs e)
        {
            base.OnTitleChanged(e);
            HideToolTip();
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            HideToolTip();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            HideToolTip();
        }
    }
}