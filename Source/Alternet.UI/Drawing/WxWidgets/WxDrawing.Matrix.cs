using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxDrawing
    {
        /// <inheritdoc/>
        public override object CreateTransformMatrix()
        {
            return new UI.Native.TransformMatrix();
        }

        /// <inheritdoc/>
        public override void UpdateTransformMatrix(
            object matrix,
            double m11,
            double m12,
            double m21,
            double m22,
            double dx,
            double dy)
        {
            ((UI.Native.TransformMatrix)matrix).Initialize(m11, m12, m21, m22, dx, dy);
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixM11(object matrix)
        {
            return ((UI.Native.TransformMatrix)matrix).M11;
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixM12(object matrix)
        {
            return ((UI.Native.TransformMatrix)matrix).M12;
        }

        /// <inheritdoc/>
        public override bool GetTransformMatrixIsIdentity(object matrix)
        {
            return ((UI.Native.TransformMatrix)matrix).IsIdentity;
        }

        /// <inheritdoc/>
        public override void MultiplyTransformMatrix(object matrix1, object matrix2)
        {
            ((UI.Native.TransformMatrix)matrix1).Multiply((UI.Native.TransformMatrix)matrix2);
        }

        /// <inheritdoc/>
        public override void RotateTransformMatrix(object matrix, double angle)
        {
            ((UI.Native.TransformMatrix)matrix).Rotate(angle);
        }

        /// <inheritdoc/>
        public override PointD TransformMatrixOnPoint(object matrix, PointD point)
        {
            return ((UI.Native.TransformMatrix)matrix).TransformPoint(point);
        }

        /// <inheritdoc/>
        public override SizeD TransformMatrixOnSize(object matrix, SizeD size)
        {
            return ((UI.Native.TransformMatrix)matrix).TransformSize(size);
        }

        /// <inheritdoc/>
        public override int TransformMatrixGetHashCode(object matrix)
        {
            return ((UI.Native.TransformMatrix)matrix).GetHashCode_();
        }

        /// <inheritdoc/>
        public override bool TransformMatrixEquals(object matrix1, object matrix2)
        {
            return ((UI.Native.TransformMatrix)matrix1).IsEqualTo((UI.Native.TransformMatrix)matrix2);
        }

        /// <inheritdoc/>
        public override void InvertTransformMatrix(object matrix)
        {
            ((UI.Native.TransformMatrix)matrix).Invert();
        }

        /// <inheritdoc/>
        public override void TranslateTransformMatrix(object matrix, double offsetX, double offsetY)
        {
            ((UI.Native.TransformMatrix)matrix).Translate(offsetX, offsetY);
        }

        /// <inheritdoc/>
        public override void ScaleTransformMatrix(object matrix, double scaleX, double scaleY)
        {
            ((UI.Native.TransformMatrix)matrix).Scale(scaleX, scaleY);
        }

        /// <inheritdoc/>
        public override void ResetTransformMatrix(object matrix)
        {
            ((UI.Native.TransformMatrix)matrix).Reset();
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixM21(object matrix)
        {
            return ((UI.Native.TransformMatrix)matrix).M21;
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixM22(object matrix)
        {
            return ((UI.Native.TransformMatrix)matrix).M22;
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixDX(object matrix)
        {
            return ((UI.Native.TransformMatrix)matrix).DX;
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixDY(object matrix)
        {
            return ((UI.Native.TransformMatrix)matrix).DY;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixM11(object matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix).M11 = value;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixM12(object matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix).M12 = value;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixM21(object matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix).M21 = value;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixM22(object matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix).M22 = value;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixDX(object matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix).DX = value;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixDY(object matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix).DY = value;
        }
    }
}
