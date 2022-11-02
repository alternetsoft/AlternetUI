using Alternet.Drawing;
using Alternet.UI;
using System;
using System.ComponentModel;

namespace DrawingSample
{
    internal sealed class ImagesPage : DrawingPage
    {
        public override string Name => "Images";

        public ImagesPage()
        {
            image = Resources.LeavesImage;
            var rectSize = Math.Min(image.Size.Width / 3, image.Size.Height / 3);
            magnifiedRect = new Rect(
                new Point(),
                new Size(rectSize, rectSize)).OffsetBy(
                    image.Size.Width / 2 - rectSize / 2,
                    image.Size.Height / 2 - rectSize / 2);
        }

        Image image;
        Rect magnifiedRect;

        Pen dashPen = new Pen(Color.Black, 2, PenDashStyle.Dash);

        public override void Draw(DrawingContext dc, Rect bounds)
        {
            double spacing = 10;
            var imageLocation = new Point(spacing, spacing);
            dc.DrawImage(image, imageLocation);
            dc.DrawRectangle(dashPen, magnifiedRect.OffsetBy(imageLocation.X, imageLocation.Y));

            var x = spacing;
            var y = image.Size.Height + spacing * 4;

            var font = Control.DefaultFont;
            var textHeight = dc.MeasureText("M", font).Height;

            for (var factor = 1.0; factor <= 3; factor++)
            {
                dc.DrawText(factor * 100 + "%", font, Brushes.Black, new Point(x, y));

                var sourceRect = new Rect(new Point(x, y + textHeight + spacing / 2), magnifiedRect.Size * factor);
                dc.DrawImage(image, sourceRect, magnifiedRect);

                x += spacing + sourceRect.Width;
            }

        }

        protected override Control CreateSettingsControl()
        {
            var control = new ImagesPageSettings();
            control.Initialize(this);
            return control;
        }
    }
}