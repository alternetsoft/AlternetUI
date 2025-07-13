using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Base class for gradient brushes.
    /// </summary>
    public abstract class GradientBrush : Brush
    {
        private GradientStop[] gradientStops;
        private SKShaderTileMode tileMode;
        private SKMatrix localMatrix = SKMatrix.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Brush"/> class.
        /// </summary>
        /// <param name="immutable">Whether this brush is immutable.</param>
        /// <param name="gradientStops">Array of <see cref="GradientStop"/>.</param>
        protected GradientBrush(GradientStop[] gradientStops, bool immutable)
            : base(immutable)
        {
            this.gradientStops = gradientStops;
        }

        /// <inheritdoc/>
        public override Color AsColor => GradientStops.Length > 0 ?
            GradientStops[0].Color : Color.Black;

        /// <summary>
        /// Gets or sets the <see cref="GradientStop"/> instances array defining the color
        /// transition in this brush.
        /// </summary>
        public virtual GradientStop[] GradientStops
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

        /// <summary>
        /// Gets or sets <see cref="SKMatrix"/> for this brush.
        /// </summary>
        public virtual SKMatrix LocalMatrix
        {
            get
            {
                return localMatrix;
            }

            set
            {
                if (localMatrix == value)
                    return;
                CheckDisposed();
                localMatrix = value;
                UpdateRequired = true;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="SKShaderTileMode"/> for this brush.
        /// </summary>
        public virtual SKShaderTileMode TileMode
        {
            get => tileMode;
            set
            {
                if (tileMode == value)
                    return;
                CheckDisposed();
                tileMode = value;
                UpdateRequired = true;
            }
        }

        /// <summary>
        /// Converts array of <see cref="GradientStop"/> to string.
        /// </summary>
        /// <param name="stops">Array of <see cref="GradientStop"/>.</param>
        /// <returns></returns>
        public static string ToString(GradientStop[] stops)
        {
            string result = string.Empty;
            foreach (var item in stops)
            {
                if (result.Length > 0)
                    result += ", ";
                result += item.ToString();
            }

            return result;
        }

        /// <summary>
        /// Converts array of <see cref="GradientStop"/> to array of <see cref="SKColor"/>.
        /// </summary>
        /// <param name="gradientStops">Array of <see cref="GradientStop"/>.</param>
        /// <returns></returns>
        public static SKColor[] ToSkiaGradientColors(GradientStop[] gradientStops)
        {
            var result = Array.ConvertAll(gradientStops, item => (SKColor)item.Color);
            return result;
        }

        /// <summary>
        /// Converts array of <see cref="GradientStop"/> to array of <see cref="Color"/>.
        /// </summary>
        /// <param name="gradientStops">Array of <see cref="GradientStop"/>.</param>
        /// <returns></returns>
        public static Color[] ToGradientColors(GradientStop[] gradientStops)
        {
            var result = Array.ConvertAll(gradientStops, item => item.Color);
            return result;
        }

        /// <summary>
        /// Converts two colors to array of <see cref="GradientStop"/>.
        /// </summary>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        /// <returns></returns>
        public static GradientStop[] GradientStopsFromEdgeColors(
            Color startColor,
            Color endColor)
        {
            return new[]
            {
                new GradientStop(startColor, 0),
                new GradientStop(endColor, 1),
            };
        }

        /// <summary>
        /// Converts array of <see cref="GradientStop"/> to array of gradient offsets.
        /// </summary>
        /// <param name="gradientStops">Array of <see cref="GradientStop"/>.</param>
        /// <returns></returns>
        public static Coord[] ToGradientOffsets(GradientStop[] gradientStops)
        {
            var result = Array.ConvertAll(gradientStops, item => (Coord)item.Offset);
            return result;
        }

        /// <summary>
        /// Converts array of <see cref="GradientStop"/> to array of gradient offsets.
        /// </summary>
        /// <param name="gradientStops">Array of <see cref="GradientStop"/>.</param>
        /// <returns></returns>
        public static float[] ToGradientOffsetsF(GradientStop[] gradientStops)
        {
            var result = Array.ConvertAll(gradientStops, item => (float)item.Offset);
            return result;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hashCode1 = (tileMode, localMatrix).GetHashCode();
            var hashCode2 = MathUtils.SequentialValuesHash(GradientStops);
            return MathUtils.CombineHashCodes(hashCode1, hashCode2);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public override bool Equals(object? other)
        {
            var o = other as GradientBrush;
            if (o == null)
                return false;
            CheckDisposed();

            return
                TileMode == o.TileMode &&
                LocalMatrix == o.LocalMatrix &&
                Enumerable.SequenceEqual(GradientStops, o.GradientStops);
        }

        /// <inheritdoc/>
        protected override SKPaint CreateSkiaPaint()
        {
            var result = base.CreateSkiaPaint();
            result.Shader = CreateSkiaShader();
            return result;
        }

        /// <summary>
        /// Creates <see cref="SKShader"/> for this radial gradient brush.
        /// </summary>
        /// <returns></returns>
        protected abstract SKShader CreateSkiaShader();
    }
}
