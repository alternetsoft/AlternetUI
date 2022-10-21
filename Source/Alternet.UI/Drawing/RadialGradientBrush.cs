using System;
using System.Linq;

namespace Alternet.Drawing
{
    /// <summary>
    /// Paints an area with a radial gradient.
    /// </summary>
    public sealed class RadialGradientBrush : IDisposable
    {
        private bool isDisposed;
        private Point center;
        private double radius;
        private Point gradientOrigin;
        private GradientStop[] gradientStops;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class.
        /// </summary>
        public RadialGradientBrush() : this(new GradientStop[0])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class with the specified start color and end color.
        /// </summary>
        /// <param name="startColor">The Color at offset 0.0.</param>
        /// <param name="endColor">The Color at offset 1.0.</param>
        public RadialGradientBrush(Color startColor, Color endColor) : this(GetGradientStopsFromEdgeColors(startColor, endColor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class that has the specified gradient stops.
        /// </summary>
        /// <param name="gradientStops">The <see cref="GradientStop"/> instances array to set on this brush.</param>
        public RadialGradientBrush(GradientStop[] gradientStops) : this(new Point(0.5, 0.5), 0.5, gradientStops)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RadialGradientBrush class that has the specified center, radius, and gradient stops.
        /// </summary>
        /// <param name="center">The center of the outermost circle of the radial gradient.</param>
        /// <param name="radius">The radius of the outermost circle of the radial gradient.</param>
        /// <param name="gradientStops">The <see cref="GradientStop"/> instances array to set on this brush.</param>
        public RadialGradientBrush(Point center, double radius, GradientStop[] gradientStops) :
            this(center, radius, new Point(0.5, 0.5), gradientStops)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class that has the specified gradient stops,
        /// start point and end point.
        /// </summary>
        /// <param name="center">The center of the outermost circle of the radial gradient.</param>
        /// <param name="radius">The radius of the outermost circle of the radial gradient.</param>
        /// <param name="gradientOrigin">The location of the two-dimensional focal point that defines the beginning of the gradient.</param>
        /// <param name="gradientStops">The <see cref="GradientStop"/> instances array to set on this brush.</param>
        public RadialGradientBrush(Point center, double radius, Point gradientOrigin, GradientStop[] gradientStops) : this(new UI.Native.RadialGradientBrush())
        {
            this.center = center;
            this.radius = radius;
            this.gradientOrigin = gradientOrigin;
            this.gradientStops = gradientStops;

            ReinitializeNativeBrush();
        }

        private RadialGradientBrush(UI.Native.RadialGradientBrush nativeBrush)
        {
            NativeBrush = nativeBrush;
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
                CheckDisposed();

                if (center == value)
                    return;

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
                CheckDisposed();

                if (radius == value)
                    return;

                radius = value;
                ReinitializeNativeBrush();
            }
        }

        /// <summary>
        /// Gets or sets the location of the two-dimensional focal point that defines the beginning of the gradient.
        /// </summary>
        public Point GradientOrigin
        {
            get => gradientOrigin;
            set
            {
                CheckDisposed();

                if (gradientOrigin == value)
                    return;

                gradientOrigin = value;
                ReinitializeNativeBrush();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="GradientStop"/> instances array defining the color transition in this brush.
        /// </summary>
        public GradientStop[] GradientStops
        {
            get => gradientStops;
            set
            {
                CheckDisposed();

                if (gradientStops == value)
                    return;

                gradientStops = value;
                ReinitializeNativeBrush();
            }
        }

        internal UI.Native.RadialGradientBrush NativeBrush { get; private set; }

        /// <summary>
        /// Releases all resources used by this <see cref="Pen"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private static GradientStop[] GetGradientStopsFromEdgeColors(Color startColor, Color endColor) =>
                                    new[] { new GradientStop(startColor, 0), new GradientStop(endColor, 1) };

        private void ReinitializeNativeBrush()
        {
            NativeBrush.Initialize(
                center,
                radius,
                gradientOrigin,
                gradientStops.Select(x => x.Color).ToArray(),
                gradientStops.Select(x => x.Offset).ToArray());
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if the object has been disposed.
        /// </summary>
        private void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
        }

        /// <inheritdoc/>
        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativeBrush.Dispose();
                    NativeBrush = null!;
                }

                isDisposed = true;
            }
        }
    }
}