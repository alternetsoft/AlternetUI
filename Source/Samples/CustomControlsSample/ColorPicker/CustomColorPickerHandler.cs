using Alternet.Drawing;
using Alternet.UI;
using System;

namespace CustomControlsSample
{
    public class CustomColorPickerHandler : ControlHandler<ColorPicker>
    {
        private Brush backgroundBrush = Brushes.LightCyan;

        private Brush backgroundHoveredBrush = Brushes.LightYellow;

        private Brush backgroundPressedBrush = Brushes.PeachPuff;

        private Pen borderPen = Pens.LightGray;

        private bool isPressed;

        private Popup? popup;

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

                if (isPressed)
                    OpenPopup();
            }
        }

        private Popup Popup
        {
            get
            {
                if (popup == null)
                {
                    popup = new Popup();
                    popup.Children.Add(new Button("HELLO"));
                    popup.SetSizeToContent();
                }

                return popup;
            }
        }

        public override void OnPaint(DrawingContext dc)
        {
            var bounds = ClientRectangle;
            dc.FillRectangle(GetBackgroundBrush(), bounds);
            dc.DrawRectangle(borderPen, bounds);
            dc.FillRectangle(new SolidBrush(Control.Value), bounds.InflatedBy(-5, -5));
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            return new Size(30, 30);
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
            CaptureMouse();
            IsPressed = true;
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            IsPressed = false;
        }

        private void Control_ValueChanged(object? sender, EventArgs e)
        {
            Refresh();
        }

        private Brush GetBackgroundBrush()
        {
            if (IsPressed)
                return backgroundPressedBrush;
            if (IsMouseOver)
                return backgroundHoveredBrush;

            return backgroundBrush;
        }

        private void OpenPopup()
        {
            Control.BeginInvoke(() =>
            {
                Popup.Location = Control.ClientToScreen(ClientRectangle.BottomLeft);
                Popup.Show();
            });
        }
    }
}