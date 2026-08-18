using System;
using System.Collections.Generic;
using System.Text;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains <see cref="Brush"/> and <see cref="Pen"/> properties.
    /// </summary>
    [Obsolete("Use DrawingResource class.")]
    public class BrushAndPen : DrawingResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrushAndPen"/> class.
        /// </summary>
        public BrushAndPen()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushAndPen"/> class.
        /// </summary>
        /// <param name="brush">Brush object.</param>
        /// <param name="pen">Pen object.</param>
        public BrushAndPen(Brush? brush, Pen? pen)
            : base(brush, pen)
        {
        }
    }
}
