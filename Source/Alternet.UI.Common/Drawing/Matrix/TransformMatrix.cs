using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /*
    SKMatrix:
    https://learn.microsoft.com/en-us/previous-versions/xamarin/xamarin-forms/user-interface/graphics/skiasharp/transforms/matrix

    ScaleX  SkewY   Perspective0
    SkewX   ScaleY  Perspective1
    TransX  TransY  Perspective2

    TransformMatrix:
    https://docs.wxwidgets.org/3.0/classwx_affine_matrix2_d.html

    m_11  m_12   0
    m_21  m_22   0
    m_tx  m_ty   1

    m11 - ScaleX - The value in the first row and first column.
    m12 - SkewY - The value in the first row and second column.
    m21 - SkewX - The value in the second row and first column.
    m22 - ScaleY - The value in the second row and second column.
    dx - TransX - The value in the third row and first column.
    dy - TransY - The value in the third row and second column.
    */

    /// <summary>
    /// Encapsulates a 3-by-2 affine matrix that represents a geometric transform.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public partial struct TransformMatrix : IEquatable<TransformMatrix>
    {
        /// <summary>
        /// Gets or sets the scaling in the x-direction. This is the same as <see cref="ScaleX"/>.
        /// This is the value in the first row and first column of the matrix.
        /// </summary>
        [FieldOffset(FieldOffsetM11)]
        public Coord M11;

        /// <summary>
        /// Gets or sets the skew in the y-direction.
        /// This is the value in the first row and second column of the matrix.
        /// This is the same as <see cref="SkewY"/>.
        /// </summary>
        [FieldOffset(FieldOffsetM12)]
        public Coord M12;

        /// <summary>
        /// Gets or sets the skew in the x-direction.
        /// This is the value in the second row and first column of the matrix.
        /// This is the same as <see cref="SkewX"/>.
        /// </summary>
        [FieldOffset(FieldOffsetM21)]
        public Coord M21;

        /// <summary>
        /// Gets or sets the scaling in the y-direction.
        /// This is the value in the second row and second column of the matrix.
        /// This is the same as <see cref="ScaleY"/>.
        /// </summary>
        [FieldOffset(FieldOffsetM22)]
        public Coord M22;

        /// <summary>
        /// Get or sets the translation in the x-direction (the dx value, or the element
        /// in the third row and first column) of the matrix. This is the same as <see cref="TransX"/>.
        /// </summary>
        [FieldOffset(FieldOffsetDX)]
        public Coord DX;

        /// <summary>
        /// Get or sets the translation in the y-direction (the dy value, or the
        /// element in the third row and second column) of the matrix.
        /// This is the same as <see cref="TransY"/>.
        /// </summary>
        [FieldOffset(FieldOffsetDY)]
        public Coord DY;

        /* Alternative field names */

        /// <summary>
        /// Gets or sets the scaling in the x-direction. This is the same as <see cref="M11"/>.
        /// This is the value in the first row and first column of the matrix.
        /// </summary>
        [FieldOffset(FieldOffsetM11)]
        public Coord ScaleX;

        /// <summary>
        /// Gets or sets the skew in the x-direction. This is the same as <see cref="M21"/>.
        /// This is the value in the second row and first column in the matrix.
        /// </summary>
        [FieldOffset(FieldOffsetM21)]
        public Coord SkewX;

        /// <summary>
        /// Get or sets the translation in the x-direction.
        /// This is the same as <see cref="DX"/>.
        /// This is the value in the third row and first column in the matrix.
        /// </summary>
        [FieldOffset(FieldOffsetDX)]
        public Coord TransX;

        /// <summary>
        /// Gets or sets the skew in the y-direction.
        /// This is the value in the first row and second column in the matrix.
        /// This is the same as <see cref="M12"/>.
        /// </summary>
        [FieldOffset(FieldOffsetM12)]
        public Coord SkewY;

        /// <summary>
        /// Gets or sets the scaling in the y-direction.
        /// This is the same as <see cref="M22"/>.
        /// This is the value in the second row and second column in the matrix.
        /// </summary>
        [FieldOffset(FieldOffsetM22)]
        public Coord ScaleY;

        /// <summary>
        /// Get or sets the translation in the y-direction.
        /// This is the same as <see cref="DY"/>.
        /// This is the value in the third row and second column in the matrix.
        /// </summary>
        [FieldOffset(FieldOffsetDY)]
        public Coord TransY;

        private const int SizeOf = sizeof(Coord);
        private const int FieldOffsetM11 = 0;
        private const int FieldOffsetM12 = SizeOf;
        private const int FieldOffsetM21 = SizeOf * 2;
        private const int FieldOffsetM22 = SizeOf * 3;
        private const int FieldOffsetDX = SizeOf * 4;
        private const int FieldOffsetDY = SizeOf * 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformMatrix"/> class
        /// as the identity matrix.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TransformMatrix()
        {
            Reset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformMatrix"/> class
        /// with the specified elements.
        /// </summary>
        /// <param name="m11">The scaling in the x-direction.</param>
        /// <param name="m12">The scaling in the y-direction.</param>
        /// <param name="m21">The skew in the x-direction.</param>
        /// <param name="m22">The skew in the y-direction.</param>
        /// <param name="dx">The translation in the x-direction.</param>
        /// <param name="dy">The translation in the x-direction.</param>
        public TransformMatrix(
            Coord m11,
            Coord m12,
            Coord m21,
            Coord m22,
            Coord dx,
            Coord dy)
        {
            this.M11 = m11;
            this.M12 = m12;
            this.M21 = m21;
            this.M22 = m22;
            this.DX = dx;
            this.DY = dy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformMatrix"/> class
        /// using data from the specified matrix.
        /// </summary>
        /// <param name="matrix"></param>
        public TransformMatrix(TransformMatrix matrix)
        {
            Assign(matrix);
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
        public Coord[] Elements
        {
            readonly get => new[] { M11, M12, M21, M22, DX, DY };
            set
            {
                M11 = value[0];
                M12 = value[1];
                M21 = value[2];
                M22 = value[3];
                DX = value[4];
                DY = value[5];
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object is mirrored along either the X or Y axis.
        /// </summary>
        public readonly bool IsMirrored => IsMirroredX || IsMirroredY;

        /// <summary>
        /// Gets a value indicating whether the object is mirrored along the X-axis.
        /// </summary>
        public readonly bool IsMirroredX => M11 < 0;

        /// <summary>
        /// Gets a value indicating whether the transformation includes a vertical mirroring.
        /// </summary>
        public readonly bool IsMirroredY => M22 < 0;

        /// <summary>
        /// Gets a value indicating whether this <see cref="TransformMatrix"/>
        /// is the identity matrix.
        /// </summary>
        /// <value>This property is <see langword="true"/> if this
        /// <see cref="TransformMatrix"/> is identity; otherwise,
        /// <see langword="false"/>.</value>
        public readonly bool IsIdentity
        {
            get
            {
                return M11 == 1d && M12 == 0d &&
                       M21 == 0d && M22 == 1d &&
                       DX == 0d && DY == 0d;
            }
        }

        /// <summary>
        /// Gets the determinant of this matrix.
        /// </summary>
        [Browsable(false)]
        public readonly Coord Determinant
        {
            get
            {
                return (M11 * M22) - (M12 * M21);
            }
        }

        /// <summary>
        /// Converts <see cref="SKMatrix"/> to <see cref="TransformMatrix"/>.
        /// </summary>
        /// <param name="m">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator TransformMatrix(SKMatrix m)
        {
            var result = new TransformMatrix(m.ScaleX, m.SkewY, m.SkewX, m.ScaleY, m.TransX, m.TransY);
            return result;
        }

        /// <summary>
        /// Converts <see cref="TransformMatrix"/> to <see cref="SKMatrix"/>.
        /// </summary>
        /// <param name="m">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator SKMatrix(TransformMatrix m)
        {
            var result = new SKMatrix(
                (float)m.ScaleX,
                (float)m.SkewX,
                (float)m.TransX,
                (float)m.SkewY,
                (float)m.ScaleY,
                (float)m.TransY,
                0,
                0,
                1);
            return result;
        }

        /// <summary>
        /// Tests whether <see cref="TransformMatrix"/> and <see cref="TransformMatrix"/>
        /// structures are different.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(TransformMatrix left, TransformMatrix right) => !(left == right);

        /// <summary>
        /// Tests whether two specified <see cref="TransformMatrix"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="TransformMatrix"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="TransformMatrix"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="TransformMatrix"/> structures
        /// are equal; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(TransformMatrix left, TransformMatrix right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Creates a <see cref="TransformMatrix"/> with the specified translation vector.
        /// </summary>
        /// <param name="offsetX">The x value by which to translate
        /// this <see cref="TransformMatrix"/>.</param>
        /// <param name="offsetY">The y value by which to translate
        /// this <see cref="TransformMatrix"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TransformMatrix CreateTranslation(Coord offsetX, Coord offsetY)
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
        public static TransformMatrix CreateScale(Coord scaleX, Coord scaleY)
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
        public static TransformMatrix CreateRotation(Coord angle)
        {
            var matrix = new TransformMatrix();
            matrix.Rotate(angle);
            return matrix;
        }

        /// <summary>
        /// Resets this <see cref="TransformMatrix"/> to have the elements of the identity matrix.
        /// </summary>
        public void Reset()
        {
            M11 = 1d;
            M12 = 0d;
            M21 = 0d;
            M22 = 1d;
            DX = 0d;
            DY = 0d;
        }

        /// <summary>
        /// Multiplies this <see cref="TransformMatrix"/> by the specified
        /// <see cref="TransformMatrix"/> by prepending
        /// the specified <see cref="TransformMatrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="TransformMatrix"/> by
        /// which this <see cref="TransformMatrix"/> is to be multiplied.</param>
        public void Multiply(TransformMatrix matrix)
        {
            /*
            concatenates the matrix
            | t.m_11  t.m_12  0 |   | m_11  m_12   0 |
            | t.m_21  t.m_22  0 | x | m_21  m_22   0 |
            | t.m_tx  t.m_ty  1 |   | m_tx  m_ty   1 |
            */

            var matrixDX = matrix.DX;
            var matrixDY = matrix.DY;
            var matrixM11 = matrix.M11;
            var matrixM12 = matrix.M12;
            var matrixM21 = matrix.M21;
            var matrixM22 = matrix.M22;

            DX += (matrixDX * M11) + (matrixDY * M21);
            DY += (matrixDX * M12) + (matrixDY * M22);
            var e11 = (matrixM11 * M11) + (matrixM12 * M21);
            var e12 = (matrixM11 * M12) + (matrixM12 * M22);
            var e21 = (matrixM21 * M11) + (matrixM22 * M21);
            M22 = (matrixM21 * M12) + (matrixM22 * M22);
            M11 = e11;
            M12 = e12;
            M21 = e21;
        }

        /// <summary>
        /// Applies the specified translation vector to
        /// this <see cref="TransformMatrix"/> by prepending the translation vector.
        /// </summary>
        /// <param name="offsetX">The x value by which to
        /// translate this <see cref="TransformMatrix"/>.</param>
        /// <param name="offsetY">The y value by which to
        /// translate this <see cref="TransformMatrix"/>.</param>
        public void Translate(Coord offsetX, Coord offsetY)
        {
            /*
            add the translation to this matrix
            |  1   0   0 |   | m_11  m_12   0 |
            |  0   1   0 | x | m_21  m_22   0 |
            | dx  dy   1 |   | m_tx  m_ty   1 |
            */

            DX += (M11 * offsetX) + (M21 * offsetY);
            DY += (M12 * offsetX) + (M22 * offsetY);
        }

        /// <summary>
        /// Applies the specified scale vector to
        /// this <see cref="TransformMatrix"/> by prepending the scale vector.
        /// </summary>
        /// <param name="scaleX">The value by which to
        /// scale this <see cref="TransformMatrix"/> in the x-axis direction.</param>
        /// <param name="scaleY">The value by which to
        /// scale this <see cref="TransformMatrix"/> in the y-axis direction.</param>
        public void Scale(Coord scaleX, Coord scaleY)
        {
            /*
            // add the scale to this matrix
            // | xScale   0      0 |   | m_11  m_12   0 |
            // |   0    yScale   0 | x | m_21  m_22   0 |
            // |   0      0      1 |   | m_tx  m_ty   1 |
            */

            M11 *= scaleX;
            M12 *= scaleX;
            M21 *= scaleY;
            M22 *= scaleY;
        }

        /// <summary>
        /// Applies a clockwise rotation of the specified angle (degrees) about the
        /// origin to this <see cref="TransformMatrix"/>.
        /// </summary>
        /// <param name="angle">The angle of the clockwise rotation, in degrees.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(Coord angle)
        {
            angle %= 360.0; // Doing the modulo before converting to radians reduces total error
            RotateRadians(angle * MathUtils.DegToRad);
        }

        /// <summary>
        /// Applies a clockwise rotation of the specified angle (radians) about the
        /// origin to this <see cref="TransformMatrix"/>.
        /// </summary>
        /// <param name="angleRadians">The angle of the clockwise rotation, in radians.</param>
        public void RotateRadians(Coord angleRadians)
        {
            /*
            // add the rotation to this matrix (clockwise, radians)
            // | cos    sin   0 |   | m_11  m_12   0 |
            // | -sin   cos   0 | x | m_21  m_22   0 |
            // |  0      0    1 |   | m_tx  m_ty   1 |
            */

            var c = Math.Cos(angleRadians);
            var s = Math.Sin(angleRadians);

            var e11 = (c * M11) + (s * M21);
            var e12 = (c * M12) + (s * M22);
            M21 = (c * M21) - (s * M11);
            M22 = (c * M22) - (s * M12);
            M11 = e11;
            M12 = e12;
        }

        /// <summary>
        /// Inverts this matrix, if it is invertible.
        /// </summary>
        public bool Invert()
        {
            /*
            makes this its inverse matrix.
            Invert
            | m_11  m_12   0 |
            | m_21  m_22   0 |
            | m_tx  m_ty   1 |
            */

            var det = Determinant;

            if (det == 0)
                return false;

            var ex = ((M21 * DY) - (M22 * DX)) / det;
            DY = ((-M11 * DY) + (M12 * DX)) / det;
            DX = ex;
            var e11 = M22 / det;
            M12 = -M12 / det;
            M21 = -M21 / det;
            M22 = M11 / det;
            M11 = e11;

            return true;
        }

        /// <summary>
        /// Applies the geometric transform this <see cref="TransformMatrix"/> represents to a point.
        /// </summary>
        /// <param name="src">A <see cref="PointD"/> to transform.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly PointD TransformPoint(PointD src)
        {
            /*
            applies that matrix to the point
                                      | m_11  m_12   0 |
            | src.m_x  src._my  1 | x | m_21  m_22   0 |
                                      | m_tx  m_ty   1 |
            */

            if (IsIdentity)
                return src;

            var x = (src.X * M11) + (src.Y * M21) + DX;
            var y = (src.X * M12) + (src.Y * M22) + DY;

            return new(x, y);
        }

        /// <summary>
        /// Creates a copy of this matrix.
        /// </summary>
        /// <returns></returns>
        public readonly TransformMatrix Clone()
        {
            return new(this);
        }

        /// <summary>
        /// Assigns matrix values with data from the specified matrix;
        /// </summary>
        /// <param name="matrix"></param>
        public void Assign(TransformMatrix matrix)
        {
            M11 = matrix.M11;
            M12 = matrix.M12;
            M21 = matrix.M21;
            M22 = matrix.M22;
            DX = matrix.DX;
            DY = matrix.DY;
        }

        /// <summary>
        /// Applies the geometric transform this <see cref="TransformMatrix"/> represents to a size.
        /// </summary>
        /// <param name="src">A <see cref="SizeD"/> to transform.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly SizeD TransformSize(SizeD src)
        {
            /*
            applies the matrix except for translations
                                      | m_11  m_12   0 |
            | src.m_x  src._my  0 | x | m_21  m_22   0 |
                                      | m_tx  m_ty   1 |
            */

            if (IsIdentity)
                return src;

            var width = (src.Width * M11) + (src.Height * M21);
            var height = (src.Width * M12) + (src.Height * M22);

            return new(width, height);
        }

        /// <summary>
        /// Applies the geometric transform this <see cref="TransformMatrix"/> represents to a rectangle.
        /// </summary>
        /// <param name="rect">A <see cref="RectD"/> to transform.</param>
        public readonly RectD TransformRect(RectD rect)
        {
            return new(TransformPoint(rect.Location), TransformSize(rect.Size));
        }

        /// <summary>
        /// Gets whether this matrix equals another matrix.
        /// </summary>
        /// <param name="matrix">Other matrix.</param>
        /// <returns></returns>
        public readonly bool Equals(TransformMatrix matrix)
        {
            var result =
                M11 == matrix.M11 &&
                M12 == matrix.M12 &&
                M21 == matrix.M21 &&
                M22 == matrix.M22 &&
                DX == matrix.DX &&
                DY == matrix.DY;
            return result;
        }

        /// <inheritdoc/>
        public readonly override bool Equals(object? obj)
        {
            if (obj is not TransformMatrix matrix)
                return false;
            return Equals(matrix);
        }

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            return (M11, M12, M21, M22, DX, DY).GetHashCode();
        }

        /// <summary>
        /// Determines whether the current transformation matrix represents an identity rotation.
        /// </summary>
        /// <remarks>
        /// This method checks the rotational components <c>M11</c> and <c>M21</c> of the matrix.
        /// If <c>M11</c> is approximately <c>1</c> and <c>M21</c> is approximately <c>0</c>,
        /// the matrix is considered to have no rotation.
        /// </remarks>
        /// <returns>
        /// <c>true</c> if the matrix has no rotational transformation; otherwise, <c>false</c>.
        /// </returns>
        public readonly bool IsRotationIdentity()
        {
            const Coord epsilon = 0.00001d;

            bool isRotationIdentity =
                Math.Abs(M11 - 1d) < epsilon &&
                Math.Abs(M21) < epsilon;
            return isRotationIdentity;
        }

        /// <summary>
        /// Extracts the clockwise rotation angle in radians from the transform matrix.
        /// Compatible with wxGraphicsContext::DrawText angle parameter.
        /// </summary>
        /// <returns>
        /// The rotation angle in radians. Returns 0 if the matrix has no rotation.
        /// </returns>
        public readonly double GetRotationAngleInRadians()
        {
            if(IsRotationIdentity())
                return 0d;

            // Extract angle using clockwise convention
            double angleRadians = Math.Atan2(M21, M11);

            // Normalize to [0, 2*pi) if needed
            if (angleRadians < 0d)
                angleRadians += 2d * Math.PI;

            return angleRadians;
        }

        /// <summary>
        /// Extracts the rotation angle in degrees from this matrix.
        /// Returns 0 if there's no rotational component (identity rotation).
        /// </summary>
        /// <returns>The rotation angle in degrees.</returns>
        public readonly Coord GetRotationAngleInDegrees()
        {
            Coord angleRadians = GetRotationAngleInRadians();

            if (angleRadians == 0d)
                return 0d;

            Coord angleDegrees = MathUtils.ToDegrees(angleRadians);

            return angleDegrees;
        }
    }
}