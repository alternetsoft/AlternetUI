using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;

namespace HelloWorldSample
{
    class MainWindow : Window
    {
        Button sayHelloButton;

        public MainWindow()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("HelloWorldSample.MainWindow.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            sayHelloButton = (Button)FindControl("sayHelloButton");
            sayHelloButton.Click += SayHelloButton_Click;
        }

        private void SayHelloButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Hello", "Hello");
        }
    }
}