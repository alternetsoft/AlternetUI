using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class NativeDrawing
    {
        /// <summary>
        /// Updates native transform matrix properties.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <param name="m11"></param>
        /// <param name="m12"></param>
        /// <param name="m21"></param>
        /// <param name="m22"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public abstract void UpdateTransformMatrix(
            object matrix,
            double m11,
            double m12,
            double m21,
            double m22,
            double dx,
            double dy);

        /// <summary>
        /// Gets M11 property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <returns></returns>
        public abstract double GetTransformMatrixM11(object matrix);

        /// <summary>
        /// Gets M12 property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <returns></returns>
        public abstract double GetTransformMatrixM12(object matrix);

        /// <summary>
        /// Gets M21 property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <returns></returns>
        public abstract double GetTransformMatrixM21(object matrix);

        /// <summary>
        /// Gets M22 property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <returns></returns>
        public abstract double GetTransformMatrixM22(object matrix);

        /// <summary>
        /// Gets DX property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <returns></returns>
        public abstract double GetTransformMatrixDX(object matrix);

        /// <summary>
        /// Gets DY property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <returns></returns>
        public abstract double GetTransformMatrixDY(object matrix);

        /// <summary>
        /// Gets a value indicating whether native transform matrix
        /// is the identity matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <returns></returns>
        public abstract bool GetTransformMatrixIsIdentity(object matrix);

        /// <summary>
        /// Resets native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        public abstract void ResetTransformMatrix(object matrix);

        /// <summary>
        /// Multiplies native transform matrix.
        /// </summary>
        /// <param name="matrix1">Native transform matrix instance 1.</param>
        /// <param name="matrix2">Native transform matrix instance 2.</param>
        public abstract void MultiplyTransformMatrix(object matrix1, object matrix2);

        /// <summary>
        /// Applies the specified translation vector to
        /// the native transform matrix by prepending the translation vector.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <param name="offsetX">The x value by which to
        /// translate the native transform matrix.</param>
        /// <param name="offsetY">The y value by which to
        /// translate the native transform matrix.</param>
        public abstract void TranslateTransformMatrix(object matrix, double offsetX, double offsetY);

        /// <summary>
        /// Applies the specified scale vector to
        /// the native transform matrix by prepending the scale vector.
        /// </summary>
        /// <param name="scaleX">The value by which to
        /// scale the native transform matrix in the x-axis direction.</param>
        /// <param name="scaleY">The value by which to
        /// scale the native transform matrix in the y-axis direction.</param>
        /// <param name="matrix">Native transform matrix instance.</param>
        public abstract void ScaleTransformMatrix(object matrix, double scaleX, double scaleY);

        /// <summary>
        /// Applies a clockwise rotation of the specified angle about the
        /// origin to the native transform matrix.
        /// </summary>
        /// <param name="angle">The angle of the clockwise rotation, in degrees.</param>
        /// <param name="matrix">Native transform matrix instance.</param>
        public abstract void RotateTransformMatrix(object matrix, double angle);

        /// <summary>
        /// Inverts the native transform matrix, if it is invertible.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        public abstract void InvertTransformMatrix(object matrix);

        /// <summary>
        /// Sets M11 property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <param name="value">New value of the M11 property.</param>
        public abstract void SetTransformMatrixM11(object matrix, double value);

        /// <summary>
        /// Sets M12 property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <param name="value">New value of the M12 property.</param>
        public abstract void SetTransformMatrixM12(object matrix, double value);

        /// <summary>
        /// Sets M21 property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <param name="value">New value of the M21 property.</param>
        public abstract void SetTransformMatrixM21(object matrix, double value);

        /// <summary>
        /// Sets M22 property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <param name="value">New value of the M22 property.</param>
        public abstract void SetTransformMatrixM22(object matrix, double value);

        /// <summary>
        /// Sets DX property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <param name="value">New value of the DX property.</param>
        public abstract void SetTransformMatrixDX(object matrix, double value);

        /// <summary>
        /// Sets DY property of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <param name="value">New value of the DY property.</param>
        public abstract void SetTransformMatrixDY(object matrix, double value);

        /// <summary>
        /// Applies the geometric transform to a <see cref="Point"/>.
        /// </summary>
        /// <param name="point">A <see cref="PointD"/> to transform.</param>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <returns></returns>
        public abstract PointD TransformMatrixOnPoint(object matrix, PointD point);

        /// <summary>
        /// Applies the geometric transform to a <see cref="Size"/>.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <param name="size">A <see cref="SizeD"/> to transform.</param>
        /// <returns></returns>
        public abstract SizeD TransformMatrixOnSize(object matrix, SizeD size);

        /// <summary>
        /// Indicates whether native transform matrix is equal to another native transform matrix.
        /// </summary>
        /// <returns></returns>
        public abstract bool TransformMatrixEquals(object matrix1, object matrix2);

        /// <summary>
        /// Gets hash code of the native transform matrix.
        /// </summary>
        /// <param name="matrix">Native transform matrix instance.</param>
        /// <returns></returns>
        public abstract int TransformMatrixGetHashCode(object matrix);

        /// <summary>
        /// Creates native transform matrix.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateTransformMatrix();
    }
}
