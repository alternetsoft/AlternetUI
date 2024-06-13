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
    public class SkiaRegionHandler : DisposableObject, IRegionHandler
    {
        private readonly SKRegion region;

        public SkiaRegionHandler()
        {
            region = new();
        }

        public Coord? ScaleFactor { get; set; }

        public SkiaRegionHandler(RectD rect)
        {
            var rectI = GraphicsFactory.PixelFromDip(rect, ScaleFactor);
            region = new(rectI);
        }

        public SkiaRegionHandler(Region region)
        {
            if (region.Handler is SkiaRegionHandler skiaHandler)
                this.region = new(skiaHandler.region);
            else
                throw new Exception("SkiaRegionHandler required in SkiaRegionHandler(Region) constructor");
        }

        public SkiaRegionHandler(PointD[] points, FillMode fillMode)
        {
            SKPath path = new();
            path.FillType = fillMode.ToSkia();
            path.AddPoly(points.PixelFromDipD(ScaleFactor));

            region = new(path);
            throw new NotImplementedException();
        }

        public virtual void Clear()
        {
            region.SetEmpty();
        }

        public virtual RegionContain ContainsPoint(PointD pt)
        {
            if (region.Contains(pt.PixelFromDip(ScaleFactor)))
                return RegionContain.InRegion;
            else
                return RegionContain.OutRegion;
        }

        public virtual RegionContain ContainsRect(RectD rect)
        {
            if (region.Contains(rect.PixelFromDip(ScaleFactor)))
                return RegionContain.InRegion;
            else
                return RegionContain.OutRegion;
        }

        public virtual RectD GetBounds()
        {
            var resultI = (RectI)region.Bounds;
            return resultI.PixelToDip(ScaleFactor);
        }

        public virtual void Op(RectD rect, SKRegionOperation op)
        {
            region.Op(rect.PixelFromDip(ScaleFactor), op);
        }

        public virtual void Op(Region region, SKRegionOperation op)
        {
            if (region.Handler is SkiaRegionHandler skiaHandler)
                this.region.Op(skiaHandler.region, op);
            else
                throw new Exception("Only SkiaRegionHandler is supported in Op(Region, SKRegionOperation)");
        }

        public virtual void IntersectWithRect(RectD rect)
        {
            Op(rect, SKRegionOperation.Intersect);
        }

        public virtual void IntersectWithRegion(Region region)
        {
            Op(region, SKRegionOperation.Intersect);
        }

        public virtual bool IsEmpty()
        {
            return region.IsEmpty;
        }

        public virtual bool IsEqualTo(Region region)
        {
            return region.Equals(region);
        }

        public virtual bool IsOk()
        {
            return !IsDisposed;
        }

        public virtual void UnionWithRect(RectD rect)
        {
            Op(rect, SKRegionOperation.Union);
        }

        public virtual void UnionWithRegion(Region region)
        {
            Op(region, SKRegionOperation.Union);
        }

        public virtual void XorWithRect(RectD rect)
        {
            Op(rect, SKRegionOperation.XOR);
        }

        public virtual void XorWithRegion(Region region)
        {
            Op(region, SKRegionOperation.XOR);
        }

        public virtual void Translate(double dx, double dy)
        {
            region.Translate(dx.PixelFromDip(ScaleFactor), dy.PixelFromDip(ScaleFactor));
        }

        public virtual void SubtractRect(RectD rect)
        {
            Op(rect, SKRegionOperation.Difference);
        }

        public virtual void SubtractRegion(Region region)
        {
            Op(region, SKRegionOperation.Difference);
        }
    }
}
