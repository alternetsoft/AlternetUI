using Alternet.UI;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace HelloWorldSample
{
    public class CustomDrawnControl : Control
    {
        private Color color = Color.LightGreen;

        Alternet.UI.Image image =
            new Alternet.UI.Image(Assembly.GetExecutingAssembly().GetManifestResourceStream("HelloWorldSample.Resources.Car.png")!);

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
            {
                var textSize = dc.MeasureText(text, DefaultFont);
                var imageSize = image.Size;
                return new SizeF(Math.Max(textSize.Width, imageSize.Width), textSize.Height + imageSize.Height) + new SizeF(20, 20);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.DrawingContext.FillRectangle(Handler.ClientRectangle, color);
            e.DrawingContext.DrawRectangle(Handler.ClientRectangle, Color.Gray);
            e.DrawingContext.DrawText(text, new PointF(10, 10), DefaultFont, Color.Black);
            e.DrawingContext.DrawImage(image, new PointF(0, 10 + e.DrawingContext.MeasureText(text, DefaultFont).Height));
        }
    }
}