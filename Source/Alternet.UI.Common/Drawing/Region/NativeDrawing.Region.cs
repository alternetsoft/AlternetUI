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

        public abstract object CreateRegion(Region region);

        public abstract object CreateRegion(PointD[] points, FillMode fillMode = FillMode.Alternate);

        public abstract bool RegionIsEmpty(Region region);

        public abstract bool RegionIsOk(Region region);

        public abstract void RegionClear(Region region);

        public abstract RegionContain RegionContains(Region region, PointD pt);

        public abstract RegionContain RegionContains(Region region, RectD rect);

        public abstract void RegionIntersect(Region region, RectD rect);

        public abstract void RegionIntersect(Region region1, Region region2);

        public abstract void RegionUnion(Region region, RectD rect);

        public abstract void RegionUnion(Region region1, Region region2);

        public abstract void RegionXor(Region region1, Region region2);

        public abstract void RegionXor(Region region, RectD rect);

        public abstract void RegionSubtract(Region region, RectD rect);

        public abstract void RegionSubtract(Region region1, Region region2);

        public abstract void RegionTranslate(Region region, double dx, double dy);

        public abstract RectD RegionGetBounds(Region region);

        public abstract bool RegionEquals(Region region1, Region region2);

        public abstract int RegionGetHashCode(Region region);
    }
}
