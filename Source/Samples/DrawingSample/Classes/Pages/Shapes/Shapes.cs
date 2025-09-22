using Alternet.Drawing;
using System;
using System.Collections.Generic;
using Alternet.UI;

namespace DrawingSample
{
    internal class Shapes
    {
        private readonly ShapesPage page;

        public Shapes(ShapesPage page)
        {
            this.page = page;
        }

        Pen StrokePen => page.StrokePen;
        Brush FillBrush => page.FillBrush;
        
        internal Brush BackgroundBrush => page.BackgroundBrush;

        public void DrawLine(Graphics dc, RectD bounds)
        {
            dc.DrawLine(StrokePen, bounds.TopLeft, bounds.BottomRight);
            dc.DrawLine(StrokePen, bounds.BottomLeft, bounds.TopRight);

            var l = Math.Min(bounds.Width, bounds.Height) / 4;
            var s = l / 2;

            dc.DrawLine(StrokePen, bounds.TopLeft + new SizeD(s, l), bounds.BottomLeft + new SizeD(s, -l));
            dc.DrawLine(StrokePen, bounds.TopLeft + new SizeD(l, s), bounds.TopRight - new SizeD(l, -s));

            dc.DrawLine(StrokePen, bounds.TopRight + new SizeD(-s, l), bounds.BottomRight + new SizeD(-s, -l));
            dc.DrawLine(StrokePen, bounds.BottomLeft + new SizeD(l, -s), bounds.BottomRight - new SizeD(l, s));
        }

        public void DrawLines(Graphics dc, RectD bounds)
        {
            var lines = new List<PointD>();
            var w = bounds.Width / 6;
            for (Coord x = 0; x < bounds.Width; x += w)
            {
                lines.Add(bounds.TopLeft + new SizeD(x, 0));
                lines.Add(bounds.BottomLeft + new SizeD(x, 0));
            }

            dc.DrawLines(StrokePen, lines.ToArray());
        }

        public void DrawBezier(Graphics dc, RectD bounds)
        {
            dc.DrawBezier(StrokePen, bounds.TopLeft, bounds.TopRight, bounds.BottomLeft, bounds.BottomRight);
        }

        public void DrawArc(Graphics dc, RectD bounds)
        {
            dc.DrawArc(StrokePen, bounds.Center, Math.Min(bounds.Width, bounds.Height) * 0.3f, 10, 210);
        }

        public void FillPie(Graphics dc, RectD bounds)
        {
            dc.FillPie(FillBrush, bounds.Center, Math.Min(bounds.Width, bounds.Height) * 0.3f, 10, 210);
        }

        public void DrawPie(Graphics dc, RectD bounds)
        {
            dc.DrawPie(StrokePen, bounds.Center, Math.Min(bounds.Width, bounds.Height) * 0.3f, 10, 210);
        }

        public void DrawBeziers(Graphics dc, RectD bounds)
        {
            var points = new[]
            {
                bounds.TopLeft, bounds.TopRight, bounds.BottomLeft, bounds.BottomRight,
                bounds.BottomLeft, bounds.BottomLeft, bounds.TopRight,
                bounds.BottomRight, bounds.BottomRight, bounds.BottomLeft,
            };

            dc.DrawBeziers(StrokePen, points);
        }

        public void DrawPolygon(Graphics dc, RectD bounds)
        {
            var lines = GetPolygonLines(bounds);

            dc.DrawPolygon(StrokePen, lines.ToArray());
        }

        public void FillPolygon(Graphics dc, RectD bounds)
        {
            var lines = GetPolygonLines(bounds);

            dc.FillPolygon(FillBrush, lines.ToArray());
        }

        public void DrawRectangle(Graphics dc, RectD bounds)
        {
            var r = GetRectangle(bounds);
            dc.DrawRectangle(StrokePen, r);
        }

        public void FillRectangle(Graphics dc, RectD bounds)
        {
            var r = GetRectangle(bounds);
            dc.FillRectangle(FillBrush, r);
        }

        public void DrawEllipse(Graphics dc, RectD bounds)
        {
            var r = GetRectangle(bounds);
            dc.DrawEllipse(StrokePen, r);
        }

        public void FillEllipse(Graphics dc, RectD bounds)
        {
            var r = GetRectangle(bounds);
            dc.FillEllipse(FillBrush, r);
        }

        public void DrawCircle(Graphics dc, RectD bounds)
        {
            dc.DrawCircle(StrokePen, bounds.Center, Math.Min(bounds.Width, bounds.Height) * 0.3f);
        }

        public void FillCircle(Graphics dc, RectD bounds)
        {
            dc.FillCircle(FillBrush, bounds.Center, Math.Min(bounds.Width, bounds.Height) * 0.3f);
        }

        public void DrawRoundedRectangle(Graphics dc, RectD bounds)
        {
            var r = GetRectangle(bounds);
            dc.DrawRoundedRectangle(StrokePen, r, 10);
        }

        public void FillRoundedRectangle(Graphics dc, RectD bounds)
        {
            var r = GetRectangle(bounds);
            dc.FillRoundedRectangle(FillBrush, r, 10);
        }

        public void DrawRectangles(Graphics dc, RectD bounds)
        {
            var r = GetRectangles(bounds);
            dc.DrawRectangles(StrokePen, r);
        }

        public void FillRectangles(Graphics dc, RectD bounds)
        {
            var r = GetRectangles(bounds);
            dc.FillRectangles(FillBrush, r);
        }

        private static RectD GetRectangle(RectD bounds)
        {
            var s = Math.Min(bounds.Width, bounds.Height) * 0.1f;
            var r = bounds.InflatedBy(-s, -s * 2);
            return r;
        }

        private static RectD[] GetRectangles(RectD bounds)
        {
            var s = Math.Min(bounds.Width, bounds.Height) / 2;
            var r = bounds.WithSize(s, s);
            var w = s / 6;
            var rects = new List<RectD>();
            for (int i = 0; i < 7; i++)
                rects.Add(r.OffsetBy(i * w, i * w));

            return rects.ToArray();
        }

        private static List<PointD> GetPolygonLines(RectD bounds)
        {
            var lines = new List<PointD>();
            var r = Math.Min(bounds.Width, bounds.Height) / 2;
            var c = bounds.Center;
            for (Coord a = 0; a <= 360; a += 45)
            {
                lines.Add(DrawingUtils.GetPointOnCircle(c, r, a));
            }

            return lines;
        }
    }
}