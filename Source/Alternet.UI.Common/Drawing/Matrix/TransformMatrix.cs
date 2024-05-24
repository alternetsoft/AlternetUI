using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Alternet.UI;

namespace Alternet.Drawing
{
    /*
    https://learn.microsoft.com/en-us/previous-versions/xamarin/xamarin-forms/user-interface/graphics/skiasharp/transforms/matrix
    
    ScaleX  SkewY   Persp0
    SkewX   ScaleY  Persp1
    TransX  TransY  Persp2

    https://docs.wxwidgets.org/3.0/classwx_affine_matrix2_d.html

    m_11  m_12   0
    m_21  m_22   0
    m_tx  m_ty   1

        m11 - The value in the first row and first column.
        m12 - The value in the first row and second column.
        m21 - The value in the second row and first column.
        m22 - The value in the second row and second column.
        dx - The value in the third row and first column.
        dy - The value in the third row and second column.

    */

    /// <summary>
    /// Encapsulates a 3-by-2 affine matrix that represents a geometric transform.
    /// This class cannot be inherited.
    /// </summary>
    public partial class TransformMatrix : BaseObject
    {
        double m11;
        double m12;
        double m21;
        double m22;
        double dx;
        double dy;

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
        /// <param name="m11">The value in the first row and first column.</param>
        /// <param name="m12">The value in the first row and second column.</param>
        /// <param name="m21">The value in the second row and first column.</param>
        /// <param name="m22">The value in the second row and second column.</param>
        /// <param name="dx">The value in the third row and first column.</param>
        /// <param name="dy">The value in the third row and second column.</param>
        public TransformMatrix(
            double m11,
            double m12,
            double m21,
            double m22,
            double dx,
            double dy)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
            this.dx = dx;
            this.dy = dy;
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
            get => new[] { m11, m12, m21, m22, dx, dy };
            set
            {
                m11 = value[0];
                m12 = value[1];
                m21 = value[2];
                m22 = value[3];
                dx = value[4];
                dy = value[5];
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
                return m11;
            }

            set
            {
                m11 = value;
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
                return m12;
            }

            set
            {
                m12 = value;
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
                return m21;
            }

            set
            {
                m21 = value;
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
                return m22;
            }

            set
            {
                m22 = value;
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
                return dx;
            }

            set
            {
                dx = value;
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
                return dy;
            }

            set
            {
                dy = value;
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
                return m11 == 1d && m12 == 0d &&
                       m21 == 0d && m22 == 1d &&
                       dx == 0d && dy == 0d;
            }
        }

        /// <summary>
        /// Gets the determinant of this matrix.
        /// </summary>
        [Browsable(false)]
        public double Determinant
        {
            get
            {
                return (m11 * m22) - (m12 * m21);
            }
        }

        /// <summary>
        /// Creates a <see cref="TransformMatrix"/> with the specified translation vector.
        /// </summary>
        /// <param name="offsetX">The x value by which to translate
        /// this <see cref="TransformMatrix"/>.</param>
        /// <param name="offsetY">The y value by which to translate
        /// this <see cref="TransformMatrix"/>.</param>
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
        public static TransformMatrix CreateRotation(double angle)
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
            m11 = 1d;
            m12 = 0d;
            m21 = 0d;
            m22 = 1d;
            dx = 0d;
            dy = 0d;
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

            var matrixDX = matrix.dx;
            var matrixDY = matrix.dy;
            var matrixM11 = matrix.m11;
            var matrixM12 = matrix.m12;
            var matrixM21 = matrix.m21;
            var matrixM22 = matrix.m22;

