using System;
using System.Linq;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Paints an area with a linear gradient.
    /// </summary>
    public class LinearGradientBrush : GradientBrush
    {
        private PointD startPoint;
        private PointD endPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class.
        /// </summary>
        public LinearGradientBrush()
            : this(Array.Empty<GradientStop>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class
        /// with the specified start color and end color.
        /// </summary>
        /// <param name="startColor">The Color at offset 0.0.</param>
        /// <param name="endColor">The Color at offset 1.0.</param>
        public LinearGradientBrush(Color startColor, Color endColor)
            : this(GradientStopsFromEdgeColors(startColor, endColor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class that has
        /// the specified gradient stops.
        /// </summary>
        /// <param name="gradientStops">The <see cref="GradientStop"/> instances array to set on
        /// this brush.</param>
        public LinearGradientBrush(GradientStop[] gradientStops)
            : this(new PointD(), new PointD(1, 1), gradientStops)
        {
        }

        /// <summary>
        /// Initializes a new instance of the LinearGradientBrush class that has the specified
        /// start color, end color,
        /// start point, end point.
        /// </summary>
        /// <param name="startPoint">The start point of the gradient.</param>
        /// <param name="endPoint">The end point of the gradient.</param>
        /// <param name="startColor">The start color of the gradient.</param>
        /// <param name="endColor">The end color of the gradient.</param>
        public LinearGradientBrush(
            PointD startPoint,
            PointD endPoint,
            Color startColor,
            Color endColor)
            : this(startPoint, endPoint, GradientStopsFromEdgeColors(startColor, endColor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class that has
        /// the specified gradient stops,
        /// start point and end point.
        /// </summary>
        /// <param name="startPoint">The start point of the gradient.</param>
        /// <param name="endPoint">The end point of the gradient.</param>
        /// <param name="gradientStops">The <see cref="GradientStop"/> instances array to set on
        /// this brush.</param>
        public LinearGradientBrush(PointD startPoint, PointD endPoint, GradientStop[] gradientStops)
            : base(gradientStops, false)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        /// <summary>
        /// Gets or sets the starting two-dimensional coordinates of the linear gradient.
        /// </summary>
        public virtual PointD StartPoint
        {
            get => startPoint;

            set
            {
                if (startPoint == value)
                    return;
                CheckDisposed();
                startPoint = value;
                UpdateRequired = true;
            }
        }

        /// <summary>
        /// Gets or sets the ending two-dimensional coordinates of the linear gradient.
        /// </summary>
        public virtual PointD EndPoint
        {
            get => endPoint;

            set
            {
                if (endPoint == value)
                    return;
                CheckDisposed();
                endPoint = value;
                UpdateRequired = true;
            }
        }

        /// <inheritdoc/>
        public override BrushType BrushType => BrushType.LinearGradient;

        /// <inheritdoc/>
        public override string ToString()
        {
            try
            {
                return $"LinearGradientBrush (StartPoint={StartPoint}, EndPoint={EndPoint}," +
                    $" GradientStops=({ToString(GradientStops)}))";
            }
            catch
            {
                return $"LinearGradientBrush";
            }
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode1 = (StartPoint, EndPoint).GetHashCode();
            var hashCode2 = base.GetHashCode();
            return MathUtils.CombineHashCodes(hashCode1, hashCode2);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public override bool Equals(object? other)
        {
            var o = other as LinearGradientBrush;
            if (o == null)
                return false;
            CheckDisposed();

            return
                StartPoint == o.StartPoint &&
                EndPoint == o.EndPoint &&
                base.Equals(other);
        }

        /// <inheritdoc/>
        protected override SKShader CreateSkiaShader()
        {
            SKShader result;

            if(LocalMatrix != SKMatrix.Empty)
            {
                result = SKShader.CreateLinearGradient(
                    startPoint,
                    endPoint,
                    RadialGradientBrush.ToSkiaGradientColors(GradientStops),
                    RadialGradientBrush.ToGradientOffsetsF(GradientStops),
                    TileMode,
                    LocalMatrix);
            }
            else
            {
                result = SKShader.CreateLinearGradient(
                    startPoint,
                    endPoint,
                    RadialGradientBrush.ToSkiaGradientColors(GradientStops),
                    RadialGradientBrush.ToGradientOffsetsF(GradientStops),
                    TileMode);
            }

            return result;
        }

        /// <inheritdoc/>
        protected override IBrushHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateLinearGradientBrushHandler(this);
        }

        /// <inheritdoc/>
        protected override void UpdateHandler()
        {
            ((ILinearGradientBrushHandler)Handler).Update(this);
        }
    }
}