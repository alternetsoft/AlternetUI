using Alternet.Drawing;
using Alternet.UI;

namespace WindowPropertiesSample
{
    internal class SizeGridLinesControl : Control
    {
        public SizeGridLinesControl()
        {
            UserPaint = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.DrawingContext;
            var bounds = e.Bounds;
            const double Step = 50;
            var pen = Pens.LightGray;

            for (double x = 0; x < bounds.Width; x += Step)
                dc.DrawLine(pen, new Point(x, bounds.Top), new Point(x, bounds.Bottom));

            for (double y = bounds.Y; y < bounds.Bottom; y += Step)
                dc.DrawLine(pen, new Point(bounds.Left, y), new Point(bounds.Right, y));
        }
    }
}