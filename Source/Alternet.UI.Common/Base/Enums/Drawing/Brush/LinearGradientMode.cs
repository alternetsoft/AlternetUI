using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the direction of a linear gradient.
    /// </summary>
    public enum LinearGradientMode
    {
        /// <summary>
        /// Specifies a horizontal linear gradient.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Specifies a vertical linear gradient.
        /// </summary>
        Vertical,

        /// <summary>
        /// Specifies a diagonal linear gradient from upper-left to lower-right.
        /// </summary>
        ForwardDiagonal,

        /// <summary>
        /// Specifies a diagonal linear gradient from upper-right to lower-left.
        /// </summary>
        BackwardDiagonal
    }
}
