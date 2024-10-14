using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal class StyledTextContainer : StyledText, IStyledText
    {
        private readonly bool isVertical;
        private readonly IEnumerable<StyledText> items;
        private readonly Coord distance;

        public StyledTextContainer(
            IEnumerable<StyledText> items,
            Coord distance = 0,
            bool isVertical = true)
        {
            this.isVertical = isVertical;
            this.items = items;
            this.distance = distance;
        }

        public void Draw(Graphics dc, PointD location, Font? font, Color? foreColor, Color? backColor)
        {
            dc.DrawStyledText(
                items,
                location,
                SafeFont(font),
                SafeForeColor(foreColor),
                SafeBackColor(backColor),
                distance,
                isVertical);
        }

        public void Draw(Graphics dc, PointD location, Font? font, Brush? brush)
        {
            dc.DrawStyledText(
                items,
                SafeFont(font),
                SafeForeBrush(brush),
                location,
                distance,
                isVertical);
        }

        public SizeD Measure(Graphics dc, Font? font)
        {
            Coord width = 0;
            Coord height = 0;

            var realFont = SafeFont(font);

            if (isVertical)
            {
                foreach (var s in items)
                {
                    var size = dc.MeasureStyledText(s, realFont);
                    width = Math.Max(width, size.Width);
                    height += size.Height + distance;
                }
            }
            else
            {
                foreach (var s in items)
                {
                    var size = dc.MeasureStyledText(s, realFont);
                    height = Math.Max(height, size.Height);
                    width += size.Width + distance;
                }
            }

            return (width, height);
        }
    }
}
