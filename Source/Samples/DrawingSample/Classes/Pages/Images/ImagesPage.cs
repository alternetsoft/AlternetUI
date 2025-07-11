using Alternet.Drawing;
using Alternet.UI;
using System;
using Alternet.UI.Extensions;

namespace DrawingSample
{
    internal sealed class ImagesPage : DrawingPage
    {
        private readonly Image image;
        private readonly AbstractControl control;
        private readonly Pen dashPen = new(Color.Black, 2, DashStyle.Dash);

        private RectD magnifiedRect;

        private InterpolationMode interpolationMode = InterpolationMode.None;

        public ImagesPage(AbstractControl control)
        {
            this.control = control;

            image = Resources.LeavesImage;

            var sizeDip = image.SizeDip(control);

            var rectSize = Math.Min(sizeDip.Width / 3, sizeDip.Height / 3);
            magnifiedRect = new RectD(
                new PointD(),
                new SizeD(rectSize, rectSize)).OffsetBy(
                    sizeDip.Width / 2 - rectSize / 2,
                    sizeDip.Height / 2 - rectSize / 2);
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

        public override void Draw(Graphics dc, RectD bounds)
        {
            var font = AbstractControl.DefaultFont;
            var textHeight = dc.MeasureText("M", font).Height;

            double spacing = 10;

            dc.DrawText($"Image size: {image.PixelSize.Width}x{image.PixelSize.Height} px", font, Brushes.Black, new PointD(spacing, spacing));

            var imageLocation = new PointD(spacing, spacing * 1.5 + textHeight);
            dc.DrawImage(image, imageLocation);
            dc.DrawRectangle(dashPen, magnifiedRect.OffsetBy(imageLocation.X, imageLocation.Y));

            var x = spacing;
            var y = image.SizeDip(control).Height + spacing * 4 + textHeight;

            var oldInterpolationMode = dc.InterpolationMode;

            dc.InterpolationMode = InterpolationMode;

            for (var factor = 1.0; factor <= 3; factor++)
            {
                dc.DrawText(factor * 100 + "%", font, Brushes.Black, new PointD(x, y));

                var sourceRect = new RectD(new PointD(x, y + textHeight + spacing / 2), magnifiedRect.Size * factor);
                var pixels = magnifiedRect.PixelFromDip(control.ScaleFactor);

                var subImage = image.GetSubBitmap(pixels);

                dc.DrawImage(subImage, sourceRect);

                x += spacing + sourceRect.Width;
            }

            dc.InterpolationMode = oldInterpolationMode;
        }

        protected override AbstractControl CreateSettingsControl()
        {
            var control = new ImagesPageSettings();
            control.Initialize(this);
            return control;
        }
    }
}