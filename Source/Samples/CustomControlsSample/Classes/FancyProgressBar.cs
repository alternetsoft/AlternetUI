using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ProgressBar"/> with custom painted fancy look.
    /// </summary>
    public partial class FancyProgressBar : ProgressBar
    {
        private readonly Font font = Control.DefaultFont;
        private readonly Pen pointerPen1 = new((Color)"#FC4154", 3);
        private readonly Pen pointerPen2 = new((Color)"#FF827D", 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="FancyProgressBar"/> class.
        /// </summary>
        public FancyProgressBar()
        {
            UserPaint = true;
            ValueChanged += Control_ValueChanged;
            BackgroundColor = (Color)"#484854";
        }

        /// <inheritdoc/>
        public override bool IsIndeterminate { get; set; }

        /// <inheritdoc/>
        public override ProgressBarOrientation Orientation { get; set; }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return new SizeD(200, 100);
        }

        /// <inheritdoc/>
        protected override HandlerType GetRequiredHandlerType() => HandlerType.Generic;

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            var bounds = e.ClipRectangle;
            var dc = e.Graphics;

            var scaleBounds = bounds.InflatedBy(-2, -2);

            dc.FillRectangle(BackColor.AsBrush, bounds);

            /*
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

            */

            dc.DrawBorderWithBrush(DefaultColors.GetControlBorderBrush(this), bounds);

            var fontMaxSize = dc.MeasureText(
                new string('M', Maximum.ToString().Length),
                font);

            dc.DoInsideClipped(
                scaleBounds,
                () =>
                {
                    DrawTicks(scaleBounds.Left + (scaleBounds.Width * 0.45f), 0);
                    DrawTicks(scaleBounds.Left + (scaleBounds.Width * 0.7f), 0.5f);

                    var pointerLineStartPoint = new PointD(scaleBounds.Left, bounds.Center.Y);
                    var pointerLineEndPoint = new PointD(scaleBounds.Right, bounds.Center.Y);
                    dc.DrawLine(pointerPen1, pointerLineStartPoint, pointerLineEndPoint);
                    dc.DrawLine(pointerPen2, pointerLineStartPoint, pointerLineEndPoint);
                });

            void DrawTicks(Coord ticksStartX, Coord offsetInSteps)
            {
                int step = 10;

                var y = bounds.Center.Y;
                var minY = y - (bounds.Height * 5);

                var yStep = MapRanges(step, Minimum, Maximum, 0, minY);

                y += offsetInSteps * yStep;

                var shift = -MapRanges(
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
                        startPoint - new SizeD(fontMaxSize.Width * 0.6f, fontMaxSize.Height / 2));

                    y += yStep;
                }
            }
        }

        internal static Coord MapRanges(
            Coord value,
            Coord from1,
            Coord to1,
            Coord from2,
            Coord to2) =>
            ((value - from1) / (to1 - from1) * (to2 - from2)) + from2;

        private void Control_ValueChanged(object? sender, EventArgs e)
        {
            Refresh();
        }
    }
}