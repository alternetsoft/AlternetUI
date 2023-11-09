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
    /// specify title, image, tip kind and some other options.
    /// </summary>
    public class RichToolTip : DisposableObject, IRichToolTip
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RichToolTip"/> class.
        /// </summary>
        /// <param name="title">Tooltip title.</param>
        /// <param name="message">Tooltip message.</param>
        public RichToolTip(string? title, string? message)
            : base(
                  Native.WxOtherFactory.CreateRichToolTip(
                    title ?? string.Empty,
                    message ?? string.Empty),
                  true)
        {
            var platform = AllPlatformDefaults.PlatformCurrent;

            var backgroundColor = DefaultBackgroundColor ?? platform.RichToolTipBackgroundColor;
            var backgroundColorEnd = DefaultBackgroundColorEnd ?? platform.RichToolTipBackgroundColorEnd;
            var foregroundColor = DefaultForegroundColor ?? platform.RichToolTipForegroundColor;
            var titleForegroundColor = DefaultTitleForegroundColor ?? platform.RichToolTipTitleForegroundColor;

            if (backgroundColor is not null)
                SetBackgroundColor(backgroundColor.Value, backgroundColorEnd);
            if (foregroundColor is not null)
                SetForegroundColor(foregroundColor.Value);
            if (titleForegroundColor is not null)
                SetTitleForegroundColor(titleForegroundColor.Value);
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
                return Native.WxOtherFactory.RichToolTipUseGeneric;
            }

            set
            {
                Native.WxOtherFactory.RichToolTipUseGeneric = value;
            }
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
        /// Gets or sets default <see cref="RichToolTip"/>.
        /// </summary>
        public static RichToolTip? Default { get; set; }

        /// <summary>
        /// Shows tooltip on the screen.
        /// </summary>
        /// <param name="title">Tooltip title.</param>
        /// <param name="message">Tooltip message.</param>
        /// <param name="icon">Tooltip standard icon.</param>
        /// <param name="control">Control for which tooltip is shown.</param>
        /// <param name="kind">Tooltip kind.</param>
        public static void Show(
            string? title,
            string? message,
            Control control,
            RichToolTipKind? kind = null,
            MessageBoxIcon? icon = null)
        {
            Default = new(title, message);
            if (kind is not null)
                Default.SetTipKind(kind.Value);
            if (icon is not null)
                Default.SetIcon(icon.Value);
            Default.Show(control);
        }

        /// <summary>
        /// Sets the background color: if two colors are specified, the background
        /// is drawn using a gradient from top to bottom, otherwise a single solid
        /// color is used.
        /// </summary>
        /// <param name="color">Background color.</param>
        /// <param name="endColor">Second background color.</param>
        public void SetBackgroundColor(Color color, Color? endColor)
        {
            Color color2;
            if (endColor is null)
                color2 = Color.Empty;
            else
                color2 = endColor.Value;
            Native.WxOtherFactory.RichToolTipSetBkColor(Handle, color, color2);
        }

        /// <summary>
        /// Sets foreground color of the tooltip message.
        /// </summary>
        /// <param name="color">Foreground color of the message.</param>
        /// <remarks>
        /// This is implemnented only for generic tooltips (when <see cref="UseGeneric"/> is true).
        /// </remarks>
        public void SetForegroundColor(Color color)
        {
            Native.WxOtherFactory.RichToolTipSetFgColor(Handle, color);
        }

        /// <summary>
        /// Sets foreground color of the tooltip title.
        /// </summary>
        /// <param name="color">Foreground color of the title.</param>
        /// <remarks>
        /// This is implemnented only for generic tooltips (when <see cref="UseGeneric"/> is true).
        /// </remarks>
        public void SetTitleForegroundColor(Color color)
        {
            Native.WxOtherFactory.RichToolTipSetTitleFgColor(Handle, color);
        }

        /// <summary>
        /// Sets timeout after which the tooltip should disappear, in milliseconds.
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
        public void SetTimeout(uint milliseconds, uint millisecondsShowdelay = 0)
        {
            Native.WxOtherFactory.RichToolTipSetTimeout(Handle, milliseconds, millisecondsShowdelay);
        }

        /// <summary>
        /// Sets the small icon to show in the tooltip.
        /// </summary>
        /// <param name="bitmap">Icon of the tooltip.</param>
        public void SetIcon(ImageSet? bitmap)
        {
            Native.WxOtherFactory.RichToolTipSetIcon(Handle, bitmap?.NativeImageSet);
        }

        /// <summary>
        /// Sets the title text font.
        /// </summary>
        /// <remarks>
        /// By default it's emphasized using the font style
        /// or colour appropriate for the current platform.
        /// </remarks>
        /// <param name="font">Font of the title.</param>
        public void SetTitleFont(Font? font)
        {
            Native.WxOtherFactory.RichToolTipSetTitleFont(Handle, font?.NativeFont);
        }

        /// <summary>
        /// Chooses the tip kind, possibly none. By default the tip is positioned
        /// automatically, as if <see cref="RichToolTipKind.Auto"/> was used.
        /// </summary>
        /// <param name="tipKind">Tip kind.</param>
        public void SetTipKind(RichToolTipKind tipKind)
        {
            Native.WxOtherFactory.RichToolTipSetTipKind(Handle, (int)tipKind);
        }

        /// <summary>
        /// Shows the tooltip for the given control and optionally a specified area.
        /// </summary>
        /// <param name="control">Control for which tooltip is shown.</param>
        /// <param name="rect">Area of the tooltip.</param>
        public void Show(Control control, Int32Rect? rect = null)
        {
            if (rect is null)
                Native.WxOtherFactory.RichToolTipShowFor(Handle, control.WxWidget, Int32Rect.Empty);
            else
                Native.WxOtherFactory.RichToolTipShowFor(Handle, control.WxWidget, rect.Value);
        }

        /// <summary>
        /// Sets the standard icon to show in the tooltip.
        /// </summary>
        /// <param name="icon">One of the standard information/warning/error icons
        /// (the question icon doesn't make sense for a tooltip)</param>
        public void SetIcon(MessageBoxIcon icon)
        {
            const int wxICON_WARNING = 0x00000100;
            const int wxICON_ERROR = 0x00000200;
            const int wxICON_INFORMATION = 0x00000800;
            const int wxICON_NONE = 0x00040000;

            int style;

            switch (icon)
            {
                default:
                case MessageBoxIcon.None:
                    style = wxICON_NONE;
                    break;
                case MessageBoxIcon.Information:
                    style = wxICON_INFORMATION;
                    break;
                case MessageBoxIcon.Warning:
                    style = wxICON_WARNING;
                    break;
                case MessageBoxIcon.Error:
                    style = wxICON_ERROR;
                    break;
            }

            Native.WxOtherFactory.RichToolTipSetIcon2(Handle, style);
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanagedResources()
        {
            Native.WxOtherFactory.DeleteRichToolTip(Handle);
        }
    }
}