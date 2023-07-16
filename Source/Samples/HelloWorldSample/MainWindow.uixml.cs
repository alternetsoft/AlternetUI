using System;
using System.IO;
using Alternet.Drawing;
using Alternet.UI;

namespace HelloWorldSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:HelloWorldSample.Sample.ico");

            InitializeComponent();
        }

        private void HelloButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Hello, world!");
        }
    }
}