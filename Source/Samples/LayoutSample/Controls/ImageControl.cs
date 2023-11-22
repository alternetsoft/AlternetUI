using Alternet.Drawing;
using Alternet.UI;
using System;

namespace LayoutSample
{
    internal class ImageControl : Control
    {
        private double zoom = 1;

        public ImageControl()
        {
            UserPaint = true;
        }

        public Image? Image { get; set; }

        public double Zoom
        {
            get => zoom;
            set
            {
                zoom = value;
                Handler.RaiseLayoutChanged();
                PerformLayout();
            }
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            var image = Image;
            if (image == null)
                return new Size();

            var size = image.Size;
            var zoom = Zoom;
            return new Size(size.Width * zoom, size.Height * zoom);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var bounds = ClientRectangle;
            e.DrawingContext.FillRectangle(Brushes.White, bounds);

            var image = Image;

            if (image != null)
                e.DrawingContext.DrawImage(image, bounds);
        }
    }
}