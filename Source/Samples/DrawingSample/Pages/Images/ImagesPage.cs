using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    internal sealed class ImagesPage : DrawingPage
    {
        private Image image;

        private Rect magnifiedRect;

        private Pen dashPen = new Pen(Color.Black, 2, DashStyle.Dash);

        private InterpolationMode interpolationMode = InterpolationMode.None;

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

        public override string Name => "Images";

        public InterpolationMode InterpolationMode
        {
            get => interpolationMode;
            set
            {
                interpolationMode = value;
                Canvas?.Invalidate();
            }
        }

        public override void Draw(DrawingContext dc, Rect bounds)
        {
            var font = Control.DefaultFont;
            var textHeight = dc.MeasureText("M", font).Height;

            double spacing = 10;

            dc.DrawText($"Image size: {image.PixelSize.Width}x{image.PixelSize.Height} px", font, Brushes.Black, new Point(spacing, spacing));

            var imageLocation = new Point(spacing, spacing * 1.5 + textHeight);
            dc.DrawImage(image, imageLocation);
            dc.DrawRectangle(dashPen, magnifiedRect.OffsetBy(imageLocation.X, imageLocation.Y));

            var x = spacing;
            var y = image.Size.Height + spacing * 4 + textHeight;

            var oldInterpolationMode = dc.InterpolationMode;

            dc.InterpolationMode = InterpolationMode;

            for (var factor = 1.0; factor <= 3; factor++)
            {
                dc.DrawText(factor * 100 + "%", font, Brushes.Black, new Point(x, y));

                var sourceRect = new Rect(new Point(x, y + textHeight + spacing / 2), magnifiedRect.Size * factor);
                dc.DrawImage(image, sourceRect, magnifiedRect);

                x += spacing + sourceRect.Width;
            }

            dc.InterpolationMode = oldInterpolationMode;
        }

        protected override Control CreateSettingsControl()
        {
            var control = new ImagesPageSettings();
            control.Initialize(this);
            return control;
        }
    }
}