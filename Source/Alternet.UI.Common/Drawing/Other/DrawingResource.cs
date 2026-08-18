using System;
using System.Collections.Generic;
using System.Text;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a drawing resource that can be defined
    /// by a brush, pen, or color.
    /// </summary>
    public class DrawingResource
    {
        private Brush? brush;
        private Pen? pen;
        private Color? color;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingResource"/> class.
        /// </summary>
        public DrawingResource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingResource"/> class with specified brush and pen.
        /// </summary>
        /// <param name="brush">Brush object.</param>
        /// <param name="pen">Pen object.</param>
        public DrawingResource(Brush? brush, Pen? pen)
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
        /// Gets or sets color value.
        /// </summary>
        public virtual Color? Color
        {
            get => color;
            set
            {
                color = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this object has a brush.
        /// </summary>
        public bool HasBrush => Brush != null;

        /// <summary>
        /// Gets a value indicating whether this object has a pen.
        /// </summary>
        public bool HasPen => Pen != null;

        /// <summary>
        /// Gets a value indicating whether this object has a color.
        /// </summary>
        public bool HasColor => Color != null;

        /// <summary>
        /// Gets this object as <see cref="SKPaint"/>.
        /// </summary>
        public (SKPaint? Fill, SKPaint? Stroke) AsPaint
        {
            get
            {
                return (brush?.SkiaPaint ?? Color?.AsBrush.SkiaPaint, pen?.SkiaPaint ?? Color?.AsBrush.SkiaPaint);
            }
        }
    }
}
