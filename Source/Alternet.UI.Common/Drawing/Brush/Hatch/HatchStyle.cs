using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies hatch styles that can be used to fill shapes.
    /// </summary>
    public enum HatchStyle
    {
        /// <summary>Horizontal lines.</summary>
        Horizontal = 0,

        /// <summary>Vertical lines.</summary>
        Vertical = 1,

        /// <summary>Diagonal lines slanting forward (bottom-left to top-right).</summary>
        ForwardDiagonal = 2,

        /// <summary>Diagonal lines slanting backward (top-left to bottom-right).</summary>
        BackwardDiagonal = 3,

        /// <summary>Crosshatch with horizontal and vertical lines.</summary>
        Cross = 4,

        /// <summary>Crosshatch with forward and backward diagonal lines.</summary>
        DiagonalCross = 5,
    }
}
