using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="scaleFactor">The scale factor.</param>
        public SkiaRegionHandler(RectD rect, Coord? scaleFactor = null)
        {
            ScaleFactor = scaleFactor;
            var rectI = GraphicsFactory.PixelFromDip(rect, ScaleFactor);
            region = new(rectI);
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
        public SkiaRegionHandler(PointD[] points, FillMode fillMode)
        {
            SKPath path = new();
            path.FillType = fillMode.ToSkia();
            path.AddPoly(points.PixelFromDipD(ScaleFactor));
            region = new(path);
        }

        /// <summary>
        /// Gets or sets scale factor used when conversion device-independent units
        /// to/from pixels is performed.
        /// </summary>
        public Coord? ScaleFactor { get; set; } = 1;

        /// <seealso cref="Region.Clear()"/>
        public virtual void Clear()
        {
            region.SetEmpty();
        }

        /// <seealso cref="Region.Contains(PointD)"/>
        public virtual RegionContain ContainsPoint(PointD pt)
        {
            if (region.Contains(pt.PixelFromDip(ScaleFactor)))
                return RegionContain.InRegion;
            else
                return RegionContain.OutRegion;
        }

        /// <seealso cref="Region.Contains(RectD)"/>
        public virtual RegionContain ContainsRect(RectD rect)
        {
            if (region.Contains(rect.PixelFromDip(ScaleFactor)))
                return RegionContain.InRegion;
            else
                return RegionContain.OutRegion;
        }

        /// <seealso cref="Region.GetBounds()"/>
        public virtual RectD GetBounds()
        {
            var resultI = (RectI)region.Bounds;
            return resultI.PixelToDip(ScaleFactor);
        }

        /// <summary>
        /// Performs operation on the region using specified operation kind and rectangle parameter.
        /// </summary>
        /// <param name="rect">Rectangle parameter value for the operation.</param>
        /// <param name="op">Operation kind.</param>
        public virtual void Op(RectD rect, SKRegionOperation op)
        {
            region.Op(rect.PixelFromDip(ScaleFactor), op);
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

        /// <seealso cref="Region.Intersect(RectD)"/>
        public virtual void IntersectWithRect(RectD rect)
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

        /// <seealso cref="Region.Union(RectD)"/>
        public virtual void UnionWithRect(RectD rect)
        {
            Op(rect, SKRegionOperation.Union);
        }

        /// <seealso cref="Region.Union(Region)"/>
        public virtual void UnionWithRegion(Region region)
        {
            Op(region, SKRegionOperation.Union);
        }

        /// <seealso cref="Region.Xor(RectD)"/>
        public virtual void XorWithRect(RectD rect)
        {
            Op(rect, SKRegionOperation.XOR);
        }

        /// <seealso cref="Region.Xor(Region)"/>
        public virtual void XorWithRegion(Region region)
        {
            Op(region, SKRegionOperation.XOR);
        }

        /// <seealso cref="Region.Translate"/>
        public virtual void Translate(Coord dx, Coord dy)
        {
            region.Translate(dx.PixelFromDip(ScaleFactor), dy.PixelFromDip(ScaleFactor));
        }

        /// <seealso cref="Region.Subtract(RectD)"/>
        public virtual void SubtractRect(RectD rect)
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
