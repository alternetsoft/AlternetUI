using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class DrawableStackElement : DrawableElement, IDrawableElement
    {
        private readonly IEnumerable<IDrawableElement> items;

        private Coord distance;
        private CoordAlignment alignment;
        private bool isVertical;

        public DrawableStackElement(
            IEnumerable<IDrawableElement> items,
            CoordAlignment alignment = CoordAlignment.Near,
            Coord distance = 0,
            bool isVertical = true)
        {
            this.items = items;
            this.distance = distance;
            this.alignment = alignment;
            this.isVertical = isVertical;
        }

        public virtual bool IsVertical
        {
            get
            {
                return isVertical;
            }

            set
            {
                SetProperty(ref isVertical, value, nameof(IsVertical));
            }
        }

        public virtual CoordAlignment Alignment
        {
            get
            {
                return alignment;
            }

            set
            {
                SetProperty(ref alignment, value, nameof(Alignment));
            }
        }

        public virtual Coord Distance
        {
            get
            {
                return distance;
            }

            set
            {
                SetProperty(ref distance, value, nameof(Distance));
            }
        }

        public override void Draw(Graphics dc, PointD origin)
        {
            var thisSide = IsVertical;
            var otherSide = !thisSide;

            var size = Measure(dc);
            var thisSize = size.GetSize(thisSide);
            var otherSize = size.GetSize(otherSide);

            foreach (var obj in items)
            {
                var measure = obj.Measure(dc);
                var alignedOrigin = origin;
                if (alignment != CoordAlignment.Near && otherSize > 0)
                {
                    RectD rect = (origin, measure);
                    RectD container = rect;
                    container.SetSize(otherSide, otherSize);

                    var alignedRect = AlignUtils.AlignRectInRect(
                        otherSide,
                        rect,
                        container,
                        alignment);

                    alignedOrigin.SetLocation(otherSide, alignedRect.GetLocation(otherSide));
                }

                obj.Draw(dc, alignedOrigin);

                origin.IncLocation(thisSide, measure.GetSize(thisSide) + distance);
            }
        }

        public override SizeD Measure(Graphics dc)
        {
            Coord width = 0;
            Coord height = 0;

            if (IsVertical)
            {
                foreach (var s in items)
                {
                    var size = s.Measure(dc);
                    width = Math.Max(width, size.Width);
                    height += size.Height + distance;
                }
            }
            else
            {
                foreach (var s in items)
                {
                    var size = s.Measure(dc);
                    height = Math.Max(height, size.Height);
                    width += size.Width + distance;
                }
            }

            return (width, height);
        }
    }
}
