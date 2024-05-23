using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class TransformMatrix
    {
        public void Initialize(
            double m11,
            double m12,
            double m21,
            double m22,
            double dx,
            double dy) => throw new Exception();

        public double M11 { get; set; }
        public double M12 { get; set; }
        public double M21 { get; set; }
        public double M22 { get; set; }
        public double DX { get; set; }
        public double DY { get; set; }

        public void Reset() => throw new Exception();
        public void Multiply(TransformMatrix matrix) => throw new Exception();
        public void Translate(double offsetX, double offsetY) => throw new Exception();
        public void Scale(double scaleX, double scaleY) => throw new Exception();
        public void Rotate(double angle) => throw new Exception();
        public void Invert() => throw new Exception();
        public PointD TransformPoint(PointD point) => throw new Exception();
        public SizeD TransformSize(SizeD size) => throw new Exception();
        public bool IsIdentity { get; }
        public bool IsEqualTo(TransformMatrix other) => throw new Exception();
        public int GetHashCode_() => throw new Exception();
    }
}