            dx += matrixDX * m11 + matrixDY * m21;
            dy += matrixDX * m12 + matrixDY * m22;
            var e11 = matrixM11 * m11 + matrixM12 * m21;
            var e12 = matrixM11 * m12 + matrixM12 * m22;
            var e21 = matrixM21 * m11 + matrixM22 * m21;
            m22 = matrixM21 * m12 + matrixM22 * m22;
            m11 = e11;
            m12 = e12;
            m21 = e21;
        }

        /// <summary>
        /// Applies the specified translation vector to
        /// this <see cref="TransformMatrix"/> by prepending the translation vector.
        /// </summary>
        /// <param name="offsetX">The x value by which to
        /// translate this <see cref="TransformMatrix"/>.</param>
        /// <param name="offsetY">The y value by which to
        /// translate this <see cref="TransformMatrix"/>.</param>
        public void Translate(double offsetX, double offsetY)
        {
            /*
            add the translation to this matrix
            |  1   0   0 |   | m_11  m_12   0 |
            |  0   1   0 | x | m_21  m_22   0 |
            | dx  dy   1 |   | m_tx  m_ty   1 |
            */

            dx += m11 * offsetX + m21 * offsetY;
            dy += m12 * offsetX + m22 * offsetY;
        }

        /// <summary>
        /// Applies the specified scale vector to
        /// this <see cref="TransformMatrix"/> by prepending the scale vector.
        /// </summary>
        /// <param name="scaleX">The value by which to
        /// scale this <see cref="TransformMatrix"/> in the x-axis direction.</param>
        /// <param name="scaleY">The value by which to
        /// scale this <see cref="TransformMatrix"/> in the y-axis direction.</param>
        public void Scale(double scaleX, double scaleY)
        {
            /*
            // add the scale to this matrix
            // | xScale   0      0 |   | m_11  m_12   0 |
            // |   0    yScale   0 | x | m_21  m_22   0 |
            // |   0      0      1 |   | m_tx  m_ty   1 |
            */

            m11 *= scaleX;
            m12 *= scaleX;
            m21 *= scaleY;
            m22 *= scaleY;
        }

        /// <summary>
        /// Applies a clockwise rotation of the specified angle (degrees) about the
        /// origin to this <see cref="TransformMatrix"/>.
        /// </summary>
        /// <param name="angle">The angle of the clockwise rotation, in degrees.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(double angle)
        {
            angle %= 360.0; // Doing the modulo before converting to radians reduces total error
            RotateRadians(angle * MathUtils.DegToRad);
        }

        /// <summary>
        /// Applies a clockwise rotation of the specified angle (radians) about the
        /// origin to this <see cref="TransformMatrix"/>.
        /// </summary>
        /// <param name="angle">The angle of the clockwise rotation, in radians.</param>
        public void RotateRadians(double angleRadians)
        {
            /*
            // add the rotation to this matrix (clockwise, radians)
            // | cos    sin   0 |   | m_11  m_12   0 |
            // | -sin   cos   0 | x | m_21  m_22   0 |
            // |  0      0    1 |   | m_tx  m_ty   1 |
            */

            var c = Math.Cos(angleRadians);
            var s = Math.Sin(angleRadians);

            var e11 = c * m11 + s * m21;
            var e12 = c * m12 + s * m22;
            m21 = c * m21 - s * m11;
            m22 = c * m22 - s * m12;
            m11 = e11;
            m12 = e12;
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

            var ex = (m21 * dy - m22 * dx) / det;
            dy = (-m11 * dy + m12 * dx) / det;
            dx = ex;
            var e11 = m22 / det;
            m12 = -m12 / det;
            m21 = -m21 / det;
            m22 = m11 / det;
            m11 = e11;

            return true;
        }

        /// <summary>
        /// Applies the geometric transform this <see cref="TransformMatrix"/> represents to a point.
        /// </summary>
        /// <param name="point">A <see cref="PointD"/> to transform.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointD TransformPoint(PointD src)
        {
            /*
            applies that matrix to the point
                                      | m_11  m_12   0 |
            | src.m_x  src._my  1 | x | m_21  m_22   0 |
                                      | m_tx  m_ty   1 |
            */

            if (IsIdentity)
                return src;

            var x = src.X * m11 + src.Y * m21 + DX;
            var y = src.X * m12 + src.Y * m22 + DY;

            return new(x,y);
        }

        /// <summary>
        /// Applies the geometric transform this <see cref="TransformMatrix"/> represents to a size.
        /// </summary>
        /// <param name="size">A <see cref="SizeD"/> to transform.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD TransformSize(SizeD src)
        {
            /*
            applies the matrix except for translations
                                      | m_11  m_12   0 |
            | src.m_x  src._my  0 | x | m_21  m_22   0 |
                                      | m_tx  m_ty   1 |
            */

            if (IsIdentity)
                return src;

            var width = src.Width * m11 + src.Height * m21;
            var height = src.Width * m12 + src.Height * m22;
            
            return new(width, height);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is not TransformMatrix matrix)
                return false;

            var result =
                m11 == matrix.m11 &&
                m12 == matrix.m12 &&
                m21 == matrix.m21 &&
                m22 == matrix.m22 &&
                dx  == matrix.dx &&
                dy == matrix.dy;
            return result;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (m11, m12, m21, m22, dx, dy).GetHashCode();
        }
    }
}