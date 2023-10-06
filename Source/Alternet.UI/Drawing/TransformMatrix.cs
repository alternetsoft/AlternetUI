using System;

namespace Alternet.Drawing
{
    /// <summary>
    /// Encapsulates a 3-by-2 affine matrix that represents a geometric transform. This class cannot be inherited.
    /// </summary>
    public sealed class TransformMatrix : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformMatrix"/> class as the identity matrix.
        /// </summary>
        public TransformMatrix()
            : this(new UI.Native.TransformMatrix())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformMatrix"/> class with the specified elements.
        /// </summary>
        /// <param name="m11">The value in the first row and first column of the new <see cref="TransformMatrix"/>.</param>
        /// <param name="m12">The value in the first row and second column of the new <see cref="TransformMatrix"/>.</param>
        /// <param name="m21">The value in the second row and first column of the new <see cref="TransformMatrix"/>.</param>
        /// <param name="m22">The value in the second row and second column of the new <see cref="TransformMatrix"/>.</param>
        /// <param name="dx">The value in the third row and first column of the new <see cref="TransformMatrix"/>.</param>
        /// <param name="dy">The value in the third row and second column of the new <see cref="TransformMatrix"/>.</param>
        public TransformMatrix(
            double m11,
            double m12,
            double m21,
            double m22,
            double dx,
            double dy)
            : this()
        {
            NativeMatrix.Initialize(m11, m12, m21, m22, dx, dy);
        }

        internal TransformMatrix(UI.Native.TransformMatrix nativeMatrix)
        {
            NativeMatrix = nativeMatrix;
        }

        /// <summary>
        /// Gets an array of floating-point values that represents the elements of this <see cref="TransformMatrix"/>.
        /// </summary>
        /// <value>Gets an array of floating-point values that represents the elements of this <see cref="TransformMatrix"/>.</value>
        /// <remarks>
        /// The elements m11, m12, m21, m22, dx, and dy of the <see cref="TransformMatrix"/> are represented by the
        /// values in the array in that order.
        /// </remarks>
        public double[] Elements => new[] { M11, M12, M21, M22, DX, DY };

        /// <summary>
        /// Gets or sets the value in the first row and first column of the new <see cref="TransformMatrix"/>.
        /// </summary>
        public double M11
        {
            get
            {
                CheckDisposed();
                return NativeMatrix.M11;
            }

            set
            {
                CheckDisposed();
                NativeMatrix.M11 = value;
            }
        }

        /// <summary>
        /// Gets or sets the value in the first row and second column of the <see cref="TransformMatrix"/>.
        /// </summary>
        public double M12
        {
            get
            {
                CheckDisposed();
                return NativeMatrix.M12;
            }

            set
            {
                CheckDisposed();
                NativeMatrix.M12 = value;
            }
        }

        /// <summary>
        /// Gets or sets the value in the second row and first column of the <see cref="TransformMatrix"/>.
        /// </summary>
        public double M21
        {
            get
            {
                CheckDisposed();
                return NativeMatrix.M21;
            }

            set
            {
                CheckDisposed();
                NativeMatrix.M21 = value;
            }
        }

        /// <summary>
        /// Gets or sets the value in the second row and second column of the <see cref="TransformMatrix"/>.
        /// </summary>
        public double M22
        {
            get
            {
                CheckDisposed();
                return NativeMatrix.M22;
            }

            set
            {
                CheckDisposed();
                NativeMatrix.M22 = value;
            }
        }

        /// <summary>
        /// Gets or sets the x translation value (the dx value, or the element in the third row and first column) of
        /// this <see cref="TransformMatrix"/>.
        /// </summary>
        public double DX
        {
            get
            {
                CheckDisposed();
                return NativeMatrix.DX;
            }

            set
            {
                CheckDisposed();
                NativeMatrix.DX = value;
            }
        }

