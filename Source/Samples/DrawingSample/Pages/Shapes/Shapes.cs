using Alternet.Drawing;
using System;
using System.Collections.Generic;

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

        public void DrawLines(DrawingContext dc, Rect bounds)
        {
            var lines = new List<Point>();
            var w = bounds.Width / 6;
            for (double x = 0; x < bounds.Width; x += w)
            {
                lines.Add(bounds.TopLeft + new Size(x, 0));
                lines.Add(bounds.BottomLeft + new Size(x, 0));
            }

            dc.DrawLines(StrokePen, lines.ToArray());
        }

        public void DrawPolygon(DrawingContext dc, Rect bounds)
        {
            var lines = GetPolygonLines(bounds);

            dc.DrawPolygon(StrokePen, lines.ToArray());
        }

        public void FillPolygon(DrawingContext dc, Rect bounds)
        {
            var lines = GetPolygonLines(bounds);

            dc.FillPolygon(FillBrush, lines.ToArray());
        }

        public void DrawRectangle(DrawingContext dc, Rect bounds)
        {
            var r = GetRectangle(bounds);
            dc.DrawRectangle(StrokePen, r);
        }

        public void FillRectangle(DrawingContext dc, Rect bounds)
        {
            var r = GetRectangle(bounds);
            dc.FillRectangle(FillBrush, r);
        }

        public void DrawRectangles(DrawingContext dc, Rect bounds)
        {
            var r = GetRectangles(bounds);
            dc.DrawRectangles(StrokePen, r);
        }

        public void FillRectangles(DrawingContext dc, Rect bounds)
        {
            var r = GetRectangles(bounds);
            dc.FillRectangles(FillBrush, r);
        }

        private static Rect GetRectangle(Rect bounds)
        {
            var s = Math.Min(bounds.Width, bounds.Height) * 0.1;
            var r = bounds.InflatedBy(-s, -s * 2);
            return r;
        }

        private static Rect[] GetRectangles(Rect bounds)
        {
            var s = Math.Min(bounds.Width, bounds.Height) / 2;
            var r = bounds.WithSize(s, s);
            var w = s / 6;
            var rects = new List<Rect>();
            for (int i = 0; i < 7; i++)
                rects.Add(r.OffsetBy(i * w, i * w));

            return rects.ToArray();
        }

        private static List<Point> GetPolygonLines(Rect bounds)
        {
            var lines = new List<Point>();
            var r = Math.Min(bounds.Width, bounds.Height) / 2;
            var c = bounds.Center;
            for (double a = 0; a <= 360; a += 45)
            {
                lines.Add(MathUtils.GetPointOnCircle(c, r, a));
            }

            return lines;
        }
    }
}