using Alternet.Drawing;
using Alternet.UI;
using System;

namespace CustomControlsSample
{
    public class CustomColorPickerHandler : ControlHandler<ColorPicker>
    {
        private bool isPressed;

        private PopupGridColors? popup;

        protected override bool NeedsPaint => true;

        private bool IsPressed
        {
            get => isPressed;
            set
            {
                if (isPressed == value)
                    return;

                isPressed = value;
                Control.Refresh();
            }
        }

        private PopupGridColors Popup
        {
            get
            {
                if (popup == null)
                {
                    popup = new()
                    {
                        /*Owner = Control.ParentWindow,*/
                        Name = "Popup",
                        HideOnEnter = false,
                        HideOnDoubleClick = false,
                    };
                    popup.Disposed += Popup_Disposed;
                    popup.VisibleChanged += Popup_VisibleChanged;
                    popup.SetSizeToContent();
                }

                return popup;
            }
        }

        private void Popup_VisibleChanged(object? sender, EventArgs e)
        {
            if (Popup.Visible || Popup.PopupResult != ModalResult.Accepted)
                return;
            Control.Value = Popup.Value;
        }

        private void Popup_Disposed(object? sender, EventArgs e)
        {
            popup = null;
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

        public override void OnPaint(Graphics dc)
        {
            var bounds = Control.ClientRectangle;
            dc.FillRectangle(GetBackgroundBrush(), bounds);
            dc.DrawRectangle(CustomControlsColors.BorderPen, bounds);
            dc.FillRectangle(ColorBrush, bounds.InflatedBy(-5, -5));
        }

        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return new SizeD(30, 30);
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            Control.UserPaint = true;
            Control.ValueChanged += Control_ValueChanged;
            Control.MouseMove += Control_MouseMove;
            Control.MouseEnter += Control_MouseEnter;
            Control.KeyDown += Control_KeyDown;
            Control.MouseLeave += Control_MouseLeave;
            Control.GotFocus += Control_GotFocus;
            Control.LostFocus += Control_LostFocus;
            Control.MouseLeftButtonDown += Control_MouseLeftButtonDown;
            Control.MouseLeftButtonUp += Control_MouseLeftButtonUp;
        }

        private void Control_KeyDown(object? sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                e.Handled = true;
                OpenPopup();
            }
        }

        private void Control_LostFocus(object? sender, EventArgs e)
        {
            Control.Refresh();
        }

        private void Control_GotFocus(object? sender, EventArgs e)
        {
            Control.Refresh();
        }

        protected override void OnDetach()
        {
            Control.ValueChanged -= Control_ValueChanged;
            Control.MouseMove -= Control_MouseMove;
            Control.MouseEnter -= Control_MouseEnter;
            Control.MouseLeave -= Control_MouseLeave;
            Control.MouseLeftButtonDown -= Control_MouseLeftButtonDown;
            Control.MouseLeftButtonUp -= Control_MouseLeftButtonUp;
            Control.GotFocus -= Control_GotFocus;
            Control.LostFocus -= Control_LostFocus;

            base.OnDetach();
        }

        private void Control_MouseLeave(object? sender, EventArgs e)
        {
            Control.Refresh();
        }

        private void Control_MouseEnter(object? sender, EventArgs e)
        {
            Control.Refresh();
        }

        private void Control_MouseMove(object? sender, MouseEventArgs e)
        {
            Control.Refresh();
        }

        private void Control_MouseLeftButtonDown(object? sender, MouseEventArgs e)
        {
            IsPressed = true;
        }

        private void Control_MouseLeftButtonUp(object? sender, MouseEventArgs e)
        {
            IsPressed = false;
            OpenPopup();
        }

        private void Control_ValueChanged(object? sender, EventArgs e)
        {
            colorBrush = null;
            Control.Refresh();
        }

        private Brush GetBackgroundBrush()
        {
            if (IsPressed)
                return CustomControlsColors.BackgroundPressedBrush;
            if (Control.IsMouseOver)
                return CustomControlsColors.BackgroundHoveredBrush;
            if(IsFocused)
                return CustomControlsColors.BackgroundFocusedBrush;

            return CustomControlsColors.BackgroundBrush;
        }

        private void OpenPopup()
        {
            if(Application.IsWindowsOS)
                Popup.ShowPopup(this.Control);
        }
    }
}