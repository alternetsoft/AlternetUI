using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates possible number of colors used in <see cref="SvgImage"/>.
    /// </summary>
    public enum SvgImageNumOfColors
    {
        /// <summary>
        /// Svg has single color.
        /// </summary>
        One,

        /// <summary>
        /// Svg has two colors.
        /// </summary>
        Two,

        /// <summary>
        /// Svg has many colors.
        /// </summary>
        Many,

        /// <summary>
        /// Svg colors is unknown.
        /// </summary>
        Uknown,
    }
}
