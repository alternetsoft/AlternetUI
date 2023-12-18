#pragma warning disable
using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_region.html#a1edc6768118cf02749b46774a0ca37f9
    public class Region
    {
        public void Clear() { }

        //Returns a value indicating whether the given point is contained within the region.
        public int ContainsPoint(Point pt) => default;
        public int ContainsRect(Rect rect) => default;
        //Returns true if the region is empty, false otherwise
        public bool IsEmpty() => default;

        public bool IsOk() => default;

        public void InitializeWithRegion(Region region) => throw new Exception();

        public void InitializeWithRect(Rect rect) => throw new Exception();

        public void InitializeWithPolygon(
            Point[] points, FillMode fillMode = FillMode.Alternate) => throw new Exception();

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