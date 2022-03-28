using System;
using Alternet.UI;

namespace LayoutSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowGrid10x10Button_Click(object? sender, EventArgs e)
        {
            var window = new Grid10x10Window();
            window.Show();
        }

        private void ShowLayoutPropertiesButton_Click(object? sender, EventArgs e)
        {
            var window = new LayoutPropertiesWindow();
            window.Show();
        }
    }
}