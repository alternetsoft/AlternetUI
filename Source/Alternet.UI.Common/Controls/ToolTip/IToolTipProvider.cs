using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with tooltips.
    /// </summary>
    public interface IToolTipProvider
    {
        IToolTipProvider SetToolTip(string message, bool systemColors = true);

        IToolTipProvider HideToolTip();

        IToolTipProvider SetToolTip(
            string? title,
            string? message,
            MessageBoxIcon? icon = null,
            uint? timeoutMilliseconds = null);

        IToolTipProvider SetBackgroundColor(Color? color, Color? endColor = null);

        IToolTipProvider SetForegroundColor(Color? color);

        IToolTipProvider SetTitleForegroundColor(Color? color);

        IToolTipProvider SetTimeout(uint milliseconds, uint millisecondsShowdelay = 0);

        IToolTipProvider SetIcon(ImageSet? bitmap);

        IToolTipProvider SetTitleFont(Font? font);

        IToolTipProvider SetToolTipFromTemplate(
            TemplateControl template,
            Color? backColor = null);

        IToolTipProvider ResetText();

        IToolTipProvider ResetTitle();

        IToolTipProvider OnlyImage(ImageSet? image, Color? backColor);

        IToolTipProvider ShowToolTip(PointD? location = null);

        IToolTipProvider SetIcon(MessageBoxIcon icon);
    }
}