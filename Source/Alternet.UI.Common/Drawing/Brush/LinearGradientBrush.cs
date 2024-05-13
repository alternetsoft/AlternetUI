using System;
using System.Linq;

namespace Alternet.Drawing
{
    /// <summary>
    /// Paints an area with a linear gradient.
    /// </summary>
    public class LinearGradientBrush : Brush
    {
        private PointD startPoint;
        private PointD endPoint;
        private GradientStop[] gradientStops;

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
            : this(GetGradientStopsFromEdgeColors(startColor, endColor))
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
            : this(startPoint, endPoint, GetGradientStopsFromEdgeColors(startColor, endColor))
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
            : base(false)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.gradientStops = gradientStops;
        }

        /// <summary>
        /// Gets or sets the starting two-dimensional coordinates of the linear gradient.
        /// </summary>
        public PointD StartPoint
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
        public PointD EndPoint
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

        /// <summary>
        /// Gets or sets the <see cref="GradientStop"/> instances array defining the color
        /// transition in this brush.
        /// </summary>
        public GradientStop[] GradientStops
        {
            get => gradientStops;

            set
            {
                if (gradientStops == value)
                    return;
                CheckDisposed();
                gradientStops = value;
                UpdateRequired = true;
            }
        }

        /// <inheritdoc/>
        public override BrushType BrushType => BrushType.LinearGradient;

        /// <inheritdoc/>
        public override Color BrushColor => GradientStops.Length > 0 ?
            GradientStops[0].Color : Color.Black;

        /// <summary>
        /// Converts two colors to array of <see cref="GradientStop"/>.
        /// </summary>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        /// <returns></returns>
        public static GradientStop[] GetGradientStopsFromEdgeColors(
            Color startColor,
            Color endColor) => new[]
            {
                new GradientStop(startColor, 0),
                new GradientStop(endColor, 1),
            };

        /// <inheritdoc/>
        public override string ToString()
        {
            try
            {
                return $"LinearGradientBrush (StartPoint={StartPoint}, EndPoint={EndPoint}," +
                    $" GradientStops=({GradientStopsToString(GradientStops)}))";
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
            var hashCode = new HashCode();
            hashCode.Add(StartPoint);
            hashCode.Add(EndPoint);
            foreach (var gradientStop in GradientStops)
                hashCode.Add(gradientStop);

            return hashCode.ToHashCode();
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
                Enumerable.SequenceEqual(GradientStops, o.GradientStops);
        }

        internal static string GradientStopsToString(GradientStop[] stops)
        {
            string result = string.Empty;
            foreach(var item in stops)
            {
                if (result.Length > 0)
                    result += ", ";
                result += item.ToString();
            }

            return result;
        }

        /// <inheritdoc/>
        protected override object CreateHandler()
        {
            return NativeDrawing.Default.CreateLinearGradientBrush();
        }

        /// <inheritdoc/>
        protected override void UpdateHandler()
        {
            NativeDrawing.Default.UpdateLinearGradientBrush(this);
        }
    }
}