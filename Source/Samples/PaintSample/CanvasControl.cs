using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PaintSample
{
    internal class CanvasControl : Control
    {
        public CanvasControl()
        {
            UserPaint = true;
            Background = Brushes.White;
        }

        List<Point> points = new List<Point>();

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.DrawingContext;

            const double Radius = 5;
            foreach (var point in points)
                dc.FillEllipse(Brushes.Blue, new Rect(point.X - Radius, point.Y - Radius, Radius * 2, Radius * 2));
        }

        bool isPressed;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            CaptureMouse();
            isPressed = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            isPressed = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isPressed)
            {
                points.Add(e.GetPosition(this));
                Invalidate();
            }
        }
    }
}