using Alternet.UI;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using Brush = Alternet.UI.Brush;
using Brushes = Alternet.UI.Brushes;
using Pens = Alternet.UI.Pens;

namespace HelloWorldSample
{
    public class CustomDrawnControl : Control
    {
        private Brush brush = Brushes.LightGreen;

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
            e.DrawingContext.FillRectangle(Handler.ClientRectangle, brush);
            e.DrawingContext.DrawRectangle(Handler.ClientRectangle, Pens.Gray);
            e.DrawingContext.DrawText(text, DefaultFont, Brushes.Black, new PointF(10, 10));
            e.DrawingContext.DrawImage(image, new PointF(0, 10 + e.DrawingContext.MeasureText(text, DefaultFont).Height));
        }
    }
}