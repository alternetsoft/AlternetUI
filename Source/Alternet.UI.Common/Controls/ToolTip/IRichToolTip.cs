using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to show, hide and customize tooltip.
    /// </summary>
    public interface IRichToolTip
    {
        /// <summary>
        /// Gets foreground color of the tooltip title.
        /// </summary>
        Color? ToolTipTitleForegroundColor { get; set; }

        /// <summary>
        /// Gets font of the tooltip title.
        /// </summary>
        Font? ToolTipTitleFont { get; set; }

        /// <summary>
        /// Gets tooltip show timeout in milliseconds. If null,
        /// <see cref="TimerUtils.DefaultToolTipTimeout"/> is used.
        /// </summary>
        int? TimeoutInMilliseconds { get; set; }

        /// <summary>
        /// Gets tooltip show delay in milliseconds.
        /// </summary>
        int ShowDelayInMilliseconds { get; set; }

        /// <summary>
        /// Gets Tooltip image.
        /// </summary>
        ImageSet? ToolTipImage { get; set; }

        /// <summary>
        /// Gets tooltip background color.
        /// </summary>
        public Color? ToolTipBackgroundColor { get; set; }

        /// <summary>
        /// Gets tooltip foreground color.
        /// </summary>
        public Color? ToolTipForegroundColor { get; set; }

        /// <summary>
        /// Gets tooltip border.
        /// </summary>
        public BorderSettings? ToolTipBorder { get; set; }

        /// <summary>
        /// Gets Tooltip icon.
        /// </summary>
        MessageBoxIcon? ToolTipIcon { get; set; }

        /// <summary>
        /// Gets tooltip background brush.
        /// </summary>
        Brush? ToolTipBackgroundBrush { get; set; }

        /// <summary>
        /// Gets <see cref="RichToolTip"/> which is used as
        /// provider for <see cref="IRichToolTip"/>. This property may return null as some other control
        /// may be used as a provider. In this case use <see cref="AbstractToolTipControl"/>.
        /// </summary>
        RichToolTip? ToolTipControl { get; }

        /// <summary>
        /// Gets <see cref="AbstractControl"/> which is used as
        /// provider for <see cref="IRichToolTip"/>. This property may return null as interface provider
        /// may not ne derived from <see cref="AbstractControl"/>.
        /// </summary>
        AbstractControl? AbstractToolTipControl { get; }

        /// <summary>
        /// Sets tooltip properties so it will be a simple tooltip.
        /// </summary>
        /// <param name="message">Tooltip message. Text in this parameter can have line breaks.</param>
        /// <param name="systemColors">Whether to use system colors for the
        /// foreground and background.</param>
        /// <returns></returns>
        IRichToolTip SetToolTip(object message, bool systemColors = true);

        /// <summary>
        /// Shows simple tooltip at the specified location in client coordinates
        /// of the tooltip container.
        /// </summary>
        /// <param name="message">Tooltip message. Text in this parameter can have line breaks.</param>
        /// <param name="systemColors">Whether to use system colors for the
        /// foreground and background.</param>
        /// <returns></returns>
        /// <param name="location">Location of the tooltip.</param>
        /// <returns></returns>
        IRichToolTip ShowToolTip(
            object message,
            bool systemColors = true,
            PointD? location = null);

        /// <summary>
        /// Hides tooltip.
        /// </summary>
        /// <returns></returns>
        IRichToolTip HideToolTip();

        /// <summary>
        /// Sets tooltip properties.
        /// </summary>
        /// <param name="title">Tooltip title. If null is specified, tooltip will have no title.</param>
        /// <param name="message">ToolTip message. If null is specified,
        /// tooltip will have no message. Text in this parameter can have line breaks.</param>
        /// <param name="icon"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// <returns></returns>
        IRichToolTip SetToolTip(
            object? title,
            object? message,
            MessageBoxIcon? icon = null,
            uint? timeoutMilliseconds = null);

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
        IRichToolTip ShowToolTip(
            object? title,
            object? message,
            MessageBoxIcon? icon = null,
            uint? timeoutMilliseconds = null,
            PointD? location = null);

        /// <summary>
        /// Sets tool tip background color.
        /// </summary>
        /// <param name="color">Background color.</param>
        IRichToolTip SetToolTipBackgroundColor(Color? color);

        /// <summary>
        /// Sets the background brush.
        /// </summary>
        /// <param name="brush">Background brush.</param>
        IRichToolTip SetToolTipBackgroundBrush(Brush? brush);

        /// <summary>
        /// Sets foreground color of the message text.
        /// </summary>
        /// <param name="color">The foreground color of the message text. If null is specified,
        /// default value is used.</param>
        /// <returns></returns>
        IRichToolTip SetToolTipForegroundColor(Color? color);

        /// <summary>
        /// Sets foreground color of the title.
        /// </summary>
        /// <param name="color">The foreground color of the title. If null is specified,
        /// default value is used.</param>
        /// <returns></returns>
        IRichToolTip SetTitleForegroundColor(Color? color);

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
        /// <param name="milliseconds">Timeout value. If null,
        /// <see cref="TimerUtils.DefaultToolTipTimeout"/> will be used.</param>
        /// <param name="millisecondsShowdelay">Show delay value. Optional. Default is 0.</param>
        IRichToolTip SetTimeout(uint? milliseconds, uint millisecondsShowdelay = 0);

        /// <summary>
        /// Sets tooltip image using <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="bitmap">The tooltip image specified
        /// using <see cref="ImageSet"/>.</param>
        /// <returns></returns>
        IRichToolTip SetIcon(ImageSet? bitmap);

        /// <summary>
        /// Sets font of the title.
        /// </summary>
        /// <param name="font">Title font.</param>
        /// <returns></returns>
        IRichToolTip SetTitleFont(Font? font);

        /// <summary>
        /// Initializes tooltip to show image built using the specified control template.
        /// </summary>
        /// <param name="template">Control template.</param>
        /// <param name="backColor">Background color. Optional.</param>
        /// <returns></returns>
        IRichToolTip SetToolTipFromTemplate(
            TemplateControl template,
            Color? backColor = null);

        /// <summary>
        /// Shows tooltip with the image built using the specified control template
        /// Tooltip is shown at the specified location specified in the client coordinates
        /// of the tooltip container.
        /// </summary>
        /// <param name="template">Control template.</param>
        /// <param name="location">Location of the tooltip.</param>
        /// <param name="backColor">Background color. Optional.</param>
        /// <returns></returns>
        IRichToolTip ShowToolTipFromTemplate(
            TemplateControl template,
            Color? backColor = null,
            PointD? location = null);

        /// <summary>
        /// Clears text of the tooltip.
        /// </summary>
        /// <returns></returns>
        IRichToolTip ResetText();

        /// <summary>
        /// Clears title of the tooltip.
        /// </summary>
        /// <returns></returns>
        IRichToolTip ResetTitle();

        /// <summary>
        /// Configures tooltip to show only the specified image.
        /// </summary>
        /// <param name="image">Tooltip image.</param>
        /// <returns></returns>
        IRichToolTip OnlyImage(ImageSet? image);

        /// <summary>
        /// Shows previously configured tooltip at the specified location in client coordinates
        /// of the tooltip container.
        /// </summary>
        /// <param name="location">Location of the tooltip. Optional.</param>
        /// <returns></returns>
        IRichToolTip ShowToolTip(PointD? location = null);

        /// <summary>
        /// Sets tooltip image using <see cref="MessageBoxIcon"/> enum.
        /// </summary>
        /// <param name="icon">The tooltip image specified
        /// using <see cref="MessageBoxIcon"/> enum.</param>
        /// <returns></returns>
        IRichToolTip SetIcon(MessageBoxIcon? icon);
    }
}