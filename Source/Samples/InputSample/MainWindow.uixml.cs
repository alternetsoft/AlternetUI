using System;
using Alternet.Drawing;
using Alternet.UI;

namespace InputSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void KeyboardInputButton_Click(object? sender, EventArgs e)
        {
            var window = new KeyboardInputWindow();
            window.Show();
        }
    }
}