using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;

namespace ControlsSample
{
    internal class MainWindow : Window
    {
        private RadioButton option1RadioButton;

        public MainWindow()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("ControlsSample.MainWindow.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            option1RadioButton = (RadioButton)FindControl("option1RadioButton");

            option1RadioButton.CheckedChanged += Option1RadioButton_CheckedChanged;
        }

        private void Option1RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            MessageBox.Show(option1RadioButton.IsChecked.ToString(), "Option 1");
        }
    }
}