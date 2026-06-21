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

        /// <inheritdoc cref="Region.Contains(PointI)"/>
        RegionContain ContainsPoint(PointI pt);

        /// <inheritdoc cref="Region.Contains(RectI)"/>
        RegionContain ContainsRect(RectI rect);

        /// <inheritdoc cref="Region.Intersect(RectI)"/>
        void IntersectWithRect(RectI rect);

        /// <inheritdoc cref="Region.Intersect(Region)"/>
        void IntersectWithRegion(Region region);

        /// <inheritdoc cref="Region.Union(RectI)"/>
        void UnionWithRect(RectI rect);

        /// <inheritdoc cref="Region.Union(Region)"/>
        void UnionWithRegion(Region region);

        /// <inheritdoc cref="Region.Xor(Region)"/>
        void XorWithRegion(Region region);

        /// <inheritdoc cref="Region.Xor(RectI)"/>
        void XorWithRect(RectI rect);

        /// <inheritdoc cref="Region.Subtract(RectI)"/>
        void SubtractRect(RectI rect);

        /// <inheritdoc cref="Region.Subtract(Region)"/>
        void SubtractRegion(Region region);

        /// <inheritdoc cref="Region.Translate"/>
        void Translate(int dx, int dy);

        /// <inheritdoc cref="Region.GetBounds()"/>
        RectI GetBounds();

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
