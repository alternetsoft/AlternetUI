using System;
using System.Linq;

namespace Alternet.Drawing
{
    /// <summary>
    /// Paints an area with a linear gradient.
    /// </summary>
    public sealed class LinearGradientBrush : Brush
    {
        private Point startPoint;
        private Point endPoint;
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
            : this(new Point(), new Point(1, 1), gradientStops)
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
        public LinearGradientBrush(Point startPoint, Point endPoint, Color startColor, Color endColor)
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
        public LinearGradientBrush(Point startPoint, Point endPoint, GradientStop[] gradientStops)
            : this(new UI.Native.LinearGradientBrush())
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.gradientStops = gradientStops;

            ReinitializeNativeBrush();
        }

        private LinearGradientBrush(UI.Native.LinearGradientBrush nativeBrush)
            : base(nativeBrush, false)
        {
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
                if (startPoint == value)
                    return;
                CheckDisposed();
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
                if (endPoint == value)
                    return;
                CheckDisposed();
                endPoint = value;
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
        public override BrushType BrushType => BrushType.LinearGradient;

        internal override Color BrushColor => GradientStops.Length > 0 ?
            GradientStops[0].Color : Color.Black;

        internal new UI.Native.LinearGradientBrush NativeBrush =>
            (UI.Native.LinearGradientBrush)base.NativeBrush;

        internal static GradientStop[] GetGradientStopsFromEdgeColors(
            Color startColor,
            Color endColor) => new[]
            {
                new GradientStop(startColor, 0),
                new GradientStop(endColor, 1),
            };

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

        private protected override bool EqualsCore(Brush other)
        {
            var o = other as LinearGradientBrush;
            if (o == null)
                return false;

            return
                StartPoint == o.StartPoint &&
                EndPoint == o.EndPoint &&
                Enumerable.SequenceEqual(GradientStops, o.GradientStops);
        }

        private protected override int GetHashCodeCore()
        {
            var hashCode = new HashCode();
            hashCode.Add(StartPoint);
            hashCode.Add(EndPoint);
            foreach (var gradientStop in GradientStops)
                hashCode.Add(gradientStop);

            return hashCode.ToHashCode();
        }

        private protected override string ToStringCore()
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

        private void ReinitializeNativeBrush()
        {
            NativeBrush.Initialize(
                startPoint,
                endPoint,
                gradientStops.Select(x => x.Color).ToArray(),
                gradientStops.Select(x => x.Offset).ToArray());
        }
    }
}