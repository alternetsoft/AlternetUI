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
            TransformMatrix matrix,
            double m11,
            double m12,
            double m21,
            double m22,
            double dx,
            double dy)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).Initialize(m11, m12, m21, m22, dx, dy);
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixM11(TransformMatrix matrix)
        {
            return ((UI.Native.TransformMatrix)matrix.NativeObject).M11;
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixM12(TransformMatrix matrix)
        {
            return ((UI.Native.TransformMatrix)matrix.NativeObject).M12;
        }

        /// <inheritdoc/>
        public override bool GetTransformMatrixIsIdentity(TransformMatrix matrix)
        {
            return ((UI.Native.TransformMatrix)matrix.NativeObject).IsIdentity;
        }

        /// <inheritdoc/>
        public override void MultiplyTransformMatrix(TransformMatrix matrix1, TransformMatrix matrix2)
        {
            ((UI.Native.TransformMatrix)matrix1.NativeObject)
                .Multiply((UI.Native.TransformMatrix)matrix2.NativeObject);
        }

        /// <inheritdoc/>
        public override void RotateTransformMatrix(TransformMatrix matrix, double angle)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).Rotate(angle);
        }

        /// <inheritdoc/>
        public override PointD TransformMatrixOnPoint(TransformMatrix matrix, PointD point)
        {
            return ((UI.Native.TransformMatrix)matrix.NativeObject).TransformPoint(point);
        }

        /// <inheritdoc/>
        public override SizeD TransformMatrixOnSize(TransformMatrix matrix, SizeD size)
        {
            return ((UI.Native.TransformMatrix)matrix.NativeObject).TransformSize(size);
        }

        /// <inheritdoc/>
        public override int TransformMatrixGetHashCode(TransformMatrix matrix)
        {
            return ((UI.Native.TransformMatrix)matrix.NativeObject).GetHashCode_();
        }

        /// <inheritdoc/>
        public override bool TransformMatrixEquals(TransformMatrix matrix1, TransformMatrix matrix2)
        {
            return ((UI.Native.TransformMatrix)matrix1.NativeObject)
                .IsEqualTo((UI.Native.TransformMatrix)matrix2.NativeObject);
        }

        /// <inheritdoc/>
        public override void InvertTransformMatrix(TransformMatrix matrix)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).Invert();
        }

        /// <inheritdoc/>
        public override void TranslateTransformMatrix(TransformMatrix matrix, double offsetX, double offsetY)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).Translate(offsetX, offsetY);
        }

        /// <inheritdoc/>
        public override void ScaleTransformMatrix(TransformMatrix matrix, double scaleX, double scaleY)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).Scale(scaleX, scaleY);
        }

        /// <inheritdoc/>
        public override void ResetTransformMatrix(TransformMatrix matrix)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).Reset();
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixM21(TransformMatrix matrix)
        {
            return ((UI.Native.TransformMatrix)matrix.NativeObject).M21;
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixM22(TransformMatrix matrix)
        {
            return ((UI.Native.TransformMatrix)matrix.NativeObject).M22;
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixDX(TransformMatrix matrix)
        {
            return ((UI.Native.TransformMatrix)matrix.NativeObject).DX;
        }

        /// <inheritdoc/>
        public override double GetTransformMatrixDY(TransformMatrix matrix)
        {
            return ((UI.Native.TransformMatrix)matrix.NativeObject).DY;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixM11(TransformMatrix matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).M11 = value;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixM12(TransformMatrix matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).M12 = value;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixM21(TransformMatrix matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).M21 = value;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixM22(TransformMatrix matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).M22 = value;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixDX(TransformMatrix matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).DX = value;
        }

        /// <inheritdoc/>
        public override void SetTransformMatrixDY(TransformMatrix matrix, double value)
        {
            ((UI.Native.TransformMatrix)matrix.NativeObject).DY = value;
        }
    }
}
