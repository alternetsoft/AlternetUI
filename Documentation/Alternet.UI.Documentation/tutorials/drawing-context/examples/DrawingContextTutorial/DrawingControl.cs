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

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightBlue, e.ClipRectangle);

            for (int size = 10; size < 200; size += 10)
                e.Graphics.DrawEllipse(Pens.Red, new(10, 10, size, size));
        }
    }
}