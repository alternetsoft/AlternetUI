using System;
using Alternet.UI;

namespace LayoutBenchmark
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowWindowButton_Click(object? sender, EventArgs e)
        {
            var window = new TestWindow();
            window.Show();
        }
    }
}