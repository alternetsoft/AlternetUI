using System;
using System.Linq;

namespace Alternet.Drawing
{
    /// <summary>
    /// Paints an area with a radial gradient.
    /// </summary>
    public class RadialGradientBrush : Brush
    {
        private PointD center;
        private double radius;
        private PointD gradientOrigin;
        private GradientStop[] gradientStops;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class.
        /// </summary>
        public RadialGradientBrush()
            : this(Array.Empty<GradientStop>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class
        /// with the specified start color and end color.
        /// </summary>
        /// <param name="startColor">The Color at offset 0.0.</param>
        /// <param name="endColor">The Color at offset 1.0.</param>
        public RadialGradientBrush(Color startColor, Color endColor)
            : this(GetGradientStopsFromEdgeColors(startColor, endColor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class that has
        /// the specified gradient stops.
        /// </summary>
        /// <param name="gradientStops">The <see cref="GradientStop"/> instances array to set on
        /// this brush.</param>
        public RadialGradientBrush(GradientStop[] gradientStops)
            : this(new PointD(0, 0), 10, gradientStops)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RadialGradientBrush class that has the specified
        /// center, radius, and gradient stops.
        /// </summary>
        /// <param name="center">The center of the outermost circle of the radial gradient.</param>
        /// <param name="radius">The radius of the outermost circle of the radial gradient.</param>
        /// <param name="gradientStops">The <see cref="GradientStop"/> instances array to set on
        /// this brush.</param>
        public RadialGradientBrush(PointD center, double radius, GradientStop[] gradientStops)
            : this(center, radius, new PointD(0, 0), gradientStops)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class that has
        /// the specified gradient stops,
        /// start point and end point.
        /// </summary>
        /// <param name="center">The center of the outermost circle of the radial gradient.</param>
        /// <param name="radius">The radius of the outermost circle of the radial gradient.</param>
        /// <param name="gradientOrigin">The location of the two-dimensional focal point that
        /// defines the beginning of the gradient.</param>
        /// <param name="gradientStops">The <see cref="GradientStop"/> instances array to set on
        /// this brush.</param>
        public RadialGradientBrush(
            PointD center,
            double radius,
            PointD gradientOrigin,
            GradientStop[] gradientStops)
            : base(false)
        {
            this.center = center;
            this.radius = radius;
            this.gradientOrigin = gradientOrigin;
            this.gradientStops = gradientStops;
        }

        /// <summary>
        /// Gets or sets the center of the outermost circle of the radial gradient.
        /// </summary>
        public PointD Center
        {
            get => center;
            set
            {
                if (center == value)
                    return;
                CheckDisposed();
                center = value;
                UpdateRequired = true;
            }
        }

        /// <summary>
        /// Gets or sets the radius of the outermost circle of the radial gradient.
        /// </summary>
        public double Radius
        {
            get => radius;
            set
            {
                if (radius == value)
                    return;
                CheckDisposed();
                radius = value;
                UpdateRequired = true;
            }
        }

        /// <summary>
        /// Gets or sets the location of the two-dimensional focal point that defines the
        /// beginning of the gradient.
        /// </summary>
        public PointD GradientOrigin
        {
            get => gradientOrigin;
            set
            {
                if (gradientOrigin == value)
                    return;
                CheckDisposed();
                gradientOrigin = value;
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
        public override BrushType BrushType => BrushType.RadialGradient;

        /// <inheritdoc/>
        public override Color BrushColor => GradientStops.Length > 0 ?
            GradientStops[0].Color : Color.Black;

        /// <inheritdoc/>
        public override string ToString()
        {
            try
            {
                return $"RadialGradientBrush (Center={Center}, Radius={Radius}," +
                    $" GradientOrigin={GradientOrigin}," +
                    $" GradientStops=({LinearGradientBrush.GradientStopsToString(GradientStops)}))";
            }
            catch
            {
                return $"RadialGradientBrush";
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public override bool Equals(object? other)
        {
            var o = other as RadialGradientBrush;
            if (o == null)
                return false;
            CheckDisposed();
            return
                Center == o.Center &&
                GradientOrigin == o.GradientOrigin &&
                Radius == o.Radius &&
                Enumerable.SequenceEqual(GradientStops, o.GradientStops);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Center);
            hashCode.Add(GradientOrigin);
            hashCode.Add(Radius);
            foreach (var gradientStop in GradientStops)
                hashCode.Add(gradientStop);

            return hashCode.ToHashCode();
        }

        /// <inheritdoc/>
        protected override object CreateHandler()
        {
            return NativeDrawing.Default.CreateRadialGradientBrush();
        }

        /// <inheritdoc/>
        protected override void UpdateHandler()
        {
            NativeDrawing.Default.UpdateRadialGradientBrush(this);
        }

        private static GradientStop[] GetGradientStopsFromEdgeColors(
            Color startColor,
            Color endColor) => new[]
            {
                new GradientStop(startColor, 0),
                new GradientStop(endColor, 1),
            };
    }
}