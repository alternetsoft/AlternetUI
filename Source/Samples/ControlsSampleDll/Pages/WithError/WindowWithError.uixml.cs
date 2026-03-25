using System;
using System.IO;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    public partial class WindowWithError : Window
    {
        public WindowWithError()
        {
            InitializeComponent();

            SetSizeToContent();
        }

        private void HelloButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Hello, world!");
        }
    }
}