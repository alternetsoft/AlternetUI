using System;
using System.Collections.Generic;
using System.Text;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains <see cref="Brush"/> and <see cref="Pen"/> properties.
    /// </summary>
    public class BrushAndPen
    {
        private Brush? brush;
        private Pen? pen;

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
        {
            this.brush = brush;
            this.pen = pen;
        }

        /// <summary>
        /// Gets or sets brush value.
        /// </summary>
        public virtual Brush? Brush
        {
            get
            {
                return brush;
            }

            set
            {
                if (brush == value)
                    return;
                brush = value;
            }
        }

        /// <summary>
        /// Gets or sets pen value.
        /// </summary>
        public virtual Pen? Pen
        {
            get
            {
                return pen;
            }

            set
            {
                if (pen == value)
                    return;
                pen = value;
            }
        }

        /// <summary>
        /// Gets this object as <see cref="SKPaint"/>.
        /// </summary>
        public SKPaint AsPaint
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
