using System;
using Alternet.UI;

namespace HelloWorld
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HelloButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Hello, world!");
        }
    }
}