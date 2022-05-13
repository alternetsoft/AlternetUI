using System;
using Alternet.UI;

namespace HelloWorldSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var child = new Window();
            child.Children.Add(new Button { Text = "Hello!" });
            child.Owner = this;
            child.Show();
        }

        private void HelloButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Hello, world!");
        }
    }
}