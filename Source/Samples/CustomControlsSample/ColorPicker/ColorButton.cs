using Alternet.Drawing;
using Alternet.UI;
using System;

namespace CustomControlsSample
{
    public class ColorButton : ColorPicker
    {
        public ColorButton()
        {
        }

        protected override ControlHandler CreateHandler()
        {
            return new CustomHandler();
        }

        public class CustomHandler : ControlHandler<ColorPicker>
        {
            private bool isPressed;

            protected override bool NeedsPaint => true;

            private bool IsPressed
            {
                get => isPressed;
                set
                {
                    if (isPressed == value)
                        return;

                    isPressed = value;
                    Refresh();
                }
            }

            SolidBrush? colorBrush;

            SolidBrush ColorBrush
            {
                get
                {
                    if (colorBrush == null)
                        colorBrush = new SolidBrush(Control.Value);

                    return colorBrush;
                }
            }

            public override void OnPaint(DrawingContext dc)
            {
                var bounds = ClientRectangle;
                dc.FillRectangle(GetBackgroundBrush(), bounds);
                dc.FillRectangle(ColorBrush, bounds.InflatedBy(-2, -2));
            }

            public override Size GetPreferredSize(Size availableSize)
            {
                return new Size(20, 20);
            }

            protected override void OnAttach()
            {
                base.OnAttach();

                Control.ValueChanged += Control_ValueChanged;
                Control.MouseMove += Control_MouseMove;
                Control.MouseEnter += Control_MouseEnter;
                Control.MouseLeave += Control_MouseLeave;
                Control.MouseLeftButtonDown += Control_MouseLeftButtonDown;
                Control.MouseLeftButtonUp += Control_MouseLeftButtonUp;
            }

            protected override void OnDetach()
            {
                Control.ValueChanged -= Control_ValueChanged;
                Control.MouseMove -= Control_MouseMove;
                Control.MouseEnter -= Control_MouseEnter;
                Control.MouseLeave -= Control_MouseLeave;
                Control.MouseLeftButtonDown -= Control_MouseLeftButtonDown;
                Control.MouseLeftButtonUp -= Control_MouseLeftButtonUp;

                base.OnDetach();
            }

            private void Control_MouseLeave(object? sender, EventArgs e)
            {
                Refresh();
            }

            private void Control_MouseEnter(object? sender, EventArgs e)
            {
                Refresh();
            }

            private void Control_MouseMove(object sender, MouseEventArgs e)
            {
                Refresh();
            }

            private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                IsPressed = true;
                Control.RaiseClick(EventArgs.Empty);
            }

            private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                IsPressed = false;
            }

            private void Control_ValueChanged(object? sender, EventArgs e)
            {
                colorBrush = null;
                Refresh();
            }

            private Brush GetBackgroundBrush()
            {
                if (IsPressed)
                    return CustomControlsColors.BackgroundPressedBrush;
                if (IsMouseOver)
                    return CustomControlsColors.BackgroundHoveredBrush;

                return CustomControlsColors.BackgroundBrush;
            }
        }
    }
}