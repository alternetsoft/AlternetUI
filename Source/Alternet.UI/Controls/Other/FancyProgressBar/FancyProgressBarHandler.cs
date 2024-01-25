#nullable disable

using System;
using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.UI
{
    internal class FancyProgressBarHandler : ProgressBarHandler
    {
        private readonly SolidBrush gaugeBackgroundBrush = new((Color)"#484854");

        private readonly Pen gaugeBorderPen = new((Color)"#9EAABA", 2);

        private readonly Font font = Font.Default;

        private readonly Pen pointerPen1 = new((Color)"#FC4154", 3);

        private readonly Pen pointerPen2 = new((Color)"#FF827D", 1);

        public override bool IsIndeterminate { get; set; }

        public override ProgressBarOrientation Orientation { get; set; }

        protected override bool NeedsPaint => true;

        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return new SizeD(200, 100);
        }

        public override void OnPaint(Graphics dc)
        {
            var bounds = Control.ClientRectangle;

            var gaugeBounds = bounds.InflatedBy(-2, -2);
            var scaleBounds = gaugeBounds.InflatedBy(-4, -4);

            dc.FillRectangle(gaugeBackgroundBrush, gaugeBounds);

            GradientStop[] gradientStops =
            [
                new GradientStop((Color)"#1B222C", 0),
                new GradientStop((Color)"#80767E", 0.5),
                new GradientStop((Color)"#0C1013", 1),
            ];

            using var scaleGradientBrush =
                new LinearGradientBrush(
                    new PointD(0, 0),
                    new PointD(0, scaleBounds.Height),
                    gradientStops);

            dc.FillRectangle(scaleGradientBrush, scaleBounds);

            dc.DrawRectangle(gaugeBorderPen, gaugeBounds);

            dc.Clip = new Region(scaleBounds);

            var fontMaxSize = dc.MeasureText(new string('M', Control.Maximum.ToString().Length), font);

            void DrawTicks(double ticksStartX, double offsetInSteps)
            {
                int step = 10;

                var y = bounds.Center.Y;
                var minY = y - (bounds.Height * 5);

                var yStep = MathUtils.MapRanges(step, Control.Minimum, Control.Maximum, 0, minY);

                y += offsetInSteps * yStep;

                var shift = -MathUtils.MapRanges(
                    Control.Value,
                    Control.Minimum,
                    Control.Maximum,
                    0,
                    minY);

                for (int tickValue = Control.Minimum; tickValue <= Control.Maximum; tickValue += step)
                {
                    double value = tickValue + (step * offsetInSteps);
                    if (value > Control.Maximum)
                        break;

                    var startPoint = new PointD(ticksStartX, y + shift);
                    dc.DrawLine(Pens.White, startPoint, new PointD(scaleBounds.Right, y + shift));

                    dc.DrawText(value.ToString(), font, Brushes.White, startPoint - new SizeD(fontMaxSize.Width * 0.6, fontMaxSize.Height / 2));

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