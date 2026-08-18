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
        private string? title;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingResource"/> class.
        /// </summary>
        public DrawingResource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingResource"/> class with specified color.
        /// </summary>
        /// <param name="color">Color object.</param>
        public DrawingResource(Color? color)
        {
            this.color = color;
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
        /// Gets or sets title of the drawing resource.
        /// </summary>
        public virtual string? Title
        {
            get
            {
                var result = title;

                if (result != null)
                    return result;

                if (HasColor)
                    return Color?.ToDisplayString();

                return result;
            }

            set
            {
                title = value;
            }
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

        /// <summary>
        /// Defines implicit conversion from <see cref="Brush"/> to <see cref="DrawingResource"/>.
        /// </summary>
        public static implicit operator DrawingResource?(Brush? brush)
        {
            return brush == null ? null : new DrawingResource { Brush = brush };
        }

        /// <summary>
        /// Defines implicit conversion from <see cref="Pen"/> to <see cref="DrawingResource"/>.
        /// </summary>
        public static implicit operator DrawingResource?(Pen? pen)
        {
            return pen == null ? null : new DrawingResource { Pen = pen };
        }

        /// <summary>
        /// Defines implicit conversion from <see cref="Color"/> to <see cref="DrawingResource"/>.
        /// </summary>
        public static implicit operator DrawingResource?(Color? color)
        {
            return color == null ? null : new DrawingResource { Color = color };
        }

        /// <summary>
        /// Defines implicit conversion from <see cref="DrawingResource"/> to <see cref="Brush"/>.
        /// </summary>
        /// <param name="resource">The drawing resource to convert.</param>
        public static implicit operator Brush?(DrawingResource? resource)
        {
            return resource?.Brush;
        }

        /// <summary>
        /// Defines implicit conversion from <see cref="DrawingResource"/> to <see cref="Pen"/>.
        /// </summary>
        /// <param name="resource">The drawing resource to convert.</param>
        public static implicit operator Pen?(DrawingResource? resource)
        {
            return resource?.Pen;
        }
        
        /// <summary>
        /// Defines implicit conversion from <see cref="DrawingResource"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="resource">The drawing resource to convert.</param>
        public static implicit operator Color?(DrawingResource? resource)
        {
            return resource?.Color;
        }
    }
}
