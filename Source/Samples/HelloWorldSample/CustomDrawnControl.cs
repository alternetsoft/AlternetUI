using Alternet.UI;
using System.Drawing;

namespace HelloWorldSample
{
    public class CustomDrawnControl : Control
    {
        private Color color = Color.LightGreen;

        public CustomDrawnControl()
        {
            UserPaint = true;
        }

        private string text = "";

        public string Text
        {
            get => text;
            set
            {
                text = value;
                Parent?.PerformLayout();
                Update();
            }
        }

        public Color Color
        {
            get => color;

            set
            {
                color = value;
                Update();
            }
        }

        public override SizeF GetPreferredSize(SizeF availableSize)
        {
            using (var dc = CreateDrawingContext())
                return dc.MeasureText(text) + new SizeF(20, 20);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.DrawingContext.FillRectangle(e.Bounds, color);
            e.DrawingContext.DrawRectangle(e.Bounds, Color.Gray);
            e.DrawingContext.DrawText(text, new PointF(10, 10), Color.Black);
        }
    }
}