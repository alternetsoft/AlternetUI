using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    public interface IRegionHandler : IDisposable
    {
        bool IsEmpty();

        bool IsOk();

        void Clear();

        RegionContain ContainsPoint(PointD pt);

        RegionContain ContainsRect(RectD rect);

        void IntersectWithRect(RectD rect);

        void IntersectWithRegion(Region region);

        void UnionWithRect(RectD rect);

        void UnionWithRegion(Region region);

        void XorWithRegion(Region region);

        void XorWithRect(RectD rect);

        void SubtractRect(RectD rect);

        void SubtractRegion(Region region);

        void Translate(double dx, double dy);

        RectD GetBounds();

        bool IsEqualTo(Region region);

        int GetHashCode();
    }
}
