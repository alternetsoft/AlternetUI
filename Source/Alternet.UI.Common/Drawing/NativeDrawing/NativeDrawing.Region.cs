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
        public abstract object CreateRegion();

        public abstract object CreateRegion(RectD rect);

        public abstract object CreateRegion(object region);

        public abstract object CreateRegion(PointD[] points, FillMode fillMode = FillMode.Alternate);

        public abstract bool RegionIsEmpty(object nativeRegion);

        public abstract bool RegionIsOk(object nativeRegion);

        public abstract void RegionClear(object nativeRegion);

        public abstract RegionContain RegionContains(object nativeRegion, PointD pt);

        public abstract RegionContain RegionContains(object nativeRegion, RectD rect);

        public abstract void RegionIntersect(object nativeRegion, RectD rect);

        public abstract void RegionIntersect(object nativeRegion1, object nativeRegion2);

        public abstract void RegionUnion(object nativeRegion, RectD rect);

        public abstract void RegionUnion(object nativeRegion1, object nativeRegion2);

        public abstract void RegionXor(object nativeRegion1, object nativeRegion2);

        public abstract void RegionXor(object nativeRegion, RectD rect);

        public abstract void RegionSubtract(object nativeRegion, RectD rect);

        public abstract void RegionSubtract(object nativeRegion1, object nativeRegion2);

        public abstract void RegionTranslate(object nativeRegion, double dx, double dy);

        public abstract RectD RegionGetBounds(object nativeRegion);

        public abstract bool RegionEquals(object nativeRegion1, object nativeRegion2);

        public abstract int RegionGetHashCode(object nativeRegion);
    }
}
