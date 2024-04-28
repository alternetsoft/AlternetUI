using System;
using System.Runtime.CompilerServices;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Encapsulates a 3-by-2 affine matrix that represents a geometric transform.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class TransformMatrix : DisposableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransformMatrix"/> class
        /// as the identity matrix.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TransformMatrix()
            : this(NativeDrawing.Default.CreateTransformMatrix())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformMatrix"/> class
        /// with the specified elements.
        /// </summary>
        /// <param name="m11">The value in the first row and first column
        /// of the new <see cref="TransformMatrix"/>.</param>
        /// <param name="m12">The value in the first row and second column of the
        /// new <see cref="TransformMatrix"/>.</param>
        /// <param name="m21">The value in the second row and first column of the
        /// new <see cref="TransformMatrix"/>.</param>
        /// <param name="m22">The value in the second row and second column of the
        /// new <see cref="TransformMatrix"/>.</param>
        /// <param name="dx">The value in the third row and first column of the
        /// new <see cref="TransformMatrix"/>.</param>
        /// <param name="dy">The value in the third row and second column of the
        /// new <see cref="TransformMatrix"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TransformMatrix(
            double m11,
            double m12,
            double m21,
            double m22,
            double dx,
            double dy)
            : this()
        {
            NativeDrawing.Default.UpdateTransformMatrix(
                NativeObject,
                m11,
                m12,
                m21,
                m22,
                dx,
                dy);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformMatrix"/> class.
        /// </summary>
        /// <param name="nativeMatrix">Native transform matrix instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TransformMatrix(object nativeMatrix)
        {
            NativeObject = nativeMatrix;
        }

        /// <summary>
        /// Gets an array of floating-point values that represents the
        /// elements of this <see cref="TransformMatrix"/>.
        /// </summary>
        /// <value>Gets an array of floating-point values that represents
        /// the elements of this <see cref="TransformMatrix"/>.</value>
        /// <remarks>
        /// The elements m11, m12, m21, m22, dx, and dy of the
        /// <see cref="TransformMatrix"/> are represented by the
        /// values in the array in that order.
        /// </remarks>
        public double[] Elements
        {
            get => new[] { M11, M12, M21, M22, DX, DY };
            set
            {
                NativeDrawing.Default.UpdateTransformMatrix(
                    NativeObject,
                    value[0],
                    value[1],
                    value[2],
                    value[3],
                    value[4],
                    value[5]);
            }
        }

        /// <summary>
        /// Gets or sets the value in the first row and first column of the
        /// new <see cref="TransformMatrix"/>.
        /// </summary>
        public double M11
        {
            get
            {
                CheckDisposed();
                return NativeDrawing.Default.GetTransformMatrixM11(NativeObject);
            }

            set
            {
                CheckDisposed();
                NativeDrawing.Default.SetTransformMatrixM11(NativeObject, value);
            }
        }

        /// <summary>
        /// Gets or sets the value in the first row and second column of
        /// the <see cref="TransformMatrix"/>.
        /// </summary>
        public double M12
        {
            get
            {
                CheckDisposed();
                return NativeDrawing.Default.GetTransformMatrixM12(NativeObject);
            }

            set
            {
                CheckDisposed();
                NativeDrawing.Default.SetTransformMatrixM12(NativeObject, value);
            }
        }

        /// <summary>
        /// Gets or sets the value in the second row and first column of
        /// the <see cref="TransformMatrix"/>.
        /// </summary>
        public double M21
        {
            get
            {
                CheckDisposed();
                return NativeDrawing.Default.GetTransformMatrixM21(NativeObject);
            }

            set
            {
                CheckDisposed();
                NativeDrawing.Default.SetTransformMatrixM21(NativeObject, value);
            }
        }

        /// <summary>
        /// Gets or sets the value in the second row and second column
        /// of the <see cref="TransformMatrix"/>.
        /// </summary>
        public double M22
        {
            get
            {
                CheckDisposed();
                return NativeDrawing.Default.GetTransformMatrixM22(NativeObject);
            }

            set
            {
                CheckDisposed();
                NativeDrawing.Default.SetTransformMatrixM22(NativeObject, value);
            }
        }

        /// <summary>
        /// Gets or sets the x translation value (the dx value, or the element
        /// in the third row and first column) of
        /// this <see cref="TransformMatrix"/>.
        /// </summary>
        public double DX
        {
            get
            {
                CheckDisposed();
                return NativeDrawing.Default.GetTransformMatrixDX(NativeObject);
            }

            set
            {
                CheckDisposed();
                NativeDrawing.Default.SetTransformMatrixDX(NativeObject, value);
            }
        }

        /// <summary>
        /// Gets or sets the y translation value (the dy value, or the
        /// element in the third row and second column) of
        /// this <see cref="TransformMatrix"/>.
        /// </summary>
        public double DY
        {
            get
            {
                CheckDisposed();
                return NativeDrawing.Default.GetTransformMatrixDY(NativeObject);
            }

            set
            {
                CheckDisposed();
                NativeDrawing.Default.SetTransformMatrixDY(NativeObject, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TransformMatrix"/>
        /// is the identity matrix.
        /// </summary>
        /// <value>This property is <see langword="true"/> if this
        /// <see cref="TransformMatrix"/> is identity; otherwise,
        /// <see langword="false"/>.</value>
        public bool IsIdentity
        {
            get
            {
                CheckDisposed();
                return NativeDrawing.Default.GetTransformMatrixIsIdentity(NativeObject);
            }
        }

        /// <summary>
        /// Gets native matrix.
        /// </summary>
        public object NativeObject { get; private set; }

        /// <summary>
        /// Creates a <see cref="TransformMatrix"/> with the specified translation vector.
        /// </summary>
        /// <param name="offsetX">The x value by which to translate
        /// this <see cref="TransformMatrix"/>.</param>
        /// <param name="offsetY">The y value by which to translate
        /// this <see cref="TransformMatrix"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TransformMatrix CreateTranslation(double offsetX, double offsetY)
        {
            var matrix = new TransformMatrix();
            matrix.Translate(offsetX, offsetY);
            return matrix;
        }

        /// <summary>
        /// Creates a <see cref="TransformMatrix"/> with the specified scale vector.
        /// </summary>
        /// <param name="scaleX">The value by which to scale
        /// this <see cref="TransformMatrix"/> in the x-axis direction.</param>
        /// <param name="scaleY">The value by which to scale
        /// this <see cref="TransformMatrix"/> in the y-axis direction.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TransformMatrix CreateScale(double scaleX, double scaleY)
        {
            var matrix = new TransformMatrix();
            matrix.Scale(scaleX, scaleY);
            return matrix;
        }

        /// <summary>
        /// Creates a <see cref="TransformMatrix"/> with clockwise rotation of the specified
        /// angle about the origin.
        /// </summary>
        /// <param name="angle">The angle of the clockwise rotation, in degrees.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TransformMatrix CreateRotation(double angle)
        {
            var matrix = new TransformMatrix();
            matrix.Rotate(angle);
            return matrix;
        }

        /// <summary>
        /// Resets this <see cref="TransformMatrix"/> to have the elements of the identity matrix.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            CheckDisposed();
            NativeDrawing.Default.ResetTransformMatrix(NativeObject);
        }

        /// <summary>
        /// Multiplies this <see cref="TransformMatrix"/> by the specified
        /// <see cref="TransformMatrix"/> by prepending
        /// the specified <see cref="TransformMatrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="TransformMatrix"/> by
        /// which this <see cref="TransformMatrix"/> is to be multiplied.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Multiply(TransformMatrix matrix)
        {
            CheckDisposed();
            NativeDrawing.Default.MultiplyTransformMatrix(NativeObject, matrix.NativeObject);
        }

        /// <summary>
        /// Applies the specified translation vector to
        /// this <see cref="TransformMatrix"/> by prepending the translation vector.
        /// </summary>
        /// <param name="offsetX">The x value by which to
        /// translate this <see cref="TransformMatrix"/>.</param>
        /// <param name="offsetY">The y value by which to
        /// translate this <see cref="TransformMatrix"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(double offsetX, double offsetY)
        {
            CheckDisposed();
            NativeDrawing.Default.TranslateTransformMatrix(NativeObject, offsetX, offsetY);
        }

        /// <summary>
        /// Applies the specified scale vector to
        /// this <see cref="TransformMatrix"/> by prepending the scale vector.
        /// </summary>
        /// <param name="scaleX">The value by which to
        /// scale this <see cref="TransformMatrix"/> in the x-axis direction.</param>
        /// <param name="scaleY">The value by which to
        /// scale this <see cref="TransformMatrix"/> in the y-axis direction.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(double scaleX, double scaleY)
        {
            CheckDisposed();
            NativeDrawing.Default.ScaleTransformMatrix(NativeObject, scaleX, scaleY);
        }

        /// <summary>
        /// Applies a clockwise rotation of the specified angle about the
        /// origin to this <see cref="TransformMatrix"/>.
        /// </summary>
        /// <param name="angle">The angle of the clockwise rotation, in degrees.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(double angle)
        {
            CheckDisposed();
            NativeDrawing.Default.RotateTransformMatrix(NativeObject, angle);
        }

        /// <summary>
        /// Inverts this matrix, if it is invertible.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invert()
        {
            CheckDisposed();
            NativeDrawing.Default.InvertTransformMatrix(NativeObject);
        }

        /// <summary>
        /// Applies the geometric transform this <see cref="TransformMatrix"/> represents to a point.
        /// </summary>
        /// <param name="point">A <see cref="PointD"/> to transform.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointD TransformPoint(PointD point)
        {
            CheckDisposed();
            return NativeDrawing.Default.TransformMatrixOnPoint(NativeObject, point);
        }

        /// <summary>
        /// Applies the geometric transform this <see cref="TransformMatrix"/> represents to a size.
        /// </summary>
        /// <param name="size">A <see cref="SizeD"/> to transform.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD TransformSize(SizeD size)
        {
            CheckDisposed();
            return NativeDrawing.Default.TransformMatrixOnSize(NativeObject, size);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is not TransformMatrix matrix2)
                return false;
            CheckDisposed();
            return NativeDrawing.Default.TransformMatrixEquals(NativeObject, matrix2.NativeObject);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return NativeDrawing.Default.TransformMatrixGetHashCode(NativeObject);
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            if (NativeObject is not null)
            {
                ((IDisposable)NativeObject).Dispose();
                NativeObject = null!;
            }
        }
    }
}