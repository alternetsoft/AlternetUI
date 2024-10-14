using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IRichToolTip
    {
        IRichToolTip SetToolTip(string message, bool systemColors = true);

        IRichToolTip ShowToolTip(
            string message,
            bool systemColors = true,
            PointD? location = null);

        IRichToolTip HideToolTip();

        IRichToolTip SetToolTip(
            object? title,
            string? message,
            MessageBoxIcon? icon = null,
            uint? timeoutMilliseconds = null);

        IRichToolTip ShowToolTip(
            object? title,
            string? message,
            MessageBoxIcon? icon = null,
            uint? timeoutMilliseconds = null,
            PointD? location = null);

        IRichToolTip SetBackgroundColor(Color? color, Color? endColor = null);

        IRichToolTip SetForegroundColor(Color? color);

        IRichToolTip SetTitleForegroundColor(Color? color);

        IRichToolTip SetTimeout(uint milliseconds, uint millisecondsShowdelay = 0);

        IRichToolTip SetIcon(ImageSet? bitmap);

        IRichToolTip SetTitleFont(Font? font);

        IRichToolTip SetToolTipFromTemplate(
            TemplateControl template,
            Color? backColor = null);

        IRichToolTip ShowToolTipFromTemplate(
            TemplateControl template,
            Color? backColor = null,
            PointD? location = null);

        IRichToolTip ResetText();

        IRichToolTip ResetTitle();

        IRichToolTip OnlyImage(
            ImageSet? image,
            Color? backColor);

        IRichToolTip ShowToolTip(PointD? location = null);

        IRichToolTip SetIcon(MessageBoxIcon icon);
    }
}
