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

        public override object CreateRegion(object region)
        {
            var nativeObject = new UI.Native.Region();
            ((UI.Native.Region)nativeObject).InitializeWithRegion((UI.Native.Region)region);
            return nativeObject;
        }

        public override object CreateRegion(PointD[] points, FillMode fillMode = FillMode.Alternate)
        {
            var nativeObject = new UI.Native.Region();
            ((UI.Native.Region)nativeObject).InitializeWithPolygon(points, (UI.Native.FillMode)fillMode);
            return nativeObject;
        }

        public override bool RegionIsEmpty(object nativeRegion)
            => ((UI.Native.Region)nativeRegion).IsEmpty();

        public override bool RegionIsOk(object nativeRegion) => ((UI.Native.Region)nativeRegion).IsOk();

        public override void RegionClear(object nativeRegion)
        {
            ((UI.Native.Region)nativeRegion).Clear();
        }

        public override RegionContain RegionContains(object nativeRegion, PointD pt)
        {
            return (RegionContain)((UI.Native.Region)nativeRegion).ContainsPoint(pt);
        }

        public override RegionContain RegionContains(object nativeRegion, RectD rect)
        {
            return (RegionContain)((UI.Native.Region)nativeRegion).ContainsRect(rect);
        }

        public override void RegionIntersect(object nativeRegion, RectD rect)
        {
            ((UI.Native.Region)nativeRegion).IntersectWithRect(rect);
        }

        public override void RegionIntersect(object nativeRegion1, object nativeRegion2)
        {
            ((UI.Native.Region)nativeRegion1).IntersectWithRegion((UI.Native.Region)nativeRegion2);
        }

        public override void RegionUnion(object nativeRegion, RectD rect)
        {
            ((UI.Native.Region)nativeRegion).UnionWithRect(rect);
        }

        public override void RegionUnion(object nativeRegion1, object nativeRegion2)
        {
            ((UI.Native.Region)nativeRegion1).UnionWithRegion((UI.Native.Region)nativeRegion2);
        }

        public override void RegionXor(object nativeRegion1, object nativeRegion2)
        {
            ((UI.Native.Region)nativeRegion1).XorWithRegion((UI.Native.Region)nativeRegion2);
        }

        public override void RegionXor(object nativeRegion, RectD rect)
        {
            ((UI.Native.Region)nativeRegion).XorWithRect(rect);
        }

        public override void RegionSubtract(object nativeRegion, RectD rect)
        {
            ((UI.Native.Region)nativeRegion).SubtractRect(rect);
        }

        public override void RegionSubtract(object nativeRegion1, object nativeRegion2)
        {
            ((UI.Native.Region)nativeRegion1).SubtractRegion((UI.Native.Region)nativeRegion2);
        }

        public override void RegionTranslate(object nativeRegion, double dx, double dy)
        {
            ((UI.Native.Region)nativeRegion).Translate(dx, dy);
        }

        public override RectD RegionGetBounds(object nativeRegion)
        {
            return ((UI.Native.Region)nativeRegion).GetBounds();
        }

        public override bool RegionEquals(object nativeRegion1, object nativeRegion2)
        {
            return ((UI.Native.Region)nativeRegion1).IsEqualTo((UI.Native.Region)nativeRegion2);
        }

        public override int RegionGetHashCode(object nativeRegion)
        {
            return ((UI.Native.Region)nativeRegion).GetHashCode_();
        }
    }
}
