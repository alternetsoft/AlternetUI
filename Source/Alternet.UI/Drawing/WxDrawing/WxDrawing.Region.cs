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
        public override object CreateRegion()
        {
            return new UI.Native.Region();
        }

        public override object CreateRegion(RectD rect)
        {
            var nativeObject = new UI.Native.Region();
            ((UI.Native.Region)nativeObject).InitializeWithRect(rect);
            return nativeObject;
        }

        public override object CreateRegion(Region region)
        {
            var nativeObject = new UI.Native.Region();
            ((UI.Native.Region)nativeObject).InitializeWithRegion((UI.Native.Region)region.NativeObject);
            return nativeObject;
        }

        public override object CreateRegion(PointD[] points, FillMode fillMode = FillMode.Alternate)
        {
            var nativeObject = new UI.Native.Region();
            ((UI.Native.Region)nativeObject).InitializeWithPolygon(points, (UI.Native.FillMode)fillMode);
            return nativeObject;
        }

        public override bool RegionIsEmpty(Region region)
            => ((UI.Native.Region)region.NativeObject).IsEmpty();

        public override bool RegionIsOk(Region region)
            => ((UI.Native.Region)region.NativeObject).IsOk();

        public override void RegionClear(Region region)
        {
            ((UI.Native.Region)region.NativeObject).Clear();
        }

        public override RegionContain RegionContains(Region region, PointD pt)
        {
            return (RegionContain)((UI.Native.Region)region.NativeObject).ContainsPoint(pt);
        }

        public override RegionContain RegionContains(Region region, RectD rect)
        {
            return (RegionContain)((UI.Native.Region)region.NativeObject).ContainsRect(rect);
        }

        public override void RegionIntersect(Region region, RectD rect)
        {
            ((UI.Native.Region)region.NativeObject).IntersectWithRect(rect);
        }

        public override void RegionIntersect(Region region1, Region region2)
        {
            ((UI.Native.Region)region1.NativeObject)
                .IntersectWithRegion((UI.Native.Region)region2.NativeObject);
        }

        public override void RegionUnion(Region region, RectD rect)
        {
            ((UI.Native.Region)region.NativeObject).UnionWithRect(rect);
        }

        public override void RegionUnion(Region region1, Region region2)
        {
            ((UI.Native.Region)region1.NativeObject)
                .UnionWithRegion((UI.Native.Region)region2.NativeObject);
        }

        public override void RegionXor(Region region1, Region region2)
        {
            ((UI.Native.Region)region1.NativeObject)
                .XorWithRegion((UI.Native.Region)region2.NativeObject);
        }

        public override void RegionXor(Region region, RectD rect)
        {
            ((UI.Native.Region)region.NativeObject).XorWithRect(rect);
        }

        public override void RegionSubtract(Region region, RectD rect)
        {
            ((UI.Native.Region)region.NativeObject).SubtractRect(rect);
        }

        public override void RegionSubtract(Region region1, Region region2)
        {
            ((UI.Native.Region)region1.NativeObject)
                .SubtractRegion((UI.Native.Region)region2.NativeObject);
        }

        public override void RegionTranslate(Region region, double dx, double dy)
        {
            ((UI.Native.Region)region.NativeObject).Translate(dx, dy);
        }

        public override RectD RegionGetBounds(Region region)
        {
            return ((UI.Native.Region)region.NativeObject).GetBounds();
        }

        public override bool RegionEquals(Region region1, Region region2)
        {
            return ((UI.Native.Region)region1.NativeObject)
                .IsEqualTo((UI.Native.Region)region2.NativeObject);
        }

        public override int RegionGetHashCode(Region region)
        {
            return ((UI.Native.Region)region.NativeObject).GetHashCode_();
        }
    }
}
