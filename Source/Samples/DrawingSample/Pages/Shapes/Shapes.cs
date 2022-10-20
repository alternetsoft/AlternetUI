using Alternet.Drawing;
using System;

namespace DrawingSample
{
    internal class Shapes
    {
        ShapesPage page;

        public Shapes(ShapesPage page)
        {
            this.page = page;
        }

        Pen StrokePen => page.StrokePen;
        Brush FillBrush => page.FillBrush;
        Brush BackgroundBrush => page.BackgroundBrush;

        public void DrawLine(DrawingContext dc, Rect bounds)
        {
            dc.DrawLine(StrokePen, bounds.TopLeft, bounds.BottomRight);
            dc.DrawLine(StrokePen, bounds.BottomLeft, bounds.TopRight);
            
            var l = Math.Min(bounds.Width, bounds.Height) / 4;
            var s = l / 2;

            dc.DrawLine(StrokePen, bounds.TopLeft + new Size(s, l), bounds.BottomLeft + new Size(s, -l));
            dc.DrawLine(StrokePen, bounds.TopLeft + new Size(l, s), bounds.TopRight - new Size(l, -s));

            dc.DrawLine(StrokePen, bounds.TopRight + new Size(-s, l), bounds.BottomRight + new Size(-s, -l));
            dc.DrawLine(StrokePen, bounds.BottomLeft + new Size(l, -s), bounds.BottomRight - new Size(l, s));

        }
    }
}