using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class Region : Alternet.Drawing.IRegionHandler
    {
        Alternet.Drawing.RegionContain Alternet.Drawing.IRegionHandler.ContainsPoint(
            Alternet.Drawing.PointD pt)
        {
            return (Alternet.Drawing.RegionContain)ContainsPoint(pt);
        }

        Alternet.Drawing.RegionContain Alternet.Drawing.IRegionHandler.ContainsRect(
            Alternet.Drawing.RectD rect)
        {
            return (Alternet.Drawing.RegionContain)ContainsRect(rect);
        }

        public void IntersectWithRegion(Alternet.Drawing.Region region)
        {
            IntersectWithRegion((UI.Native.Region)region.Handler);
        }

        public void UnionWithRegion(Alternet.Drawing.Region region)
        {
            UnionWithRegion((UI.Native.Region)region.Handler);
        }

        public void XorWithRegion(Alternet.Drawing.Region region)
        {
            XorWithRegion((UI.Native.Region)region.Handler);
        }

        public void SubtractRegion(Alternet.Drawing.Region region)
        {
            SubtractRegion((UI.Native.Region)region.Handler);
        }

        public bool IsEqualTo(Alternet.Drawing.Region region)
        {
            return IsEqualTo((UI.Native.Region)region.Handler);
        }

        int Alternet.Drawing.IRegionHandler.GetHashCode()
        {
            return GetHashCode_();
        }
    }
}
