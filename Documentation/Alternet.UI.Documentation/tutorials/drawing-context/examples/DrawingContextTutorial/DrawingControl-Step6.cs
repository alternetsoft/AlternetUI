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

            float y = 210;
            for (int month = 3; month <= 5; month++)
            {
                var text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                var textSize = e.DrawingContext.MeasureText(text, font);
                e.DrawingContext.DrawText(text, font, Brushes.Black, new(0, y, textSize.Width, textSize.Height));
                y += textSize.Height + 10;
            }
        }
    }
}