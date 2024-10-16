using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements simple text control which can text with new line
    /// characters and wrapping.
    /// </summary>
    public class GenericWrappedTextControl : GenericTextControl
    {
        private TextHorizontalAlignment textHorizontalAlignment = TextFormat.DefaultHorizontalAlignment;
        private TextVerticalAlignment textVerticalAlignment = TextFormat.DefaultVerticalAlignment;
        private Coord textDistance = 0;
        private TextTrimming textTrimming = TextFormat.DefaultTrimming;
        private TextWrapping textWrapping = TextFormat.DefaultWrapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericWrappedTextControl"/> class.
        /// </summary>
        public GenericWrappedTextControl()
        {
        }

        /// <summary>
        /// Gets or sets distance between lines of text.
        /// </summary>
        public Coord LineDistance
        {
            get
            {
                return textDistance;
            }

            set
            {
                if (textDistance == value)
                    return;
                textDistance = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <see cref="TextFormat.VerticalAlignment"/>.
        public TextVerticalAlignment TextVerticalAlignment
        {
            get => textVerticalAlignment;
            set
            {
                if (textVerticalAlignment == value)
                    return;
                textVerticalAlignment = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <see cref="TextFormat.Trimming"/>.
        public virtual TextTrimming TextTrimming
        {
            get => textTrimming;
            set
            {
                if (textTrimming == value)
                    return;
                textTrimming = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <see cref="TextFormat.Wrapping"/>.
        public virtual TextWrapping TextWrapping
        {
            get => textWrapping;
            set
            {
                if (textWrapping == value)
                    return;
                textWrapping = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets text alignment.
        /// </summary>
        public virtual TextHorizontalAlignment TextHorizontalAlignment
        {
            get
            {
                return textHorizontalAlignment;
            }

            set
            {
                if (textHorizontalAlignment == value)
                    return;
                textHorizontalAlignment = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <inheritdoc/>
        public override SizeD MeasureText(Graphics dc, Font font, SizeD availableSize)
        {
            return DrawInternal(dc, (PointD.Empty, availableSize), font);
        }

        /// <inheritdoc/>
        public override void DrawText(
            Graphics dc,
            RectD rect,
            Font font,
            Color foreColor,
            Color backColor)
        {
            var size = DrawInternal(dc, rect, font);
            var textRect = rect.WithSize(size.Width, size.Height);

            var alignedItemRect = AlignUtils.AlignRectInRect(
                true,
                textRect,
                rect,
                (CoordAlignment)TextVerticalAlignment);

            DrawInternal(dc, alignedItemRect, font, foreColor, backColor);
        }

        private SizeD DrawInternal(
            Graphics dc,
            RectD rect,
            Font font,
            Color? foreColor = null,
            Color? backColor = null)
        {
            var wrappedText
                = DrawingUtils.WrapTextToList(TextForPaint, rect.Width, font, dc);

            var origin = rect.Location;
            SizeD totalMeasure = 0;

            foreach (var s in wrappedText)
            {
                var measure = dc.MeasureText(s, font);

                if (foreColor is not null)
                {
                    RectD itemRect = (origin, measure);
                    RectD itemContainer = itemRect;
                    itemContainer.Width = rect.Width;

                    var alignment = AlignUtils.Convert(TextHorizontalAlignment);

                    var alignedItemRect = AlignUtils.AlignRectInRect(
                        false,
                        itemRect,
                        itemContainer,
                        (CoordAlignment)alignment);

                    dc.DrawText(s, alignedItemRect.Location, font, foreColor, backColor ?? Color.Empty);
                }

                var increment = measure.Height + textDistance;
                origin.Y += increment;
                totalMeasure.Height += increment;

                totalMeasure.Width = Math.Max(totalMeasure.Width, measure.Width);
            }

            return totalMeasure;
        }
    }
}
