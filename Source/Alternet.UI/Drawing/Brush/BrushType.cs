using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines possible <see cref="Brush"/> types.
    /// </summary>
    public enum BrushType
    {
        /// <summary>
        /// An empty brush.
        /// </summary>
        None,

        /// <summary>
        /// Brush is <see cref="SolidBrush"/>.
        /// </summary>
        Solid,

        /// <summary>
        /// Brush is <see cref="HatchBrush"/>.
        /// </summary>
        Hatch,

        /// <summary>
        /// Brush is <see cref="LinearGradientBrush"/>.
        /// </summary>
        LinearGradient,

        /// <summary>
        /// Brush is <see cref="RadialGradientBrush"/>.
        /// </summary>
        RadialGradient,
    }
}
