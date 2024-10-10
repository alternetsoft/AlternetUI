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
    /// specify title, image, tip kind and other options.
    /// </summary>
    public class RichToolTip : BaseComponent
    {
        private static bool useGeneric = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="RichToolTip"/> class
        /// with empty title and message text.
        /// </summary>
        public RichToolTip()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RichToolTip"/> class
        /// with the specified title and message text.
        /// </summary>
        /// <param name="title">Tooltip title.</param>
        /// <param name="message">Tooltip message.</param>
        public RichToolTip(string? title, string? message)
        {
            Handler = ToolTipFactory.Handler.CreateRichToolTipHandler(
                title ?? string.Empty,
                message ?? string.Empty,
                useGeneric);

            var platform = AllPlatformDefaults.PlatformCurrent;

            var backgroundColor = DefaultBackgroundColor ?? platform.RichToolTipBackgroundColor;
            var backgroundColorEnd = DefaultBackgroundColorEnd ?? platform.RichToolTipBackgroundColorEnd;
            var foregroundColor = DefaultForegroundColor ?? platform.RichToolTipForegroundColor;
            var titleForegroundColor = DefaultTitleForegroundColor
                ?? platform.RichToolTipTitleForegroundColor;

            if (backgroundColor is not null)
                SetBackgroundColor(backgroundColor, backgroundColorEnd);
            if (foregroundColor is not null)
                SetForegroundColor(foregroundColor);
            if (titleForegroundColor is not null)
                SetTitleForegroundColor(titleForegroundColor);

            IgnoreImages = DefaultIgnoreImages;
        }

        /// <summary>
        /// Gets or sets whether to use generic or native control as a <see cref="RichToolTip"/>.
        /// </summary>
        /// <remarks>
        /// When generic control is used, it is possible to specify foreground color
        /// and foreground title color. In the native control it is not implemented.
        /// We suggest to use generic version under Linux as native version have bad colors
        /// on Ubuntu so message is not visible. Default is true.
        /// </remarks>
        public static bool UseGeneric
        {
            get
            {
                return useGeneric;
            }

            set
            {
                useGeneric = value;
            }
        }

        /// <summary>
        /// Gets or sets default value for the <see cref="IgnoreImages"/> property.
        /// </summary>
        public static bool DefaultIgnoreImages { get; set; } = false;

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
        /// Gets or sets default <see cref="RichToolTip"/>.
        /// </summary>
        public static RichToolTip? Default { get; set; }

        /// <summary>
        /// Gets <see cref="IRichToolTipHandler"/> provider used to work with tooltip.
        /// </summary>
        public IRichToolTipHandler Handler
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets whether to ignore image in tooltip, even if it is specified.
        /// </summary>
        public virtual bool IgnoreImages { get; set; }

        /// <summary>
        /// Shows simple tooltip on the screen.
        /// </summary>
        /// <param name="message">Tooltip message.</param>
        /// <param name="control">Control for which tooltip is shown.</param>
        /// <param name="useSystemColorInfo">If <c>true</c>, sets tooltip colors
        /// to <see cref="FontAndColor.SystemColorInfo"/>.</param>
        public static void ShowSimple(string message, Control control, bool useSystemColorInfo = true)
        {
            RichToolTip toolTip = new(null, message);
            RichToolTip.Default = toolTip;
            toolTip.SetTipKind(RichToolTipKind.None);
            if (useSystemColorInfo)
            {
                var colors = FontAndColor.SystemColorInfo;
                toolTip.SetBackgroundColor(colors.BackgroundColor);
                toolTip.SetForegroundColor(colors.ForegroundColor);
            }

            toolTip.Show(control);
        }

        /// <summary>
        /// Hides tooltip assigned to the <see cref="Default"/> property.
        /// </summary>
        public static void HideDefault()
        {
            try
            {
                var toolTip = RichToolTip.Default;
                RichToolTip.Default = null;
                toolTip?.Dispose();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Shows tooltip on the screen.
        /// </summary>
        /// <param name="title">Tooltip title.</param>
        /// <param name="message">Tooltip message.</param>
        /// <param name="icon">Tooltip standard icon.</param>
        /// <param name="control">Control for which tooltip is shown.</param>
        /// <param name="kind">Tooltip kind.</param>
        /// <param name="timeoutMilliseconds">
        /// Timeout in milliseconds after which tooltip will be hidden. Optional. If not specified,
        /// default timeout value is used. If 0 is specified, tooltip will not be hidden after timeout.
        /// </param>
        /// <param name="adjustPos">Whether to adjust position depending on the tip kind.
        /// Optional. Default is <c>true</c>.</param>
        public static void Show(
            string? title,
            string? message,
            Control control,
            RichToolTipKind? kind = null,
            MessageBoxIcon? icon = null,
            uint? timeoutMilliseconds = null,
            bool adjustPos = true)
        {
            Default = new(title, message);
            if (kind is not null)
            {
                Default.SetTipKind(kind.Value);
            }

            if (icon is not null)
            {
                if (string.IsNullOrEmpty(title))
                    icon = MessageBoxIcon.None;
                Default.SetIcon(icon.Value);
            }

            if(timeoutMilliseconds is not null)
            {
                Default.SetTimeout(timeoutMilliseconds.Value);
            }

            Default.Show(control, null, adjustPos);
        }

        /// <summary>
        /// Sets the background color: if two colors are specified, the background
        /// is drawn using a gradient from top to bottom, otherwise a single solid
        /// color is used.
        /// </summary>
        /// <param name="color">Background color.</param>
        /// <param name="endColor">Second background color.</param>
        public virtual void SetBackgroundColor(Color? color, Color? endColor = null)
        {
            if (color is null)
                return;

            Color color2;
            if (endColor is null)
                color2 = Color.Empty;
            else
                color2 = endColor;
            Handler.SetBackgroundColor(color, color2);
        }

        /// <summary>
        /// Sets foreground color of the tooltip message.
        /// </summary>
        /// <param name="color">Foreground color of the message.</param>
        /// <remarks>
        /// This is implemnented only for generic tooltips (when <see cref="UseGeneric"/> is true).
        /// </remarks>
        public virtual void SetForegroundColor(Color? color)
        {
            if (color is null)
                return;
            Handler.SetForegroundColor(color);
        }

        /// <summary>
        /// Sets foreground color of the tooltip title.
        /// </summary>
        /// <param name="color">Foreground color of the title.</param>
        /// <remarks>
        /// This is implemnented only for generic tooltips (when <see cref="UseGeneric"/> is true).
        /// </remarks>
        public virtual void SetTitleForegroundColor(Color? color)
        {
            if (color is null)
                return;
            Handler.SetTitleForegroundColor(color);
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
        public virtual void SetTimeout(uint milliseconds, uint millisecondsShowdelay = 0)
        {
            Handler.SetTimeout(milliseconds, millisecondsShowdelay);
        }

        /// <summary>
        /// Sets the small icon to show in the tooltip.
        /// </summary>
        /// <param name="bitmap">Icon of the tooltip.</param>
        public virtual void SetIcon(ImageSet? bitmap)
        {
            Handler.SetIcon(bitmap);
        }

        /// <summary>
        /// Sets the title text font.
        /// </summary>
        /// <remarks>
        /// By default it's emphasized using the font style
        /// or colour appropriate for the current platform.
        /// </remarks>
        /// <param name="font">Font of the title.</param>
        public virtual void SetTitleFont(Font? font)
        {
            Handler.SetTitleFont(font);
        }

        /// <summary>
        /// Chooses the tip kind, possibly none. By default the tip is positioned
        /// automatically, as if <see cref="RichToolTipKind.Auto"/> was used.
        /// </summary>
        /// <param name="tipKind">Tip kind.</param>
        public virtual void SetTipKind(RichToolTipKind tipKind)
        {
            Handler.SetTipKind(tipKind);
        }

        /// <summary>
        /// Shows the tooltip for the given control and optionally a specified area.
        /// This is lowest level method. Area coordinates are in pixels and are relative to
        /// the <paramref name="control"/> location.
        /// </summary>
        /// <param name="control">Control for which tooltip is shown.</param>
        /// <param name="rect">Area of the tooltip in pixels.
        /// Shows tooltip at the center of the rectangle. Optional. If not specified, tooltip
        /// is shown at the center of the control.</param>
        /// <param name="adjustPos">Whether to adjust position depending on the tip kind.
        /// Optional. Default is <c>true</c>.</param>
        /// <remarks>
        /// Size of the <paramref name="rect"/> affects tooltip location as (rect.width/2, rect.height/2)
        /// is added to the tooltip location.
        /// </remarks>
        public virtual void Show(Control control, RectI? rect = null, bool adjustPos = true)
        {
            Handler.Show(control, rect, adjustPos);
        }

        /// <summary>
        /// Shows the tooltip for the given control and optionally a specified location.
        /// Location coordinates are in device-independent units and are relative to
        /// the <paramref name="control"/> location.
        /// </summary>
        /// <param name="control">Control for which tooltip is shown.</param>
        /// <param name="location">Location where tooltip will be shown.</param>
        /// <param name="adjustPos">Whether to adjust position depending on the tip kind.
        /// Optional. Default is <c>true</c>.</param>
        public virtual void ShowAtLocation(Control control, PointD? location = null, bool adjustPos = true)
        {
            if(location is null)
            {
                Show(control, null, adjustPos);
            }
            else
            {
                var pxLocation = GraphicsFactory.PixelFromDip(location.Value, control.ScaleFactor);
                var area = (pxLocation.X - 1, pxLocation.Y - 1, 2, 2);
                Show(control, area, adjustPos);
            }
        }

        /// <summary>
        /// Sets the standard icon to show in the tooltip.
        /// </summary>
        /// <param name="icon">One of the standard information/warning/error icons
        /// (the question icon doesn't make sense for a tooltip)</param>
        public virtual void SetIcon(MessageBoxIcon icon)
        {
            if(IgnoreImages)
                icon = MessageBoxIcon.None;
            Handler.SetIcon(icon);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            Handler?.Dispose();
            Handler = null!;
        }
    }
}