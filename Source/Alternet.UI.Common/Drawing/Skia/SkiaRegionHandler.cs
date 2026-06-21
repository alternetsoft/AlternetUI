using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;
using Alternet.UI.Extensions;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements <see cref="IRegionHandler"/> provider for the SkiaSharp regions.
    /// </summary>
    public class SkiaRegionHandler : DisposableObject, IRegionHandler
    {
        private readonly SKRegion region;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaRegionHandler"/> class.
        /// </summary>
        public SkiaRegionHandler()
        {
            region = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaRegionHandler"/> class
        /// with the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        public SkiaRegionHandler(RectI rect)
        {
            region = new(rect);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaRegionHandler"/> class
        /// with the specified region.
        /// </summary>
        /// <param name="region"></param>
        /// <exception cref="ArgumentException">Raised if <paramref name="region"/>
        /// type is not supported.</exception>
        public SkiaRegionHandler(Region region)
        {
            if (region.Handler is SkiaRegionHandler skiaHandler)
                this.region = new(skiaHandler.region);
            else
            {
                throw new ArgumentException(
                    "SkiaRegionHandler required in SkiaRegionHandler constructor");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaRegionHandler"/> class.
        /// </summary>
        /// <param name="points">Array of points which specify region.</param>
        /// <param name="fillMode">Fill mode.</param>
        /// <param name="scaleFactor">The scale factor for the polygon.</param>
        public SkiaRegionHandler(ReadOnlySpan<PointD> points, FillMode fillMode, float scaleFactor = 1.0f)
        {
            SKPath path = new();
            path.FillType = fillMode.ToSkia();

            ReadOnlySpan<SKPoint> spanSKPoint = MemoryMarshal.Cast<PointD, SKPoint>(points);
            path.AddPoly(spanSKPoint);

            if (scaleFactor != 1.0f)
            {
                path.Transform(SKMatrix.CreateScale(scaleFactor, scaleFactor));
            }

            region = new(path);
        }

        /// <summary>
        /// Gets the SkiaSharp region which used by this handler.
        /// </summary>
        public SKRegion Region => region;

        /// <seealso cref="Region.Clear()"/>
        public virtual void Clear()
        {
            region.SetEmpty();
        }

        /// <seealso cref="Region.Contains(PointI)"/>
        public virtual RegionContain ContainsPoint(PointI pt)
        {
            if (region.Contains(pt))
                return RegionContain.InRegion;
            else
                return RegionContain.OutRegion;
        }

        /// <seealso cref="Region.Contains(RectI)"/>
        public virtual RegionContain ContainsRect(RectI rect)
        {
            if (region.Contains(rect))
                return RegionContain.InRegion;
            else
                return RegionContain.OutRegion;
        }

        /// <seealso cref="Region.GetBounds()"/>
        public virtual RectI GetBounds()
        {
            return region.Bounds;
        }

        /// <summary>
        /// Performs operation on the region using specified operation kind and rectangle parameter.
        /// </summary>
        /// <param name="rect">Rectangle parameter value for the operation.</param>
        /// <param name="op">Operation kind.</param>
        public virtual void Op(RectI rect, SKRegionOperation op)
        {
            region.Op(rect, op);
        }

        /// <summary>
        /// Performs operation on this region using specified operation kind and region parameter.
        /// </summary>
        /// <param name="region">Region parameter value for the operation.</param>
        /// <param name="op">Operation kind.</param>
        public virtual void Op(Region region, SKRegionOperation op)
        {
            if (region.Handler is SkiaRegionHandler skiaHandler)
                this.region.Op(skiaHandler.region, op);
            else
            {
                throw new Exception(
                    "Only SkiaRegionHandler is supported in Op(Region, SKRegionOperation)");
            }
        }

        /// <seealso cref="Region.Intersect(RectI)"/>
        public virtual void IntersectWithRect(RectI rect)
        {
            Op(rect, SKRegionOperation.Intersect);
        }

        /// <seealso cref="Region.Intersect(Region)"/>
        public virtual void IntersectWithRegion(Region region)
        {
            Op(region, SKRegionOperation.Intersect);
        }

        /// <seealso cref="Region.IsEmpty"/>
        public virtual bool IsEmpty()
        {
            return region.IsEmpty;
        }

        /// <summary>
        /// Calculates hash code for this object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return region.GetHashCode();
        }

        /// <summary>
        /// Gets whether or not this region is equal to other region.
        /// </summary>
        /// <param name="region">Region to compare with.</param>
        /// <returns></returns>
        public virtual bool IsEqualTo(Region region)
        {
            return region.Equals(region);
        }

        /// <seealso cref="Region.IsOk"/>
        public virtual bool IsOk()
        {
            return !IsDisposed;
        }

        /// <seealso cref="Region.Union(RectI)"/>
        public virtual void UnionWithRect(RectI rect)
        {
            Op(rect, SKRegionOperation.Union);
        }

        /// <seealso cref="Region.Union(Region)"/>
        public virtual void UnionWithRegion(Region region)
        {
            Op(region, SKRegionOperation.Union);
        }

        /// <seealso cref="Region.Xor(RectI)"/>
        public virtual void XorWithRect(RectI rect)
        {
            Op(rect, SKRegionOperation.XOR);
        }

        /// <seealso cref="Region.Xor(Region)"/>
        public virtual void XorWithRegion(Region region)
        {
            Op(region, SKRegionOperation.XOR);
        }

        /// <seealso cref="Region.Translate"/>
        public virtual void Translate(int dx, int dy)
        {
            region.Translate(dx, dy);
        }

        /// <seealso cref="Region.Subtract(RectI)"/>
        public virtual void SubtractRect(RectI rect)
        {
            Op(rect, SKRegionOperation.Difference);
        }

        /// <seealso cref="Region.Subtract(Region)"/>
        public virtual void SubtractRegion(Region region)
        {
            Op(region, SKRegionOperation.Difference);
        }
    }
}
