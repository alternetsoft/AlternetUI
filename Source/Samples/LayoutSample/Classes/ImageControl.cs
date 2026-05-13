using Alternet.Drawing;
using Alternet.UI;
using System;
using Alternet.UI.Extensions;

namespace LayoutSample
{
    internal class ImageControl : HiddenBorder
    {
        private Coord zoom = 1;
        private Image? image;

        public ImageControl()
        {
            UserPaint = true;
        }

        public override RectD Bounds
        {
            get => base.Bounds;
            set
            {
                if (base.Bounds == value)
                    return;
                base.Bounds = value;
            }
        }

        public override SizeD SuggestedSize
        {
            get
            {
                return GetZoomedImageSize();
            }

            set
            {
            }
        }

        public Image? Image
        {
            get => image;
            set
            {
                if (value == image)
                    return;
                image = value;
                PerformLayoutAndInvalidate();
            }
        }

        public Coord Zoom
        {
            get => zoom;
            set
            {
                if (zoom == value)
                    return;
                zoom = value;
                PerformLayoutAndInvalidate();
            }
        }

        public SizeD GetZoomedImageSize()
        {
            var image = Image;
            if (image == null)
                return SizeD.Empty;

            var size = image.SizeDip(this);
            var zoom = Zoom;
            var result = new SizeD(size.Width * zoom, size.Height * zoom);
            return result;
        }

        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            return GetZoomedImageSize();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            var image = Image;

            if (image != null)
            {
                e.Graphics.FillRectangle(Color.Moccasin.AsBrush, e.ClientRectangle);
                e.Graphics.DrawRectangle(LightDarkColors.Green.AsPen, e.ClientRectangle);
                var size = image.SizeDip(this);
                var bounds = ((0, 0), new SizeD(size.Width * zoom, size.Height * zoom));
                e.Graphics.FillRectangle(DefaultColors.WindowBackColor.AsBrush, bounds);
                e.Graphics.DrawImage(image, bounds);
                e.Graphics.DrawRectangle(LightDarkColors.Red.AsPen, bounds);
            }
        }
    }
}