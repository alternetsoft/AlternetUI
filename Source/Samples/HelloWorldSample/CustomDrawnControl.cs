using Alternet.UI;
using System.Drawing;

namespace HelloWorldSample
{
    internal class CustomDrawnControl : Control
    {
        private string text = "";
        private Color color = Color.LightGreen;

        public CustomDrawnControl()
        {
            UserPaint = true;
        }

        public void SetText(string value)
        {
            text = value;
            Update();
        }

        public void SetColor(Color value)
        {
            color = value;
            Update();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.DrawingContext.FillRectangle(e.Bounds, color);
            e.DrawingContext.DrawRectangle(e.Bounds, Color.Gray);
            e.DrawingContext.DrawText("Custom Drawn Control", new PointF(10, 10), Color.Black);
            e.DrawingContext.DrawText(text, new PointF(10, 30), Color.Black);
        }
    }
}