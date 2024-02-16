﻿using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ProgressBar"/> with custom painted fancy look.
    /// </summary>
    public partial class FancyProgressBar : ProgressBar
    {
        private readonly SolidBrush gaugeBackgroundBrush = new((Color)"#484854");

        private readonly Pen gaugeBorderPen = new((Color)"#9EAABA", 2);

        private readonly Font font = Font.Default;

        private readonly Pen pointerPen1 = new((Color)"#FC4154", 3);

        private readonly Pen pointerPen2 = new((Color)"#FF827D", 1);

        internal new FancyProgressBarHandler Handler => (FancyProgressBarHandler)base.Handler;

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            var bounds = e.Bounds;
            var dc = e.DrawingContext;

            var gaugeBounds = bounds.InflatedBy(-2, -2);
            var scaleBounds = gaugeBounds.InflatedBy(-4, -4);

            dc.FillRectangle(gaugeBackgroundBrush, gaugeBounds);

            GradientStop[] gradientStops =
            {
                new((Color)"#1B222C", 0),
                new((Color)"#80767E", 0.5),
                new((Color)"#0C1013", 1),
            };

            using var scaleGradientBrush =
                new LinearGradientBrush(
                    new PointD(0, 0),
                    new PointD(0, scaleBounds.Height),
                    gradientStops);

            dc.FillRectangle(scaleGradientBrush, scaleBounds);

            dc.DrawRectangle(gaugeBorderPen, gaugeBounds);

            dc.Clip = new Region(scaleBounds);

            var fontMaxSize = dc.MeasureText(
                new string('M', Maximum.ToString().Length),
                font);

            void DrawTicks(double ticksStartX, double offsetInSteps)
            {
                int step = 10;

                var y = bounds.Center.Y;
                var minY = y - (bounds.Height * 5);

                var yStep = MathUtils.MapRanges(step, Minimum, Maximum, 0, minY);

                y += offsetInSteps * yStep;

                var shift = -MathUtils.MapRanges(
                    Value,
                    Minimum,
                    Maximum,
                    0,
                    minY);

                for (
                    int tickValue = Minimum;
                    tickValue <= Maximum;
                    tickValue += step)
                {
                    double value = tickValue + (step * offsetInSteps);
                    if (value > Maximum)
                        break;

                    var startPoint = new PointD(ticksStartX, y + shift);
                    dc.DrawLine(Pens.White, startPoint, new PointD(scaleBounds.Right, y + shift));

                    dc.DrawText(
                        value.ToString(),
                        font,
                        Brushes.White,
                        startPoint - new SizeD(fontMaxSize.Width * 0.6, fontMaxSize.Height / 2));

                    y += yStep;
                }
            }

            DrawTicks(scaleBounds.Left + (scaleBounds.Width * 0.45), 0);
            DrawTicks(scaleBounds.Left + (scaleBounds.Width * 0.7), 0.5);

            var pointerLineStartPoint = new PointD(scaleBounds.Left, bounds.Center.Y);
            var pointerLineEndPoint = new PointD(scaleBounds.Right, bounds.Center.Y);
            dc.DrawLine(pointerPen1, pointerLineStartPoint, pointerLineEndPoint);
            dc.DrawLine(pointerPen2, pointerLineStartPoint, pointerLineEndPoint);
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return new SizeD(200, 100);
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return new FancyProgressBarHandler();
        }

        internal class FancyProgressBarHandler : ProgressBarHandler
        {
            public override bool IsIndeterminate { get; set; }

            public override ProgressBarOrientation Orientation { get; set; }

            protected override void OnAttach()
            {
                base.OnAttach();
                Control.UserPaint = true;
                Control.ValueChanged += Control_ValueChanged;
            }

            protected override void OnDetach()
            {
                Control.ValueChanged -= Control_ValueChanged;

                base.OnDetach();
            }

            private void Control_ValueChanged(object sender, EventArgs e)
            {
                Control.Refresh();
            }
        }
    }
}