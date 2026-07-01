using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a fancy slider control which can be rotated like a knob.
    /// </summary>
    public partial class KnobSlider : StdSlider
    {
        private bool dragging = false;
        private PointD dragStartPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnobSlider"/> class.
        /// </summary>
        public KnobSlider()
        {
            SuggestedSize = new SizeD(100, 100);
            ParentBackColor = true;
            ParentForeColor = true;
            ForEachChild(c => c.Hide());
            UserPaint = true;
        }

        /// <inheritdoc/>
        public override SliderOrientation Orientation { get; set; }

        /// <inheritdoc/>
        public override SliderTickStyle TickStyle { get; set; }

        /// <inheritdoc/>
        public override SizeD SuggestedSize
        {
            get
            {
                var size = base.SuggestedSize.Width;

                if(float.IsNaN(size) || size < 70)
                    size = 70;

                return new SizeD(size, size);
            }

            set
            {
                base.SuggestedSize = value;
            }
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
        protected override SizeD GetPreferredSizeInternal(PreferredSizeContext context)
        {
            return (SuggestedSize.Width, SuggestedSize.Width);
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

            DrawDefaultBackground(e);

            dc.DrawRoundedRectangle(DefaultColors.GetControlBorderColor(this).AsPen, bounds, 10);

            var center = GetControlCenter();
            var controlRadius = GetControlRadius();
            var largeTickLength = controlRadius * 0.1f;
            var smallTickLength = largeTickLength * 0.5f;
            var knobPadding = largeTickLength * 0.5f;
            var knobRadius = controlRadius - largeTickLength - knobPadding;

            dc.DrawCircle(ForeColor.GetAsPen(1), center, knobRadius);

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
            dc.DrawLine(LightDarkColors.Red.GetAsPen(3), pointerEndPoint1, pointerEndPoint2);

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

            DrawTicks(ForeColor.GetAsPen(2), scaleRange / smallTicksCount, smallTickLength);
            DrawTicks(ForeColor.GetAsPen(2), scaleRange / largeTicksCount, largeTickLength);
        }

        private static float MapRanges(
            float value,
            float from1,
            float to1,
            float from2,
            float to2) =>
            ((value - from1) / (to1 - from1) * (to2 - from2)) + from2;
    }
}