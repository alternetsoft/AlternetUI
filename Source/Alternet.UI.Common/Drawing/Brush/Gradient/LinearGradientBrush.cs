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
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class that has
        /// the specified bounding rectangle, start color, end color, and linear gradient mode.
        /// </summary>
        /// <param name="rect">The rectangle to calculate the start and end points from.</param>
        /// <param name="color1">The start color of the gradient.</param>
        /// <param name="color2">The end color of the gradient.</param>
        /// <param name="linearGradientMode">The linear gradient mode.</param>
        public LinearGradientBrush(RectD rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
            : this(GetStartPointFromRect(rect, linearGradientMode), GetEndPointFromRect(rect, linearGradientMode), color1, color2)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class that has
        /// the specified bounding rectangle, start color, end color, and angle.
        /// </summary>
        /// <param name="rect">The rectangle to calculate the start and end points from.</param>
        /// <param name="color1">The start color of the gradient.</param>
        /// <param name="color2">The end color of the gradient.</param>
        /// <param name="angle">The angle of the gradient.</param>
        /// <param name="isAngleScaleable">Indicates whether the angle is scalable.</param>
        public LinearGradientBrush(RectD rect, Color color1, Color color2, float angle, bool isAngleScaleable)
               : this(GetStartEndPoints(rect, angle, isAngleScaleable), color1, color2)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class that has
        /// the specified bounding rectangle, start color, end color, and angle.
        /// </summary>
        /// <param name="rect">The rectangle to calculate the start and end points from.</param>
        /// <param name="color1">The start color of the gradient.</param>
        /// <param name="color2">The end color of the gradient.</param>
        /// <param name="angle">The angle of the gradient.</param>
        public LinearGradientBrush(RectD rect, Color color1, Color color2, float angle)
            : this(rect, color1, color2, angle, isAngleScaleable: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the LinearGradientBrush class that has the specified
        /// start color, end color, start point, and end point.
        /// </summary>
        /// <param name="startPoint">The start point of the gradient.</param>
        /// <param name="endPoint">The end point of the gradient.</param>
        /// <param name="startColor">The start color of the gradient.</param>
        /// <param name="endColor">The end color of the gradient.</param>
        public LinearGradientBrush(PointD startPoint, PointD endPoint, Color startColor, Color endColor)
            : this(startPoint, endPoint, GradientStopsFromEdgeColors(startColor, endColor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class that has the specified
        /// start point, end point, start color, and end color. Start and end points are specified as a tuple of <see cref="PointD"/> values.
        /// </summary>
        /// <param name="points">A tuple containing the start and end points of the gradient.</param>
        /// <param name="startColor">The start color of the gradient.</param>
        /// <param name="endColor">The end color of the gradient.</param>
        public LinearGradientBrush((PointD StartPoint, PointD EndPoint) points, Color startColor, Color endColor)
            : this(points.StartPoint, points.EndPoint, GradientStopsFromEdgeColors(startColor, endColor))
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
                UpdateRequired();
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
                UpdateRequired();
            }
        }

        /// <inheritdoc/>
        public override BrushType BrushType => BrushType.LinearGradient;

        /// <summary>
        /// Gets the start and end points of the linear gradient as a tuple of <see cref="PointD"/> values.
        /// </summary>
        /// <param name="rect">The rectangle to calculate the start and end points from.</param>
        /// <param name="angle">The angle of the linear gradient in degrees.</param>
        /// <param name="isAngleScaleable">Indicates whether the angle is scalable.</param>
        /// <returns>A tuple containing the start and end points of the linear gradient.</returns>
        public static (PointD, PointD) GetStartEndPoints(RectD rect, float angle, bool isAngleScaleable)
        {
            float radians = angle * MathF.PI / 180f;
            float dx = MathF.Cos(radians);
            float dy = MathF.Sin(radians);

            // Center of rectangle
            var center = new PointD(rect.Left + rect.Width / 2f, rect.Top + rect.Height / 2f);

            // Half dimensions
            float halfX = rect.Width / 2f;
            float halfY = rect.Height / 2f;

            // Scale vector depending on isAngleScaleable
            float scale = isAngleScaleable
                ? MathF.Sqrt(halfX * halfX + halfY * halfY)   // stretch with rect
                : MathF.Min(halfX, halfY);                    // fixed length

            var startPoint = new PointD(center.X - dx * scale, center.Y - dy * scale);
            var endPoint = new PointD(center.X + dx * scale, center.Y + dy * scale);

            return (startPoint, endPoint);
        }

        /// <summary>
        /// Gets the start point of the linear gradient based on the specified rectangle and linear gradient mode.
        /// </summary>
        /// <param name="rect">The rectangle to calculate the start point from.</param>
        /// <param name="linearGradientMode">The linear gradient mode.</param>
        /// <returns>The start point of the linear gradient.</returns>
        public static PointD GetStartPointFromRect(RectD rect, LinearGradientMode linearGradientMode)
        {
            switch (linearGradientMode)
            {
                case LinearGradientMode.Horizontal:
                default:
                    return new PointD(rect.Left, rect.Top);
                case LinearGradientMode.Vertical:
                    return new PointD(rect.Left, rect.Top);
                case LinearGradientMode.ForwardDiagonal:
                    return new PointD(rect.Left, rect.Top);
                case LinearGradientMode.BackwardDiagonal:
                    return new PointD(rect.Right, rect.Top);
            }
        }

        /// <summary>
        /// Gets the end point of the linear gradient from the specified rectangle and linear gradient mode.
        /// </summary>
        /// <param name="rect">The rectangle to calculate the end point from.</param>
        /// <param name="linearGradientMode">The linear gradient mode.</param>
        /// <returns>The end point of the linear gradient.</returns>
        public static PointD GetEndPointFromRect(RectD rect, LinearGradientMode linearGradientMode)
        {
            switch (linearGradientMode)
            {
                case LinearGradientMode.Horizontal:
                default:
                    return new PointD(rect.Right, rect.Top);
                case LinearGradientMode.Vertical:
                    return new PointD(rect.Left, rect.Bottom);
                case LinearGradientMode.ForwardDiagonal:
                    return new PointD(rect.Right, rect.Bottom);
                case LinearGradientMode.BackwardDiagonal:
                    return new PointD(rect.Left, rect.Bottom);
            }
        }

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

            return
                StartPoint == o.StartPoint &&
                EndPoint == o.EndPoint &&
                base.Equals(other);
        }

        /// <inheritdoc/>
        protected override SKShader CreateSkiaShader()
        {
            SKShader result;

            if (!LocalMatrix.IsIdentity)
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
    }
}
