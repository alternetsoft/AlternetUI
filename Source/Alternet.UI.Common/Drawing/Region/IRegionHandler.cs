using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to work with region.
    /// </summary>
    public interface IRegionHandler : IDisposable
    {
        /// <inheritdoc cref="Region.IsEmpty"/>
        bool IsEmpty();

        /// <inheritdoc cref="Region.IsOk"/>
        bool IsOk();

        /// <inheritdoc cref="Region.Clear"/>
        void Clear();

        /// <inheritdoc cref="Region.Contains(PointD)"/>
        RegionContain ContainsPoint(PointD pt);

        /// <inheritdoc cref="Region.Contains(RectD)"/>
        RegionContain ContainsRect(RectD rect);

        /// <inheritdoc cref="Region.Intersect(RectD)"/>
        void IntersectWithRect(RectD rect);

        /// <inheritdoc cref="Region.Intersect(Region)"/>
        void IntersectWithRegion(Region region);

        /// <inheritdoc cref="Region.Union(RectD)"/>
        void UnionWithRect(RectD rect);

        /// <inheritdoc cref="Region.Union(Region)"/>
        void UnionWithRegion(Region region);

        /// <inheritdoc cref="Region.Xor(Region)"/>
        void XorWithRegion(Region region);

        /// <inheritdoc cref="Region.Xor(RectD)"/>
        void XorWithRect(RectD rect);

        /// <inheritdoc cref="Region.Subtract(RectD)"/>
        void SubtractRect(RectD rect);

        /// <inheritdoc cref="Region.Subtract(Region)"/>
        void SubtractRegion(Region region);

        /// <inheritdoc cref="Region.Translate"/>
        void Translate(Coord dx, Coord dy);

        /// <inheritdoc cref="Region.GetBounds()"/>
        RectD GetBounds();

        /// <summary>
        /// Gets whether or not this region is equal to other region.
        /// </summary>
        /// <param name="region">Region to compare with.</param>
        /// <returns></returns>
        bool IsEqualTo(Region region);

        /// <inheritdoc cref="Region.GetHashCode"/>
        int GetHashCode();
    }
}
