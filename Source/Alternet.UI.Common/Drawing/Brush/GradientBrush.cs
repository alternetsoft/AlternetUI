using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private SKMatrix localMatrix = SKMatrix.CreateIdentity();

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

        /// <summary>
        /// Gets the starting color of the gradient, defined by the first stop.
        /// </summary>
        [Browsable(false)]
        public virtual Color StartColor
        {
            get
            {
                var stops = OrderedGradientStops;

                if (stops.Length == 0)
                    return Color.Black;
                return stops[0].Color;
            }
        }

        /// <summary>
        /// Gets the ending color of the gradient, defined by the last stop.
        /// </summary>
        [Browsable(false)]
        public virtual Color EndColor
        {
            get
            {
                var stops = OrderedGradientStops;

                if (stops.Length == 0)
                    return Color.Black;
                return stops[stops.Length - 1].Color;
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override Color AsColor => StartColor;

        /// <summary>
        /// Gets <see cref="GradientStop"/> instances ordered by their offset.
        /// </summary>
        [Browsable(false)]
        public virtual GradientStop[]  OrderedGradientStops
        {
            get
            {
                if (GradientStops == null || GradientStops.Length == 0)
                    return Array.Empty<GradientStop>();
                return GradientStops.OrderBy(s => s.Offset).ToArray();
            }
        }

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
                UpdateRequired();
            }
        }

        /// <summary>
        /// Gets or sets matrix that defines a local geometric transform for this brush.
        /// </summary>
        [Browsable(false)]
        public virtual TransformMatrix Transform
        {
            get => new (LocalMatrix);
            
            set
            {
                LocalMatrix = (SKMatrix)value;
            }
        }

        /// <summary>
        /// Gets or sets matrix that defines a local geometric transform for this brush.
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
                UpdateRequired();
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
                UpdateRequired();
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
        /// <returns>Array of <see cref="SKColor"/>.</returns>
        public static SKColor[] ToSkiaGradientColors(GradientStop[] gradientStops)
        {
            var result = Array.ConvertAll(gradientStops, item => (SKColor)item.Color);
            return result;
        }

        /// <summary>
        /// Converts array of <see cref="GradientStop"/> to array of <see cref="Color"/>.
        /// </summary>
        /// <param name="gradientStops">Array of <see cref="GradientStop"/>.</param>
        /// <returns>Array of <see cref="Color"/>.</returns>
        public static Color[] ToGradientColors(GradientStop[] gradientStops)
        {
            var result = Array.ConvertAll(gradientStops, item => item.Color);
            return result;
        }

        /// <summary>
        /// Converts two colors to array of <see cref="GradientStop"/>.
        /// </summary>
        /// <param name="startColor">The start color.</param>
        /// <param name="endColor">The end color.</param>
        /// <returns>Array of <see cref="GradientStop"/>.</returns>
        public static GradientStop[] GradientStopsFromEdgeColors(Color startColor, Color endColor)
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
        /// <returns>Array of <see cref="Coord"/>.</returns>
        public static Coord[] ToGradientOffsets(GradientStop[] gradientStops)
        {
            var result = Array.ConvertAll(gradientStops, item => (Coord)item.Offset);
            return result;
        }

        /// <summary>
        /// Converts array of <see cref="GradientStop"/> to array of gradient offsets.
        /// </summary>
        /// <param name="gradientStops">Array of <see cref="GradientStop"/>.</param>
        /// <returns>Array of <see cref="float"/>.</returns>
        public static float[] ToGradientOffsetsF(GradientStop[] gradientStops)
        {
            var result = Array.ConvertAll(gradientStops, item => (float)item.Offset);
            return result;
        }

        /// <summary>
        /// Resets the <see cref="Transform"/> property to identity matrix.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetTransform()
        {
            LocalMatrix = SKMatrix.CreateIdentity();
        }

        /// <summary>
        /// Multiply the <see cref="Transform"/> property by the specified matrix.
        /// This method prepends the specified matrix to the transform of this brush.
        /// </summary>
        /// <param name="matrix">The matrix to multiply by.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MultiplyTransform(TransformMatrix matrix) => MultiplyTransform(matrix, MatrixOrder.Prepend);

        /// <summary>
        /// Multiply the <see cref="Transform"/> property by the specified matrix using the specified order.
        /// </summary>
        /// <param name="matrix">The matrix to multiply by.</param>
        /// <param name="order">The order in which to apply the multiplication.</param>
        public virtual void MultiplyTransform(TransformMatrix matrix, MatrixOrder order)
        {
            if (order == MatrixOrder.Prepend)
            {
                Transform = matrix * Transform;
            }
            else
            {
                Transform = Transform * matrix;
            }
        }

        /// <summary>
        /// Translates the local geometric transform by the specified amount. This method prepends the translation to the transform. 
        /// </summary>
        /// <param name="dx">The amount to translate in the x-direction.</param>
        /// <param name="dy">The amount to translate in the y-direction.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TranslateTransform(float dx, float dy) => TranslateTransform(dx, dy, MatrixOrder.Prepend);

        /// <summary>
        /// Translates the local geometric transform by the specified amount using the specified order.
        /// </summary>
        /// <param name="dx">The amount to translate in the x-direction.</param>
        /// <param name="dy">The amount to translate in the y-direction.</param>
        /// <param name="order">The order in which to apply the translation.</param>
        public virtual void TranslateTransform(float dx, float dy, MatrixOrder order)
        {
            var translation = TransformMatrix.CreateTranslation(dx, dy);

            if (order == MatrixOrder.Prepend)
            {
                Transform = translation * Transform;
            }
            else
            {
                Transform *= translation;
            }
        }

        /// <summary>
        /// Scales the local geometric transform by the specified amount. This method prepends the scale to the transform. 
        /// </summary>
        /// <param name="sx">The scale factor in the x-direction.</param>
        /// <param name="sy">The scale factor in the y-direction.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScaleTransform(float sx, float sy) => ScaleTransform(sx, sy, MatrixOrder.Prepend);

        /// <summary>
        /// Scales the local geometric transform by the specified amount using the specified order.
        /// </summary>
        /// <param name="sx">The scale factor in the x-direction.</param>
        /// <param name="sy">The scale factor in the y-direction.</param>
        /// <param name="order">The order in which to apply the scale.</param>
        public virtual void ScaleTransform(float sx, float sy, MatrixOrder order)
        {
            var scale = TransformMatrix.CreateScale(sx, sy);

            if (order == MatrixOrder.Prepend)
            {
                Transform = scale * Transform;
            }
            else
            {
                Transform *= scale;
            }
        }

        /// <summary>
        /// Rotates the local geometric transform by the specified amount. This method prepends the rotation to the transform. 
        /// </summary>
        /// <param name="angle">The angle of rotation.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateTransform(float angle) => RotateTransform(angle, MatrixOrder.Prepend);

        /// <summary>
        /// Rotates the local geometric transform by the specified amount using the specified order of the rotation.
        /// </summary>
        /// <param name="angle">The angle of rotation.</param>
        /// <param name="order">The order in which to apply the rotation.</param>
        public virtual void RotateTransform(float angle, MatrixOrder order)
        {
            var rotation = TransformMatrix.CreateRotation(angle);

            if (order == MatrixOrder.Prepend)
            {
                Transform = rotation * Transform;
            }
            else
            {
                Transform *= rotation;
            }
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
