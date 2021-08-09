using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;

namespace ExplorerUISample
{
    internal class MainWindow : Window
    {
        public MainWindow()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("ExplorerUISample.MainWindow.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            //option1RadioButton = (RadioButton)FindControl("option1RadioButton");
            //option1RadioButton.CheckedChanged += Option1RadioButton_CheckedChanged;
        }
    }
}