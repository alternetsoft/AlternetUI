using System;
using Alternet.UI;

namespace LayoutSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:LayoutSample.Sample.ico");

            InitializeComponent();
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