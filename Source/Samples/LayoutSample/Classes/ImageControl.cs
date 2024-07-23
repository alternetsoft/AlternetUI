using Alternet.Drawing;
using Alternet.UI;
using System;
using Alternet.UI.Extensions;

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
                PerformLayout();
            }
        }

        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            var image = Image;
            if (image == null)
                return new SizeD();

            var size = image.SizeDip(this);
            var zoom = Zoom;
            return new SizeD(size.Width * zoom, size.Height * zoom);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var bounds = ClientRectangle;
            e.Graphics.FillRectangle(Brushes.White, bounds);

            var image = Image;

            if (image != null)
                e.Graphics.DrawImage(image, bounds);
        }
    }
}