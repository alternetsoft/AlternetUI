using Alternet.Drawing;
using Alternet.UI;
using System;

namespace PaintSample
{
    internal class ToolButton : Control
    {
        private bool isPressed;
        private bool isToggled;

        public ToolButton(Tool tool)
        {
            UserPaint = true;
            Tool = tool;

            image = LoadImage();
        }

        private Image LoadImage()
        {
            var stream = GetType().Assembly.GetManifestResourceStream(
                "PaintSample.Resources.ToolIcons." + Tool.GetType().Name.Replace("Tool", "") + ".png");
#pragma warning disable
            if (stream == null)
                throw new InvalidOperationException();
#pragma warning restore

            return new Bitmap(stream);
        }

        private readonly Image image;

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

        public bool IsToggled
        {
            get => isToggled;

            set
            {
                if (isToggled == value)
                    return;

                isToggled = value;

                ToggledChanged?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        public event EventHandler? ToggledChanged;

        public Tool Tool { get; }

        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return new SizeD(30, 30);
        }

        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            CaptureMouse();
            IsPressed = true;
        }

        protected override void OnMouseLeftButtonUp(MouseEventArgs e)
        {
            ReleaseMouseCapture();
            IsPressed = false;

            if (IsMouseOver)
            {
                RaiseClick(EventArgs.Empty);
                
                if (!IsToggled)
                    IsToggled = true;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
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

            if (IsPressed || IsToggled) 
            {
                innerRect.Offset(1, 1);
            }
            else
            {
                //var shadowRect = innerRect;
                //shadowRect.Offset(1, 1);
                //dc.FillRectangle(Brushes.Gray, shadowRect);
            }

            dc.FillRectangle(IsToggled ? Brushes.White : Brushes.WhiteSmoke, innerRect);
            dc.DrawRectangle(Pens.Gray, innerRect);

            var imageSize = new SizeD(image.PixelSize.Width, image.PixelSize.Height);

            var imageOrigin = new PointD(
                innerRect.X + (innerRect.Width - imageSize.Width) / 2,
                innerRect.Y + (innerRect.Height - imageSize.Height) / 2);

            dc.DrawImage(image, new RectD(imageOrigin, imageSize));

            if (IsToggled)
            {
                innerRect.Inflate(-1, -1);
                dc.DrawRectangle(Pens.Gold, innerRect);
            }
        }
    }
}