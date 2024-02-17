using System;
using Alternet.UI;
using LayoutSample;

namespace ControlsSample
{
    public partial class LayoutMainWindow : Control
    {
        public LayoutMainWindow()
        {
            InitializeComponent();

            MinimumSize = (600, 600);

            SetSizeToContent();
        }

        private void ShowGrid10x10Button_Click(object? sender, EventArgs e)
        {
            var window = new Grid10x10Window();
            window.Show();
        }

        private void ShowStackLayoutPropertiesButton_Click(object? sender, EventArgs e)
        {
            var window = new StackLayoutPropertiesWindow();
            window.Show();
        }

        private void ShowGridLayoutPropertiesButton_Click(object? sender, EventArgs e)
        {
            var window = new GridLayoutPropertiesWindow();
            window.Show();
        }

        private void ShowScrollingButton_Click(object? sender, EventArgs e)
        {
            var window = new ScrollingWindow();
            window.Show();
        }

        private void ShowFocusButton_Click(object? sender, EventArgs e)
        {
            var window = new FocusWindow();
            window.Show();
        }
    }
}