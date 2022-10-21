using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class Region
    {
        public void InitializeWithRect(Rect rect) => throw new Exception();

        public void InitializeWithPolygon(Point[] points, FillMode fillMode = FillMode.Alternate) => throw new Exception();

        public void IntersectWithRect(Rect rect) => throw new Exception();

        public void IntersectWithRegion(Region region) => throw new Exception();

        public void UnionWithRect(Rect rect) => throw new Exception();

        public void UnionWithRegion(Region region) => throw new Exception();

        public void XorWithRect(Rect rect) => throw new Exception();

        public void XorWithRegion(Region region) => throw new Exception();

        public void SubtractRect(Rect rect) => throw new Exception();

        public void SubtractRegion(Region region) => throw new Exception();

        public void Translate(double dx, double dy) => throw new Exception();

        public Rect GetBounds() => throw new Exception();

        public bool IsEqualTo(Region other) => throw new Exception();

        public int GetHashCode_() => throw new Exception();
    }
}