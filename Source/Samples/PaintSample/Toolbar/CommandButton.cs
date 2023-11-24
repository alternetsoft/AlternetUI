using Alternet.Drawing;
using Alternet.UI;
using System;

namespace PaintSample
{
    public class CommandButton : Control
    {
        private bool isPressed;

        public CommandButton(string imageName)
        {
            UserPaint = true;
            this.imageName = imageName;
            image = LoadImage();
        }

        private Image LoadImage()
        {
            var stream = GetType().Assembly.GetManifestResourceStream(
                "PaintSample.Resources.CommandIcons." + imageName + ".png");
            if (stream == null)
                throw new InvalidOperationException();

            return new Bitmap(stream);
        }

        private readonly Image image;
        private readonly string imageName;

        private bool IsPressed
        {
            get => isPressed;
            set
            {
                if (isPressed == value)
                    return;

                isPressed = value;
                Invalidate();
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            return new Size(30, 30);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            CaptureMouse();
            if (Enabled)
                IsPressed = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            IsPressed = false;

            if (IsMouseOver && Enabled)
            {
                RaiseClick(EventArgs.Empty);
            }
        }

        protected override void OnMouseEnter()
        {
            Refresh();
        }

        protected override void OnMouseLeave()
        {
            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.DrawingContext;

            var innerRect = e.Bounds;
            innerRect.Inflate(-2, -2);

            if (IsPressed) 
            {
                innerRect.Offset(1, 1);
            }
            else
            {
                //var shadowRect = innerRect;
                //shadowRect.Offset(1, 1);
                //dc.FillRectangle(Brushes.Black, shadowRect);
            }

            dc.FillRectangle(Brushes.WhiteSmoke, innerRect);
            dc.DrawRectangle(Pens.Gray, innerRect);

            var imageSize = new Size(image.PixelSize.Width, image.PixelSize.Height);

            var imageOrigin = new Point(
                innerRect.X + (innerRect.Width - imageSize.Width) / 2,
                innerRect.Y + (innerRect.Height - imageSize.Height) / 2);

            dc.DrawImage(image, new Rect(imageOrigin, imageSize));

            if (!Enabled)
                dc.FillRectangle(new SolidBrush(Color.FromArgb(unchecked((int)0xaaffffff))), innerRect);
        }
    }
}