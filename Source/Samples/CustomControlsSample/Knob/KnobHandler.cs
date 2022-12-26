﻿#nullable disable

using Alternet.Drawing;
using Alternet.UI;
using System;

namespace CustomControlsSample
{
    public class KnobHandler : SliderHandler
    {
        private Pen knobBorderPen = new Pen(Color.Black, 2);

        private Pen largeTickPen = new Pen(Color.Black, 2);

        private Pen smallTickPen = new Pen(Color.Parse("#404040"), 2);

        private Pen knobPointerPen1 = new Pen(Color.Parse("#FC4154"), 3);

        private Pen knobPointerPen2 = new Pen(Color.Parse("#FF827D"), 1);

        private bool dragging = false;

        private Point dragStartPosition;

        public override SliderOrientation Orientation { get; set; }

        public override SliderTickStyle TickStyle { get; set; }

        protected override bool NeedsPaint => true;

        public override void OnPaint(DrawingContext dc)
        {
            var center = GetControlCenter();
            double controlRadius = GetControlRadius();
            var largeTickLength = controlRadius * 0.1;
            var smallTickLength = largeTickLength * 0.5;
            var knobPadding = largeTickLength * 0.5;
            var knobRadius = controlRadius - largeTickLength - knobPadding;

            using var knobGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(knobRadius * 2, knobRadius * 2),
                new[]
                {
                    new GradientStop(Color.Parse("#A9A9A9"), 0),
                    new GradientStop(Color.Parse("#676767"), 0.5),
                    new GradientStop(Color.Parse("#353535"), 1),
                });

            dc.FillCircle(knobGradientBrush, center, knobRadius);
            dc.DrawCircle(knobBorderPen, center, knobRadius);

            var emptyScaleSectorAngle = 70.0;
            var emptyScaleSectorHalfAngle = emptyScaleSectorAngle / 2;

            var scaleStartAngle = 90 + emptyScaleSectorHalfAngle;
            var scaleRange = 360 - emptyScaleSectorAngle;
            var scaleEndAngle = scaleStartAngle + scaleRange;

            var pointerAngle = MathUtil.MapRanges(
                Control.Value,
                Control.Minimum,
                Control.Maximum,
                scaleStartAngle,
                scaleEndAngle);

            const double DegreesToRadians = Math.PI / 180;

            Point GetScalePoint(double angle, double radius)
            {
                var radians = angle * DegreesToRadians;
                return center + new Size(radius * Math.Cos(radians), radius * Math.Sin(radians));
            }

            var pointerEndPoint1 = GetScalePoint(pointerAngle, knobRadius * 0.95);
            var pointerEndPoint2 = GetScalePoint(pointerAngle, knobRadius * 0.5);
            dc.DrawLine(knobPointerPen1, pointerEndPoint1, pointerEndPoint2);
            dc.DrawLine(knobPointerPen2, pointerEndPoint1, pointerEndPoint2);

            void DrawTicks(Pen pen, double step, double tickLength)
            {
                for (var angle = scaleStartAngle; angle <= scaleEndAngle; angle += step)
                    dc.DrawLine(pen, GetScalePoint(angle, controlRadius - tickLength), GetScalePoint(angle, controlRadius));
            }

            var largeTicksCount = 5;
            var smallTicksCount = largeTicksCount * 4;

            DrawTicks(smallTickPen, scaleRange / smallTicksCount, smallTickLength);
            DrawTicks(largeTickPen, scaleRange / largeTicksCount, largeTickLength);
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            return new Size(75, 75);
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            UserPaint = true;
            Control.ValueChanged += Control_ValueChanged;
            Control.MouseMove += Control_MouseMove;
            Control.MouseEnter += Control_MouseEnter;
            Control.MouseLeave += Control_MouseLeave;
            Control.MouseLeftButtonDown += Control_MouseLeftButtonDown;
            Control.MouseLeftButtonUp += Control_MouseLeftButtonUp;
            Control.MouseWheel += Control_MouseWheel;
        }

        protected override void OnDetach()
        {
            Control.ValueChanged -= Control_ValueChanged;
            Control.MouseMove -= Control_MouseMove;
            Control.MouseEnter -= Control_MouseEnter;
            Control.MouseLeave -= Control_MouseLeave;
            Control.MouseLeftButtonDown -= Control_MouseLeftButtonDown;
            Control.MouseLeftButtonUp -= Control_MouseLeftButtonUp;
            Control.MouseWheel -= Control_MouseWheel;

            base.OnDetach();
        }

        private double GetControlRadius()
        {
            var bounds = ClientRectangle;
            var gaugePadding = 1;
            return Math.Min(bounds.Width, bounds.Height) / 2 - gaugePadding;
        }

        private Point GetControlCenter()
        {
            return ClientRectangle.Center;
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                var location = e.GetPosition(Control);
                int opos = Control.Value;
                int pos = opos;
                var delta = dragStartPosition.Y - location.Y;
                pos += (int)delta;
                int min = Control.Minimum;
                int max = Control.Maximum;
                if (pos < min) pos = min;
                if (pos > max) pos = max;
                if (pos != opos)
                {
                    Control.Value = pos;
                    dragStartPosition = location;
                }
            }
        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CaptureMouse();

            var location = e.GetPosition(Control);

            SetFocus();
            if (MathUtil.IsPointInCircle(location, GetControlCenter(), GetControlRadius()))
            {
                dragStartPosition = location;
                dragging = true;
            }
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            dragging = false;
        }

        private void Control_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int pos;
            int m;
            var delta = e.Delta;
            if (0 < delta)
            {
                delta = 1;
                pos = Control.Value;
                pos += delta;
                m = Control.Maximum;
                if (pos > m)
                    pos = m;
                Control.Value = pos;
            }
            else if (0 > delta)
            {
                delta = -1;
                pos = Control.Value;
                pos += delta;
                m = Control.Minimum;
                if (pos < m)
                    pos = m;
                Control.Value = pos;
            }
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}