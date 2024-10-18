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
        Color? TitleForegroundColor { get; }

        /// <summary>
        /// Gets font of the tooltip title.
        /// </summary>
        Font? TitleFont { get; }

        /// <summary>
        /// Gets tooltip show timeout in milliseconds.
        /// </summary>
        int TimeoutInMilliseconds { get; }

        /// <summary>
        /// Gets tooltip show delay in milliseconds.
        /// </summary>
        int ShowDelayInMilliseconds { get; }

        /// <summary>
        /// Gets Tooltip image.
        /// </summary>
        ImageSet? ToolTipImage { get; }

        /// <summary>
        /// Gets Tooltip icon.
        /// </summary>
        MessageBoxIcon? ToolTipIcon { get; }

        /// <summary>
        /// Gets end color of the tooltip background for the linear gradient painting.
        /// </summary>
        Color? BackgroundEndColor { get; }

        /// <summary>
        /// Gets end color of the tooltip background for the linear gradient painting.
        /// </summary>
        Color? BackgroundStartColor { get; }

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
        /// Sets the background color: if two colors are specified, the background
        /// is painted using a gradient from top to bottom, otherwise a single solid
        /// color is used.
        /// </summary>
        /// <param name="color">Background color.</param>
        /// <param name="endColor">Second background color.</param>
        IRichToolTip SetBackgroundColor(Color? color, Color? endColor = null);

        /// <summary>
        /// Sets foreground color of the message text.
        /// </summary>
        /// <param name="color">The foreground color of the message text. If null is specified,
        /// default value is used.</param>
        /// <returns></returns>
        IRichToolTip SetForegroundColor(Color? color);

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
        /// <param name="milliseconds">Timeout value.</param>
        /// <param name="millisecondsShowdelay">Show delay value.</param>
        IRichToolTip SetTimeout(uint milliseconds, uint millisecondsShowdelay = 0);

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
        /// <param name="backColor">Background color of the tooltip. Optional.</param>
        /// <returns></returns>
        IRichToolTip OnlyImage(
            ImageSet? image,
            Color? backColor);

        /// <summary>
        /// Shows previously configured tooltip at the specified location in client coordinates
        /// of the tooltip container.
        /// </summary>
        /// <param name="location">Location of the tooltip.</param>
        /// <returns></returns>
        IRichToolTip ShowToolTip(PointD? location = null);

        /// <summary>
        /// Sets tooltip image using <see cref="MessageBoxIcon"/> enum.
        /// </summary>
        /// <param name="icon">The tooltip image specified
        /// using <see cref="MessageBoxIcon"/> enum.</param>
        /// <returns></returns>
        IRichToolTip SetIcon(MessageBoxIcon icon);
    }
}