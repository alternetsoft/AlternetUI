#nullable disable

using Alternet.Drawing;
using Alternet.UI;
using System;
using System.ComponentModel;

namespace CustomControlsSample.Gauge
{
    public class GaugeHandler : ProgressBarHandler
    {
        protected override bool NeedsPaint => true;

        public override bool IsIndeterminate { get; set; }
        public override ProgressBarOrientation Orientation { get; set; }

        public override Size GetPreferredSize(Size availableSize)
        {
            return new Size(100, 100);
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            UserPaint = true;
            Control.ValueChanged += Control_ValueChanged;
        }

        protected override void OnDetach()
        {
            Control.ValueChanged -= Control_ValueChanged;

            base.OnDetach();
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        static double MapRanges(double value, double from1, double to1, double from2, double to2) =>
            (value - from1) / (to1 - from1) * (to2 - from2) + from2;

        public override void OnPaint(DrawingContext dc)
        {
            var bounds = ClientRectangle;

            var gaugeBounds = bounds.InflatedBy(-2, -2);
            var scaleBounds = gaugeBounds.InflatedBy(-4, -4);

            dc.FillRectangle(new SolidBrush(Color.Parse("#484854")), gaugeBounds);
            var scaleGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, scaleBounds.Height),
                            new[]
                            {
                                new GradientStop(Color.Parse("#1B222C"), 0),
                                new GradientStop(Color.Parse("#80767E"), 0.5),
                                new GradientStop(Color.Parse("#0C1013"), 1),
                            });
            dc.FillRectangle(scaleGradientBrush, scaleBounds);
            dc.DrawRectangle(new Pen(Color.Parse("#9EAABA"), 2), gaugeBounds);

            dc.Clip = new Region(scaleBounds);

            var font = Alternet.UI.Control.DefaultFont;

            var fontMaxSize = dc.MeasureText(new string('M', Control.Maximum.ToString().Length), font);

            void DrawTicks(double ticksStartX, double offsetInSteps)
            {
                int step = 10;

                var y = bounds.Center.Y;
                var minY = y - bounds.Height * 5;

                var yStep = MapRanges(step, Control.Minimum, Control.Maximum, 0, minY);

                y += offsetInSteps * yStep;

                var shift = -MapRanges(Control.Value, Control.Minimum, Control.Maximum, 0, minY);

                for (int tickValue = Control.Minimum; tickValue <= Control.Maximum; tickValue += step)
                {
                    double value = tickValue + step * offsetInSteps;
                    if (value > Control.Maximum)
                        break;

                    var startPoint = new Point(ticksStartX, y + shift);
                    dc.DrawLine(Pens.White, startPoint, new Point(scaleBounds.Right, y + shift));

                    dc.DrawText(value.ToString(), font, Brushes.White, startPoint - new Size(fontMaxSize.Width * 0.6, fontMaxSize.Height / 2));

                    y += yStep;
                }
            }

            DrawTicks(scaleBounds.Left + scaleBounds.Width * 0.45, 0);
            DrawTicks(scaleBounds.Left + scaleBounds.Width * 0.7, 0.5);

            var pointerLineStartPoint = new Point(scaleBounds.Left, bounds.Center.Y);
            var pointerLineEndPoint = new Point(scaleBounds.Right, bounds.Center.Y);
            dc.DrawLine(new Pen(Color.Parse("#FC4154"), 3), pointerLineStartPoint, pointerLineEndPoint);
            dc.DrawLine(new Pen(Color.Parse("#FF827D"), 1), pointerLineStartPoint, pointerLineEndPoint);
        }
    }
}