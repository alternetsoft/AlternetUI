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
        public int ContainsPoint(PointI pt) => default;
        public int ContainsRect(RectI rect) => default;
        //Returns true if the region is empty, false otherwise
        public bool IsEmpty() => default;

        public bool IsOk() => default;

        public void InitializeWithRegion(Region region) => throw new Exception();

        public void InitializeWithRect(RectI rect) => throw new Exception();

        public unsafe void InitializeWithPolygon(
            PointD* points, int pointsCount,
            FillMode fillMode = FillMode.Alternate,
            float scaleFactor = 1.0f) => throw new Exception();

        public void IntersectWithRect(RectI rect) => throw new Exception();

        public void IntersectWithRegion(Region region) => throw new Exception();

        public void UnionWithRect(RectI rect) => throw new Exception();

        public void UnionWithRegion(Region region) => throw new Exception();

        public void XorWithRect(RectI rect) => throw new Exception();

        public void XorWithRegion(Region region) => throw new Exception();

        public void SubtractRect(RectI rect) => throw new Exception();

        public void SubtractRegion(Region region) => throw new Exception();

        public void Translate(int dx, int dy) => throw new Exception();

        public RectI GetBounds() => throw new Exception();

        public bool IsEqualTo(Region other) => throw new Exception();

        public int GetRegionHashCode() => throw new Exception();
    }
}