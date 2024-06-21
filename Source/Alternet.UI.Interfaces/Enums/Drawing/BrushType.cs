using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines possible brush types.
    /// </summary>
    public enum BrushType
    {
        /// <summary>
        /// An empty brush.
        /// </summary>
        None,

        /// <summary>
        /// Brush is solid.
        /// </summary>
        Solid,

        /// <summary>
        /// Brush is hatch.
        /// </summary>
        Hatch,

        /// <summary>
        /// Brush is linear gradient.
        /// </summary>
        LinearGradient,

        /// <summary>
        /// Brush is radial gradient.
        /// </summary>
        RadialGradient,

        /// <summary>
        /// Transparent brush.
        /// </summary>
        Transparent,

        /// <summary>
        /// Brush is texture.
        /// </summary>
        Texture,
    }
}
