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
    public class DrawingResource : IEquatable<DrawingResource>
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
        /// Initializes a new instance of the <see cref="DrawingResource"/> class with specified brush.
        /// </summary>
        /// <param name="brush">Brush object.</param>
        public DrawingResource(Brush? brush)
        {
            this.brush = brush;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingResource"/> class with specified pen.
        /// </summary>
        /// <param name="pen">Pen object.</param>
        public DrawingResource(Pen? pen)
        {
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
        /// Compares two <see cref="DrawingResource"/> instances for equality.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>True if equal; otherwise false.</returns>
        public static bool operator ==(DrawingResource? left, DrawingResource? right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left is null || right is null)
                return false;

            return Equals(left.Brush, right.Brush)
                && Equals(left.Pen, right.Pen)
                && Equals(left.Color, right.Color);
        }

        /// <summary>
        /// Compares two <see cref="DrawingResource"/> instances for inequality.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>True if not equal; otherwise false.</returns>
        public static bool operator !=(DrawingResource? left, DrawingResource? right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is DrawingResource other && this == other;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            (object, object, object) tuple = (Brush ?? (object)0, Color ?? (object)0, Pen ?? (object)0);
            return tuple.GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals(DrawingResource? other)
        {
            return this == other;
        }
    }
}
