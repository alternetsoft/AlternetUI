using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with rich tooltip.
    /// </summary>
    internal interface IRichToolTipHandler : IDisposableObject
    {
        /// <summary>
        /// Gets tooltip window size in pixels.
        /// </summary>
        SizeI SizeInPixels { get; }

        /// <summary>
        /// Sets location decrement for the tooltip.
        /// </summary>
        void SetLocationDecrement(bool decrementX, bool decrementY);

        /// <summary>
        /// Sets background colors.
        /// </summary>
        void SetBackgroundColor(Color color, Color endColor);

        /// <summary>
        /// Sets tooltip icon to the specified <see cref="MessageBoxIcon"/>.
        /// </summary>
        /// <param name="icon">Tooltip icon id.</param>
        void SetIcon(MessageBoxIcon icon);

        /// <inheritdoc cref="RichToolTip.SetToolTipForegroundColor"/>
        void SetForegroundColor(Color color);

        /// <inheritdoc cref="RichToolTip.SetTitleForegroundColor"/>
        void SetTitleForegroundColor(Color color);

        /// <inheritdoc cref="RichToolTip.SetTimeout"/>
        void SetTimeout(uint milliseconds, uint millisecondsShowdelay = 0);

        /// <summary>
        /// Sets tooltip icon.
        /// </summary>
        /// <param name="bitmap">Tooltip icon.</param>
        void SetIcon(ImageSet? bitmap);

        /// <inheritdoc cref="RichToolTip.SetTitleFont"/>
        void SetTitleFont(Font? font);

        /// <summary>
        /// Sets tip kind of the tooltip.
        /// </summary>
        /// <param name="tipKind">Value to set.</param>
        void SetTipKind(RichToolTipKind tipKind);

        /// <summary>
        /// Shows tooltip on the screen.
        /// </summary>
        /// <param name="control">Control which bounds are used as base position of the tooltip.</param>
        /// <param name="rect">Tooltip size and relative position.</param>
        /// <param name="adjustPos">Whether to adjust position depending on the tip kind.</param>
        void Show(AbstractControl control, RectI? rect = null, bool adjustPos = true);
    }
}
