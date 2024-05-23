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
        Color? BackgroundColor { get; }

        /// <summary>
        /// Gets foreground color.
        /// </summary>
        Color? ForegroundColor { get; }

        /// <summary>
        /// Gets font.
        /// </summary>
        Font? Font { get; }
    }
}
