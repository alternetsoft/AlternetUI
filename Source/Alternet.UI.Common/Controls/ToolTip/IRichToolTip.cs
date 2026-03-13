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
        /// Gets or sets the maximum width, in device-independent units (DIPs), that text can occupy before it is
        /// truncated or wrapped.
        /// </summary>
        /// <remarks>Set this property to limit the width of displayed title and message text. If the value is null, 
        /// <see cref="RichToolTip.DefaultMaxWidth"/> is used.</remarks>
        float? MaxTextWidth { get; set; }

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
        Color? ToolTipBackgroundColor { get; set; }

        /// <summary>
        /// Gets tooltip foreground color.
        /// </summary>
        Color? ToolTipForegroundColor { get; set; }

        /// <summary>
        /// Gets tooltip border.
        /// </summary>
        BorderSettings? ToolTipBorder { get; set; }

        /// <summary>
        /// Gets tooltip location in client coordinates of the tooltip owner. If null, tooltip is shown at the default location.
        /// </summary>
        PointD? ToolTipLocation { get; set; }

        /// <summary>
        /// Gets or sets the owner of the tooltip. This can be any object, but if it is a <see cref="AbstractControl"/>,
        /// the tooltip location is relative to it.
        /// </summary>
        object? ToolTipOwner { get; set; }

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
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip SetToolTip(object message, bool systemColors = true);

        /// <summary>
        /// Shows simple tooltip at the specified location in client coordinates
        /// of the tooltip container.
        /// </summary>
        /// <param name="message">Tooltip message. Text in this parameter can have line breaks.</param>
        /// <param name="systemColors">Whether to use system colors for the
        /// foreground and background.</param>
        /// <param name="location">Location of the tooltip.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip ShowToolTip(
            object message,
            bool systemColors = true,
            PointD? location = null);

        /// <summary>
        /// Hides tooltip.
        /// </summary>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip HideToolTip();

        /// <summary>
        /// Sets tooltip properties.
        /// </summary>
        /// <param name="title">Tooltip title. If null is specified, tooltip will have no title.</param>
        /// <param name="message">ToolTip message. If null is specified,
        /// tooltip will have no message. Text in this parameter can have line breaks.</param>
        /// <param name="icon">Tooltip icon. If null is specified, no icon will be shown.</param>
        /// <param name="timeoutMilliseconds">Timeout in milliseconds after which tooltip will be hidden.
        /// Optional. If not specified, default timeout value is used.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
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
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
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
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip SetToolTipBackgroundColor(Color? color);

        /// <summary>
        /// Sets the background brush.
        /// </summary>
        /// <param name="brush">Background brush.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip SetToolTipBackgroundBrush(Brush? brush);

        /// <summary>
        /// Sets foreground color of the message text.
        /// </summary>
        /// <param name="color">The foreground color of the message text. If null is specified,
        /// default value is used.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip SetToolTipForegroundColor(Color? color);

        /// <summary>
        /// Sets foreground color of the title.
        /// </summary>
        /// <param name="color">The foreground color of the title. If null is specified,
        /// default value is used.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
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
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip SetTimeout(uint? milliseconds, uint millisecondsShowdelay = 0);

        /// <summary>
        /// Sets tooltip image using <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="bitmap">The tooltip image specified
        /// using <see cref="ImageSet"/>.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip SetIcon(ImageSet? bitmap);

        /// <summary>
        /// Sets font of the title.
        /// </summary>
        /// <param name="font">Title font.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip SetTitleFont(Font? font);

        /// <summary>
        /// Initializes tooltip to show image built using the specified control template.
        /// </summary>
        /// <param name="template">Control template.</param>
        /// <param name="backColor">Background color. Optional.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
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
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip ShowToolTipFromTemplate(
            TemplateControl template,
            Color? backColor = null,
            PointD? location = null);

        /// <summary>
        /// Clears text of the tooltip.
        /// </summary>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip ResetText();

        /// <summary>
        /// Configures the tooltip with the specified parameters.
        /// </summary>
        /// <param name="prm">An object containing the parameters to apply to the tooltip. Cannot be null.</param>
        /// <returns>The current instance of the tooltip, allowing for method chaining.</returns>
        IRichToolTip SetParams(RichToolTipParams prm);

        /// <summary>
        /// Clears title of the tooltip.
        /// </summary>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip ResetTitle();

        /// <summary>
        /// Configures tooltip to show only the specified image.
        /// </summary>
        /// <param name="image">Tooltip image.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip OnlyImage(ImageSet? image);

        /// <summary>
        /// Shows previously configured tooltip at the specified location in client coordinates
        /// of the tooltip container.
        /// </summary>
        /// <param name="location">Location of the tooltip. Optional.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip ShowToolTip(PointD? location = null);

        /// <summary>
        /// Posts an show tooltip action to the application message loop.
        /// When this method is called, the tooltip will be shown after the current event is processed.
        /// The tooltip will be shown at the specified screen location or at a default position if no location is provided.
        /// </summary>
        /// <param name="location">The screen coordinates where the tooltip should be displayed. If null, the tooltip is shown at a default
        /// location determined by the implementation.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip PostShowToolTip(PointD? location = null);

        /// <summary>
        /// Sets tooltip image using <see cref="MessageBoxIcon"/> enum.
        /// </summary>
        /// <param name="icon">The tooltip image specified
        /// using <see cref="MessageBoxIcon"/> enum.</param>
        /// <returns>Returns this <see cref="IRichToolTip"/> object instance for use in the call sequences.</returns>
        IRichToolTip SetIcon(MessageBoxIcon? icon);
    }
}