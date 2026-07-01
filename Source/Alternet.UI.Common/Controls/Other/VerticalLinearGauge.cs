using System;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a vertical linear gauge control. It has a vertical orientation and displays
    /// a value within a specified range.
    /// </summary>
    public partial class VerticalLinearGauge : StdProgressBar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalLinearGauge"/> class.
        /// </summary>
        public VerticalLinearGauge()
        {
            SuggestedSize = new SizeD(100, 100);
        }

        /// <inheritdoc/>
        public override bool IsIndeterminate { get; set; }

        /// <inheritdoc/>
        public override SizeD SuggestedSize
        {
            get
            {
                return base.SuggestedSize;
            }

            set
            {
                if (value.IsNanWidthOrHeight)
                    return;

                base.SuggestedSize = value;
            }
        }

        /// <inheritdoc/>
        public override ProgressBarOrientation Orientation { get; set; }

        /// <inheritdoc/>
        protected override SizeD GetPreferredSizeInternal(PreferredSizeContext context)
        {
            return SuggestedSize;
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            var bounds = e.ClientRectangle;
            var dc = e.Graphics;

            var scaleBounds = bounds.InflatedBy(-2, -2);

            var fontMaxSize = dc.MeasureText(
                new string('M', Maximum.ToString().Length), RealFont);

            dc.DoInsideClipped(
                scaleBounds,
                () =>
                {
                    DrawTicks(scaleBounds.Left + (scaleBounds.Width * 0.45f), 0);
                    DrawTicks(scaleBounds.Left + (scaleBounds.Width * 0.7f), 0.5f);

                    var pointerLineStartPoint = new PointD(scaleBounds.Left, bounds.Center.Y);
                    var pointerLineEndPoint = new PointD(scaleBounds.Right, bounds.Center.Y);
                    dc.DrawLine(LightDarkColors.Red.GetAsPen(3), pointerLineStartPoint, pointerLineEndPoint);
                });

            dc.DrawRoundedRectangle(DefaultColors.GetControlBorderColor(this).AsPen, bounds, 10);

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
                    dc.DrawLine(ForeColor.AsPen, startPoint, new PointD(scaleBounds.Right, y + shift));

                    dc.DrawText(
                        value.ToString(),
                        RealFont,
                        ForeColor.AsBrush,
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
    }
}