using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="StdSlider"/> with custom painted fancy look.
    /// </summary>
    public partial class FancySlider : StdSlider
    {
        private readonly SolidBrush gaugeBackgroundBrush = new((Color)"#484854");
        private readonly Pen knobBorderPen = new(Color.Black, 2);
        private readonly Pen largeTickPen = new(Color.Black, 2);
        private readonly Pen smallTickPen = new((Color)"#FFFFFF", 2);
        private readonly Pen knobPointerPen1 = new((Color)"#FC4154", 3);
        private readonly Pen knobPointerPen2 = new((Color)"#FF827D", 1);
        private bool dragging = false;
        private PointD dragStartPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="FancySlider"/> class.
        /// </summary>
        public FancySlider()
        {
            ForEachChild(c => c.Hide());
            UserPaint = true;
            ValueChanged += Control_ValueChanged;            
        }

        /// <inheritdoc/>
        public override SliderOrientation Orientation { get; set; }

        /// <inheritdoc/>
        public override SliderTickStyle TickStyle { get; set; }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            return new SizeD(100, 100);
        }

        internal Coord GetControlRadius()
        {
            var bounds = ClientRectangle;
            var gaugePadding = 10;
            return (Math.Min(bounds.Width, bounds.Height) / 2) - gaugePadding;
        }

        internal PointD GetControlCenter()
        {
            return ClientRectangle.Center;
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(EventArgs e)
        {
            Refresh();
        }

        /// <inheritdoc/>
        protected override void OnMouseEnter(EventArgs e)
        {
            Refresh();
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (dragging)
            {
                var location = e.Location;
                int oldPos = Value;
                int pos = oldPos;
                
                var deltaY = dragStartPosition.Y - location.Y;
                var deltaX = dragStartPosition.X - location.X;

                double delta;

                if (Math.Abs(deltaY) > Math.Abs(deltaX))
                {
                    delta = deltaY;
                }
                else
                {
                    delta = deltaX;
                }

                pos += (int)delta;
                int min = Minimum;
                int max = Maximum;
                if (pos < min) pos = min;
                if (pos > max) pos = max;
                if (pos != oldPos)
                {
                    Value = pos;
                    dragStartPosition = location;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            CaptureMouse();

            var location = e.Location;

            SetFocus();
            if (DrawingUtils.IsPointInCircle(
                location,
                GetControlCenter(),
                GetControlRadius()))
            {
                dragStartPosition = location;
                dragging = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonUp(MouseEventArgs e)
        {
            ReleaseMouseCapture();

            dragging = false;
        }

        /// <inheritdoc/>
        protected override HandlerType GetRequiredHandlerType() => HandlerType.Generic;

        /// <inheritdoc/>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int pos;
            int m;
            var delta = e.Delta;
            if (delta > 0)
            {
                delta = 1;
                pos = Value;
                pos += delta;
                m = Maximum;
                if (pos > m)
                    pos = m;
                Value = pos;
            }
            else if (delta < 0)
            {
                delta = -1;
                pos = Value;
                pos += delta;
                m = Minimum;
                if (pos < m)
                    pos = m;
                Value = pos;
            }
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            var bounds = e.ClientRectangle;
            var dc = e.Graphics;

            dc.FillRectangle(gaugeBackgroundBrush, bounds);

            /*
            var scaleBounds = gaugeBounds.InflatedBy(-4, -4);
            var gradientStops1 = new[]
            {
                new GradientStop((Color)"#1B222C", 0),
                new GradientStop((Color)"#80767E", 0.5),
                new GradientStop((Color)"#0C1013", 1),
            };

            using var scaleGradientBrush = new LinearGradientBrush(
                PointD.Empty,
                new PointD(0, scaleBounds.Height),
                gradientStops1);

            dc.FillRectangle(scaleGradientBrush, scaleBounds);
            */

            dc.DrawBorderWithBrush(DefaultColors.GetControlBorderBrush(this), bounds);
                
            var center = GetControlCenter();
            var controlRadius = GetControlRadius();
            var largeTickLength = controlRadius * 0.1f;
            var smallTickLength = largeTickLength * 0.5f;
            var knobPadding = largeTickLength * 0.5f;
            var knobRadius = controlRadius - largeTickLength - knobPadding;

            /*
            var gradientStops2 = new[]
            {
                    new GradientStop((Color)"#A9A9A9", 0),
                    new GradientStop((Color)"#676767", 0.5),
                    new GradientStop((Color)"#353535", 1),
            };

            using var knobGradientBrush = new LinearGradientBrush(
                PointD.Empty,
                new PointD(knobRadius * 2, knobRadius * 2),
                gradientStops2);

            dc.FillCircle(knobGradientBrush, center, knobRadius);
            */

            dc.DrawCircle(knobBorderPen, center, knobRadius);

            var emptyScaleSectorAngle = 70.0f;
            var emptyScaleSectorHalfAngle = emptyScaleSectorAngle / 2;

            var scaleStartAngle = 90 + emptyScaleSectorHalfAngle;
            var scaleRange = 360 - emptyScaleSectorAngle;
            var scaleEndAngle = scaleStartAngle + scaleRange;

            var pointerAngle = MapRanges(
                Value,
                Minimum,
                Maximum,
                scaleStartAngle,
                scaleEndAngle);

            const float DegreesToRadians = MathF.PI / 180;

            PointD GetScalePoint(Coord angle, Coord radius)
            {
                var radians = angle * DegreesToRadians;
                return center + new SizeD(radius * MathF.Cos(radians), radius * MathF.Sin(radians));
            }

            var pointerEndPoint1 = GetScalePoint(pointerAngle, knobRadius * 0.95f);
            var pointerEndPoint2 = GetScalePoint(pointerAngle, knobRadius * 0.5f);
            dc.DrawLine(knobPointerPen1, pointerEndPoint1, pointerEndPoint2);
            dc.DrawLine(knobPointerPen2, pointerEndPoint1, pointerEndPoint2);

            void DrawTicks(Pen pen, float step, float tickLength)
            {
                for (var angle = scaleStartAngle; angle <= scaleEndAngle; angle += step)
                {
                    dc.DrawLine(
                        pen,
                        GetScalePoint(angle, controlRadius - tickLength),
                        GetScalePoint(angle, controlRadius));
                }
            }

            var largeTicksCount = 5;
            var smallTicksCount = largeTicksCount * 4;

            DrawTicks(smallTickPen, scaleRange / smallTicksCount, smallTickLength);
            DrawTicks(largeTickPen, scaleRange / largeTicksCount, largeTickLength);
        }

        private static float MapRanges(
            float value,
            float from1,
            float to1,
            float from2,
            float to2) =>
            ((value - from1) / (to1 - from1) * (to2 - from2)) + from2;

        private void Control_ValueChanged(object? sender, EventArgs e)
        {
            Refresh();
        }
    }
}