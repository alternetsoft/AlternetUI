using Alternet.Drawing;
using Alternet.UI;
using System;
using Alternet.UI.Extensions;

namespace LayoutSample
{
    internal class ImageControl : HiddenBorder
    {
        private Coord zoom = 1;

        public ImageControl()
        {
            UserPaint = true;
        }

        public Image? Image { get; set; }

        public Coord Zoom
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

            var image = Image;

            if (image != null)
            {
                var size = image.SizeDip(this);
                var bounds = ((0, 0), new SizeD(size.Width * zoom, size.Height * zoom));
                e.Graphics.FillRectangle(DefaultColors.WindowBackColor.AsBrush, bounds);
                e.Graphics.DrawImage(image, bounds);
                e.Graphics.DrawRectangle(LightDarkColors.Red.AsPen, bounds);
            }
        }
    }
}