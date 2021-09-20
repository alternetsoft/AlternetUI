using Alternet.UI;
using System;
using Alternet.Drawing;
using System.IO;
using System.Reflection;

namespace HelloWorldSample
{
    public class CustomDrawnControl : Control
    {
        private Brush brush = Brushes.LightGreen;

        Image image =
            new Image(Assembly.GetExecutingAssembly().GetManifestResourceStream("HelloWorldSample.Resources.Car.png")!);

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

        public Brush Brush
        {
            get => brush;

            set
            {
                brush = value;
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
            e.DrawingContext.FillRectangle(brush, Handler.ClientRectangle);
            e.DrawingContext.DrawRectangle(Pens.Gray, Handler.ClientRectangle);
            e.DrawingContext.DrawText(text, DefaultFont, Brushes.Black, new PointF(10, 10));
            e.DrawingContext.DrawImage(image, new PointF(0, 10 + e.DrawingContext.MeasureText(text, DefaultFont).Height));
        }
    }
}