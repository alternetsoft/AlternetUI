using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements horizontal or vertical stack of drawable elements.
    /// </summary>
    public class DrawableStackElement : DrawableElement, IDrawableElement
    {
        private IEnumerable<IDrawableElement>? items;
        private Coord distance;
        private CoordAlignment alignment;
        private bool isVertical;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableStackElement"/> class.
        /// </summary>
        public DrawableStackElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableStackElement"/> class
        /// with the specified parameters.
        /// </summary>
        public DrawableStackElement(
            IEnumerable<IDrawableElement>? items,
            CoordAlignment alignment = CoordAlignment.Near,
            Coord distance = 0,
            bool isVertical = true)
        {
            this.items = items;
            this.distance = distance;
            this.alignment = alignment;
            this.isVertical = isVertical;
        }

        /// <summary>
        /// Gets or sets items.
        /// </summary>
        public virtual IEnumerable<IDrawableElement>? Items
        {
            get
            {
                return items;
            }

            set
            {
                SetProperty(ref items, value, nameof(Items));
            }
        }

        /// <summary>
        /// Gets or sets whether stack is vertical.
        /// </summary>
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

        /// <summary>
        /// Gets or sets opposite coordinate alignment (horizontal if IsVertical is true).
        /// </summary>
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

        /// <summary>
        /// Gets or sets distance between elements.
        /// </summary>
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

        /// <inheritdoc/>
        public override void Draw(Graphics dc, RectD container)
        {
            if (items is null)
                return;

            var thisSide = IsVertical;
            var otherSide = !thisSide;

            var size = Measure(dc, container.Size);
            var otherSize = size.GetSize(otherSide);
            var origin = container.Location;

            foreach (var obj in items)
            {
                var measure = obj.Measure(dc, container.Size);

                RectD itemRect = (origin, measure);
                RectD itemContainer = itemRect;
                itemContainer.SetSize(otherSide, otherSize);

                var alignedItemRect = AlignUtils.AlignRectInRect(
                    otherSide,
                    itemRect,
                    itemContainer,
                    alignment);

                obj.Draw(dc, alignedItemRect);

                origin.IncLocation(thisSide, measure.GetSize(thisSide) + distance);
            }
        }

        /// <inheritdoc/>
        public override SizeD Measure(Graphics dc, SizeD availableSize)
        {
            if (items is null)
                return SizeD.Empty;

            Coord width = 0;
            Coord height = 0;

            if (IsVertical)
            {
                foreach (var s in items)
                {
                    var size = s.Measure(dc, availableSize);
                    width = Math.Max(width, size.Width);
                    height += size.Height + distance;
                }
            }
            else
            {
                foreach (var s in items)
                {
                    var size = s.Measure(dc, availableSize);
                    height = Math.Max(height, size.Height);
                    width += size.Width + distance;
                }
            }

            return (width, height);
        }
    }
}
