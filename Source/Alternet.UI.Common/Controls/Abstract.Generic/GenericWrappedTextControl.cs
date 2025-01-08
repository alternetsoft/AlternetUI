﻿using System;
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
        private TextFormat.Record textFormat;
        private List<string>? wrappedText;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericWrappedTextControl"/> class
        /// with the specified parent.
        /// </summary>
        public GenericWrappedTextControl(AbstractControl? parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericWrappedTextControl"/> class.
        /// </summary>
        public GenericWrappedTextControl()
        {
        }

        /// <summary>
        /// Gets or sets distance between lines of text.
        /// </summary>
        public virtual Coord LineDistance
        {
            get
            {
                return textFormat.Distance;
            }

            set
            {
                if (textFormat.Distance == value)
                    return;
                textFormat.Distance = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <see cref="TextFormat.VerticalAlignment"/>.
        public virtual TextVerticalAlignment TextVerticalAlignment
        {
            get => textFormat.VerticalAlignment;
            set
            {
                if (textFormat.VerticalAlignment == value)
                    return;
                textFormat.VerticalAlignment = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <see cref="TextFormat.Trimming"/>.
        public virtual TextTrimming TextTrimming
        {
            get => textFormat.Trimming;
            set
            {
                if (textFormat.Trimming == value)
                    return;
                textFormat.Trimming = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <see cref="TextFormat.Wrapping"/>.
        public virtual TextWrapping TextWrapping
        {
            get => textFormat.Wrapping;
            set
            {
                if (textFormat.Wrapping == value)
                    return;
                textFormat.Wrapping = value;
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
                return textFormat.HorizontalAlignment;
            }

            set
            {
                if (textFormat.HorizontalAlignment == value)
                    return;
                textFormat.HorizontalAlignment = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets text formatting record.
        /// </summary>
        /// <returns></returns>
        public virtual TextFormat.Record GetFormat()
        {
            return textFormat;
        }

        /// <summary>
        /// Sets text formatting record.
        /// </summary>
        public virtual void SetFormat(TextFormat.Record value)
        {
            textFormat = value;
            PerformLayoutAndInvalidate(() =>
            {
                MaxHeight = value.MaxHeight;
                MaxWidth = value.MaxWidth;
                SetSuggestedSize(value.SuggestedWidth, value.SuggestedHeight);
                Padding = value.Padding;
            });
        }

        /// <inheritdoc/>
        public override SizeD MeasureText(Graphics dc, Font font, SizeD availableSize)
        {
            var wrappedWidth = availableSize.Width;

            wrappedText
                = DrawingUtils.WrapTextToList(TextForPaint, ref wrappedWidth, font, dc);

            var result = DrawInternal(dc, (PointD.Empty, availableSize), font);
            return result;
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
            var textRect = rect.WithHeight(size.Height);

            var vertAlignment = AlignUtils.Convert(TextVerticalAlignment);

            var alignedItemRect = AlignUtils.AlignRectInRect(
                textRect,
                rect,
                null,
                vertAlignment);

            DrawInternal(dc, alignedItemRect, font, foreColor, backColor);
        }

        private SizeD DrawInternal(
            Graphics dc,
            RectD rect,
            Font font,
            Color? foreColor = null,
            Color? backColor = null)
        {
            var wrappedWidth = 0;

            if (wrappedText is null)
                return SizeD.Empty;

            var origin = rect.Location;
            SizeD totalMeasure = (wrappedWidth, 0);

            foreach (var s in wrappedText)
            {
                var measure = dc.MeasureText(s, font).Ceiling();

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

                    dc.DrawText(
                        s,
                        alignedItemRect.Location,
                        font,
                        foreColor,
                        backColor ?? Color.Empty);
                }

                var increment = measure.Height + LineDistance;
                origin.Y += increment;
                totalMeasure.Height += increment;

                totalMeasure.Width = Math.Max(totalMeasure.Width, measure.Width);
            }

            return totalMeasure.ApplyMax(rect.Size);
        }
    }
}
