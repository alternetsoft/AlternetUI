using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;

namespace PaintSample
{
    internal class CanvasControl : Control
    {
        private List<Point[]> polylines = new List<Point[]>();

        private List<Point>? currentPolyline;

        public CanvasControl()
        {
            UserPaint = true;
            Background = Brushes.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.DrawingContext;
            foreach (var polyline in polylines)
                dc.DrawLines(Pens.Blue, polyline);
            if (currentPolyline != null)
                dc.DrawLines(Pens.Blue, currentPolyline.ToArray());
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            CaptureMouse();

            currentPolyline = new List<Point>();
            currentPolyline.Add(e.GetPosition(this));
            Invalidate();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            if (currentPolyline == null)
                throw new InvalidOperationException();

            polylines.Add(currentPolyline.ToArray());
            currentPolyline = null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (currentPolyline != null)
            {
                currentPolyline.Add(e.GetPosition(this));
                Invalidate();
            }
        }
    }
}