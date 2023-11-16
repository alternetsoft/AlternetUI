using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods to get and set background color, foreground color and font.
    /// </summary>
    internal interface IFontAndColor : IReadOnlyFontAndColor
    {
        /// <summary>
        /// Gets or sets background color.
        /// </summary>
        new Color? BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets foreground color.
        /// </summary>
        new Color? ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets font.
        /// </summary>
        new Font? Font { get; set; }
    }
}