        /// <summary>
        /// Gets or sets the y translation value (the dy value, or the element in the third row and second column) of
        /// this <see cref="TransformMatrix"/>.
        /// </summary>
        public double DY
        {
            get
            {
                CheckDisposed();
                return NativeMatrix.DY;
            }

            set
            {
                CheckDisposed();
                NativeMatrix.DY = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TransformMatrix"/> is the identity matrix.
        /// </summary>
        /// <value>This property is <see langword="true"/> if this <see cref="TransformMatrix"/> is identity; otherwise,
        /// <see langword="false"/>.</value>
        public bool IsIdentity
        {
            get
            {
                CheckDisposed();
                return NativeMatrix.IsIdentity;
            }
        }

        internal UI.Native.TransformMatrix NativeMatrix { get; private set; }

        /// <summary>
        /// Creates a <see cref="TransformMatrix"/> with the specified translation vector.
        /// </summary>
        /// <param name="offsetX">The x value by which to translate this <see cref="TransformMatrix"/>.</param>
        /// <param name="offsetY">The y value by which to translate this <see cref="TransformMatrix"/>.</param>
        public static TransformMatrix CreateTranslation(double offsetX, double offsetY)
        {
            var matrix = new TransformMatrix();
            matrix.Translate(offsetX, offsetY);
            return matrix;
        }

        /// <summary>
        /// Creates a <see cref="TransformMatrix"/> with the specified scale vector.
        /// </summary>
        /// <param name="scaleX">The value by which to scale this <see cref="TransformMatrix"/> in the x-axis direction.</param>
        /// <param name="scaleY">The value by which to scale this <see cref="TransformMatrix"/> in the y-axis direction.</param>
        public static TransformMatrix CreateScale(double scaleX, double scaleY)
        {
            var matrix = new TransformMatrix();
            matrix.Scale(scaleX, scaleY);
            return matrix;
        }

        /// <summary>
        /// Creates a <see cref="TransformMatrix"/> with clockwise rotation of the specified angle about the origin.
        /// </summary>
        /// <param name="angle">The angle of the clockwise rotation, in degrees.</param>
        public static TransformMatrix CreateRotation(double angle)
        {
            var matrix = new TransformMatrix();
            matrix.Rotate(angle);
            return matrix;
        }

        /// <summary>
        /// Releases all resources used by this <see cref="TransformMatrix"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Resets this <see cref="TransformMatrix"/> to have the elements of the identity matrix.
        /// </summary>
        public void Reset()
        {
            CheckDisposed();
            NativeMatrix.Reset();
        }

        /// <summary>
        /// Multiplies this <see cref="TransformMatrix"/> by the specified <see cref="TransformMatrix"/> by prepending
        /// the specified <see cref="TransformMatrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="TransformMatrix"/> by which this <see cref="TransformMatrix"/> is to be multiplied.</param>
        public void Multiply(TransformMatrix matrix)
        {
            CheckDisposed();
            NativeMatrix.Multiply(matrix.NativeMatrix);
        }

        /// <summary>
        /// Applies the specified translation vector to this <see cref="TransformMatrix"/> by prepending the translation vector.
        /// </summary>
        /// <param name="offsetX">The x value by which to translate this <see cref="TransformMatrix"/>.</param>
        /// <param name="offsetY">The y value by which to translate this <see cref="TransformMatrix"/>.</param>
        public void Translate(double offsetX, double offsetY)
        {
            CheckDisposed();
            NativeMatrix.Translate(offsetX, offsetY);
        }

        /// <summary>
        /// Applies the specified scale vector to this <see cref="TransformMatrix"/> by prepending the scale vector.
        /// </summary>
        /// <param name="scaleX">The value by which to scale this <see cref="TransformMatrix"/> in the x-axis direction.</param>
        /// <param name="scaleY">The value by which to scale this <see cref="TransformMatrix"/> in the y-axis direction.</param>
        public void Scale(double scaleX, double scaleY)
        {
            CheckDisposed();
            NativeMatrix.Scale(scaleX, scaleY);
        }

        /// <summary>
        /// Applies a clockwise rotation of the specified angle about the origin to this <see cref="TransformMatrix"/>.
        /// </summary>
        /// <param name="angle">The angle of the clockwise rotation, in degrees.</param>
        public void Rotate(double angle)
        {
            CheckDisposed();
            NativeMatrix.Rotate(angle);
        }

        /// <summary>
        /// Inverts this Matrix, if it is invertible.
        /// </summary>
        public void Invert()
        {
            CheckDisposed();
            NativeMatrix.Invert();
        }

        /// <summary>
        /// Applies the geometric transform this <see cref="TransformMatrix"/> represents to a point.
        /// </summary>
        /// <param name="point">A <see cref="Point"/> to transform.</param>
        public Point TransformPoint(Point point)
        {
            CheckDisposed();
            return NativeMatrix.TransformPoint(point);
        }

        /// <summary>
        /// Applies the geometric transform this <see cref="TransformMatrix"/> represents to a size.
        /// </summary>
        /// <param name="size">A <see cref="Size"/> to transform.</param>
        public Size TransformSize(Size size)
        {
            CheckDisposed();
            return NativeMatrix.TransformSize(size);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            CheckDisposed();

            if (obj is not TransformMatrix matrix2)
                return false;

            return NativeMatrix.IsEqualTo(matrix2.NativeMatrix);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return NativeMatrix.GetHashCode_();
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if the object has been disposed.
        /// </summary>
        private void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativeMatrix.Dispose();
                    NativeMatrix = null!;
                }

                isDisposed = true;
            }
        }
    }
}