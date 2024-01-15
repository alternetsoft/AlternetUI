using Alternet.Drawing;
using Alternet.UI;

namespace DrawingContextTutorial
{
    public class DrawingControl : Control
    {
        public DrawingControl()
        {
            UserPaint = true;
        }

        private static Font font = new Font(FontFamily.GenericSerif, 15);

        protected override void OnPaint(PaintEventArgs e)
        {
            e.DrawingContext.FillRectangle(Brushes.LightBlue, e.Bounds);

            for (int size = 10; size < 200; size += 10)
                e.DrawingContext.DrawEllipse(Pens.Red, new(10, 10, size, size));

            e.DrawingContext.DrawText("Hello!", font, Brushes.Black, new PointF(10, 220));
        }
    }
}