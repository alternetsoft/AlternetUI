using Alternet.Drawing;
using System;
using System.Collections.Generic;

namespace PaintSample
{
    internal sealed class Polyline : Shape
    {
        private readonly List<Point> points = new List<Point>();

        public Polyline(Pen pen, Point initialPoint)
        {
            Pen = pen;
            points.Add(initialPoint);
        }

        public void AddPoint(Point point)
        {
            points.Add(point);
            RaiseChanged();
        }

        public override void Paint(DrawingContext dc)
        {
            if (Pen == null)
                throw new InvalidOperationException();

            dc.DrawLines(Pen, points.ToArray());
        }
    }
}