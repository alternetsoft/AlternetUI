using System;
using System.Linq;

namespace Alternet.Drawing
{
    /// <summary>
    /// Paints an area with a linear gradient.
    /// </summary>
    public sealed class LinearGradientBrush : IDisposable
    {
        private bool isDisposed;
        private Point startPoint;
        private Point endPoint;
        private GradientStop[] gradientStops;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class.
        /// </summary>
        public LinearGradientBrush() : this(new GradientStop[0])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class with the specified start color and end color.
        /// </summary>
        /// <param name="startColor">The Color at offset 0.0.</param>
        /// <param name="endColor">The Color at offset 1.0.</param>
        public LinearGradientBrush(Color startColor, Color endColor) : this(GetGradientStopsFromEdgeColors(startColor, endColor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class that has the specified gradient stops.
        /// </summary>
        /// <param name="gradientStops">The <see cref="GradientStop"/> instances array to set on this brush.</param>
        public LinearGradientBrush(GradientStop[] gradientStops) : this(new Point(), new Point(1, 1), gradientStops)
        {
        }

        /// <summary>
        /// Initializes a new instance of the LinearGradientBrush class that has the specified start color, end color,
        /// start point, end point.
        /// </summary>
        /// <param name="startPoint">The start point of the gradient.</param>
        /// <param name="endPoint">The end point of the gradient.</param>
        /// <param name="startColor">The start color of the gradient.</param>
        /// <param name="endColor">The end color of the gradient.</param>
        public LinearGradientBrush(Point startPoint, Point endPoint, Color startColor, Color endColor) :
            this(startPoint, endPoint, GetGradientStopsFromEdgeColors(startColor, endColor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class that has the specified gradient stops,
        /// start point and end point.
        /// </summary>
        /// <param name="startPoint">The start point of the gradient.</param>
        /// <param name="endPoint">The end point of the gradient.</param>
        /// <param name="gradientStops">The <see cref="GradientStop"/> instances array to set on this brush.</param>
        public LinearGradientBrush(Point startPoint, Point endPoint, GradientStop[] gradientStops) : this(new UI.Native.LinearGradientBrush())
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.gradientStops = gradientStops;

            ReinitializeNativeBrush();
        }

        private LinearGradientBrush(UI.Native.LinearGradientBrush nativeBrush)
        {
            NativeBrush = nativeBrush;
            gradientStops = new GradientStop[0];
        }

        /// <summary>
        /// Gets or sets the starting two-dimensional coordinates of the linear gradient.
        /// </summary>
        public Point StartPoint
        {
            get => startPoint;

            set
            {
                CheckDisposed();

                if (startPoint == value)
                    return;

                startPoint = value;
                ReinitializeNativeBrush();
            }
        }

        /// <summary>
        /// Gets or sets the ending two-dimensional coordinates of the linear gradient.
        /// </summary>
        public Point EndPoint
        {
            get => endPoint;

            set
            {
                CheckDisposed();

                if (endPoint == value)
                    return;

                endPoint = value;
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

        internal UI.Native.LinearGradientBrush NativeBrush { get; private set; }

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
                startPoint,
                endPoint,
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