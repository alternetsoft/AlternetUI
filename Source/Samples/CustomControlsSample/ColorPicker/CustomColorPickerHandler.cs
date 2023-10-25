using Alternet.Drawing;
using Alternet.UI;
using System;

namespace CustomControlsSample
{
    public class CustomColorPickerHandler : ControlHandler<ColorPicker>
    {
        private bool isPressed;

        private Window? popup;

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

        private Window Popup
        {
            get
            {
                if (popup == null)
                {
                    popup = new Window();
                    popup.Owner = Control.ParentWindow;
                    popup.ShowInTaskbar = false;
                    popup.StartLocation = WindowStartLocation.Manual;
                    popup.HasTitleBar = false;
                    popup.AlwaysOnTop = true;
                    popup.CloseEnabled = false;
                    popup.MinimizeEnabled = false;
                    popup.MaximizeEnabled = false;
                    popup.Resizable = false;
                    popup.Deactivated += Popup_Deactivated;

                    var border = new Border();
                    border.Children.Add(GetColorButtonsGrid());
                    popup.Children.Add(border);
                    popup.SetSizeToContent();
                }

                return popup;
            }
        }

        private void Popup_Deactivated(object? sender, EventArgs e)
        {
            (sender as Window)?.Hide();
        }

        private readonly Color[] colors = new[]
        {
            Color.IndianRed,
            Color.LightSalmon,
            Color.Firebrick,
            Color.DarkRed,

            Color.ForestGreen,
            Color.YellowGreen,
            Color.PaleGreen,
            Color.Olive,

            Color.PowderBlue,
            Color.DodgerBlue,
            Color.DarkBlue,
            Color.SteelBlue,

            Color.Silver,
            Color.LightSlateGray,
            Color.DarkSlateGray,
            Color.Black
        };

        Grid GetColorButtonsGrid()
        {
            int RowCount = 4;
            int ColumnCount = 4;

            var grid = new Grid();

            for (int y = 0; y < ColumnCount; y++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int x = 0; x < RowCount; x++)
                grid.RowDefinitions.Add(new RowDefinition());

            int i = 0;
            for (int x = 0; x < RowCount; x++)
            {
                for (int y = 0; y < ColumnCount; y++)
                {
                    var button = new ColorButton { Value = colors[i++] };
                    button.Click += ColorButton_Click;
                    grid.Children.Add(button);
                    Grid.SetRow(button, y);
                    Grid.SetColumn(button, x);
                }
            }

            return grid;
        }

        private void ColorButton_Click(object? sender, EventArgs e)
        {
            Control.Value = ((ColorPicker)sender!).Value;
            Popup.Hide();
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
            dc.DrawRectangle(CustomControlsColors.BorderPen, bounds);
            dc.FillRectangle(ColorBrush, bounds.InflatedBy(-5, -5));
        }

        public override Size GetPreferredSize(Size availableSize)
        {
            return new Size(30, 30);
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            UserPaint = true;
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

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                e.Handled = true;
                OpenPopup();
            }
        }

        private void Control_LostFocus(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            Refresh();
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
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPressed = false;
            OpenPopup();
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
            if(IsFocused)
                return CustomControlsColors.BackgroundFocusedBrush;

            return CustomControlsColors.BackgroundBrush;
        }

        private void OpenPopup()
        {
            Control.BeginInvoke(() =>
            {
                Popup.HandleNeeded();
                var bl = ClientRectangle.BottomLeft;
                var blScreen = Control.ClientToScreen(bl);
                Popup.Location = blScreen;
                Popup.SetSizeToContent();
                Popup.Show();
                if (Popup.IsFocusable)
                    Popup.SetFocus();
            });
        }
    }
}