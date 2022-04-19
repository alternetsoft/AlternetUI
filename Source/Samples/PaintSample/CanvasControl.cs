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
        }

        List<Point> points = new List<Point>();

        protected override void OnPaint(PaintEventArgs e)
        {
            e.DrawingContext.FillRectangle(Brushes.White, e.Bounds);

            const double Radius = 5;
            foreach (var point in points)
                e.DrawingContext.FillEllipse(Brushes.Blue, new Rect(point.X - Radius, point.Y - Radius, Radius * 2, Radius * 2));
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