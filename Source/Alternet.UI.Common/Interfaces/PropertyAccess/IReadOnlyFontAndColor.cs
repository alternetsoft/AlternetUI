using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods to get background color, foreground color and font.
    /// </summary>
    public interface IReadOnlyFontAndColor
    {
        /// <summary>
        /// Gets background color.
        /// </summary>
        LightDarkColor? BackgroundColor { get; }

        /// <summary>
        /// Gets foreground color.
        /// </summary>
        LightDarkColor? ForegroundColor { get; }

        /// <summary>
        /// Gets font.
        /// </summary>
        Font? Font { get; }

        /// <summary>
        /// Gets this object with changed font.
        /// </summary>
        /// <returns></returns>
        IReadOnlyFontAndColor WithFont(Font? font);

        /// <summary>
        /// Gets this object with changed foreground color.
        /// </summary>
        /// <returns></returns>
        IReadOnlyFontAndColor WithForeColor(LightDarkColor? color);

        /// <summary>
        /// Gets this object with changed background color.
        /// </summary>
        /// <returns></returns>
        IReadOnlyFontAndColor WithBackColor(LightDarkColor? color);
    }
}
