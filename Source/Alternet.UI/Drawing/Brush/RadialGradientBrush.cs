using System;
using System.Linq;

namespace Alternet.Drawing
{
    /// <summary>
    /// Paints an area with a radial gradient.
    /// </summary>
    public sealed class RadialGradientBrush : Brush
    {
        private Point center;
        private double radius;
        private Point gradientOrigin;
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
            : this(new Point(0, 0), 10, gradientStops)
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
        public RadialGradientBrush(Point center, double radius, GradientStop[] gradientStops)
            : this(center, radius, new Point(0, 0), gradientStops)
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
            Point center,
            double radius,
            Point gradientOrigin,
            GradientStop[] gradientStops)
            : this(new UI.Native.RadialGradientBrush())
        {
            this.center = center;
            this.radius = radius;
            this.gradientOrigin = gradientOrigin;
            this.gradientStops = gradientStops;

            ReinitializeNativeBrush();
        }

        private RadialGradientBrush(UI.Native.RadialGradientBrush nativeBrush)
            : base(nativeBrush, false)
        {
            gradientStops = new GradientStop[0];
        }

        /// <summary>
        /// Gets or sets the center of the outermost circle of the radial gradient.
        /// </summary>
        public Point Center
        {
            get => center;
            set
            {
                if (center == value)
                    return;
                CheckDisposed();
                center = value;
                ReinitializeNativeBrush();
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
                ReinitializeNativeBrush();
            }
        }

        /// <summary>
        /// Gets or sets the location of the two-dimensional focal point that defines the
        /// beginning of the gradient.
        /// </summary>
        public Point GradientOrigin
        {
            get => gradientOrigin;
            set
            {
                if (gradientOrigin == value)
                    return;
                CheckDisposed();
                gradientOrigin = value;
                ReinitializeNativeBrush();
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
                ReinitializeNativeBrush();
            }
        }

        /// <inheritdoc/>
        public override BrushType BrushType => BrushType.RadialGradient;

        internal override Color BrushColor => GradientStops.Length > 0 ?
            GradientStops[0].Color : Color.Black;

        internal new UI.Native.RadialGradientBrush NativeBrush =>
            (UI.Native.RadialGradientBrush)base.NativeBrush;

        private protected override bool EqualsCore(Brush other)
        {
            var o = other as RadialGradientBrush;
            if (o == null)
                return false;

            return
                Center == o.Center &&
                GradientOrigin == o.GradientOrigin &&
                Radius == o.Radius &&
                Enumerable.SequenceEqual(GradientStops, o.GradientStops);
        }

        private protected override int GetHashCodeCore()
        {
            var hashCode = new HashCode();
            hashCode.Add(Center);
            hashCode.Add(GradientOrigin);
            hashCode.Add(Radius);
            foreach (var gradientStop in GradientStops)
                hashCode.Add(gradientStop);

            return hashCode.ToHashCode();
        }

        private protected override string ToStringCore()
        {
            try
            {
                return $"RadialGradientBrush ({Center}, {Radius}, {GradientOrigin}, {GradientStops})";
            }
            catch
            {
                return $"RadialGradientBrush";
            }
        }

        private static GradientStop[] GetGradientStopsFromEdgeColors(
            Color startColor,
            Color endColor) => new[]
            {
                new GradientStop(startColor, 0),
                new GradientStop(endColor, 1),
            };

        private void ReinitializeNativeBrush()
        {
            NativeBrush.Initialize(
                center,
                radius,
                gradientOrigin,
                gradientStops.Select(x => x.Color).ToArray(),
                gradientStops.Select(x => x.Offset).ToArray());
        }
    }
}