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
            if (stream == null)
                throw new InvalidOperationException();

            return new Image(stream);
        }

        Image image;

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

        public override Size GetPreferredSize(Size availableSize)
        {
            return new Size(30, 30);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            CaptureMouse();
            IsPressed = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            IsPressed = false;

            if (Handler.IsMouseOver)
            {
                RaiseClick(EventArgs.Empty);
                
                if (!IsToggled)
                    IsToggled = true;
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

            if (IsPressed || IsToggled) 
            {
                innerRect.Offset(1, 1);
            }
            else
            {
                var shadowRect = innerRect;
                shadowRect.Offset(1, 1);
                dc.FillRectangle(Brushes.Black, shadowRect);
            }

            dc.FillRectangle(IsToggled ? Brushes.White : Brushes.WhiteSmoke, innerRect);
            dc.DrawRectangle(Pens.Black, innerRect);

            var imageSize = new Size(image.PixelSize.Width, image.PixelSize.Height);

            var imageOrigin = new Point(
                innerRect.X + (innerRect.Width - imageSize.Width) / 2,
                innerRect.Y + (innerRect.Height - imageSize.Height) / 2);

            dc.DrawImage(image, new Rect(imageOrigin, imageSize));

            if (IsToggled)
            {
                innerRect.Inflate(-1, -1);
                dc.DrawRectangle(Pens.Gold, innerRect);
            }
        }
    }
}