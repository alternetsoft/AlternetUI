using Alternet.Drawing;
using Alternet.UI;
using System;

namespace PaintSample
{
    internal class ColorSwatch : Control
    {
        private bool isPressed;

        public ColorSwatch(Color swatchColor)
        {
            UserPaint = true;
            SwatchColor = swatchColor;
        }

        public Color SwatchColor { get; }

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

        public override Size GetPreferredSize(Size availableSize)
        {
            return new Size(20, 20);
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
                RaiseClick(EventArgs.Empty);
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

            dc.FillRectangle(new SolidBrush(SwatchColor), innerRect);
            dc.DrawRectangle(Pens.Gray, innerRect);
        }
    }
}