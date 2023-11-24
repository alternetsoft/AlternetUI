using System;
using Alternet.Drawing;
using Alternet.UI;

namespace InputSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:InputSample.Sample.ico");
            InitializeComponent();
        }

        private void KeyboardInputButton_Click(object? sender, EventArgs e)
        {
            var window = new KeyboardInputWindow();
            window.Show();
        }

        private void MouseInputButton_Click(object? sender, EventArgs e)
        {
            var window = new MouseInputWindow();
            window.Show();
        }
    }
}