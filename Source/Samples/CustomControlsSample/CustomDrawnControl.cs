using Alternet.UI;
using System;
using Alternet.Drawing;
using System.IO;
using System.Reflection;

namespace CustomControlsSample
{
    public class CustomDrawnControl : Control
    {
        private Brush brush = Brushes.LightGreen;

        Image image =
            new Image(Assembly.GetExecutingAssembly().GetManifestResourceStream("CustomControlsSample.Resources.Car.png")!);

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
                PerformLayout();
                Invalidate();
            }
        }

        public Brush Brush
        {
            get => brush;

            set
            {
                brush = value;
                Invalidate();
            }
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            using (var dc = CreateDrawingContext())
            {
                var textSize = dc.MeasureText(text, DefaultFont);
                var imageSize = image.Size;
                return new Size(Math.Max(textSize.Width, imageSize.Width), textSize.Height + imageSize.Height) + new Size(20, 20);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.DrawingContext.FillRectangle(brush, Handler.ClientRectangle);
            e.DrawingContext.DrawRectangle(Pens.Gray, Handler.ClientRectangle);
            e.DrawingContext.DrawText(text, DefaultFont, Brushes.Black, new Point(10, 10));
            e.DrawingContext.DrawImage(image, new Point(0, 10 + e.DrawingContext.MeasureText(text, DefaultFont).Height));
        }
    }
}