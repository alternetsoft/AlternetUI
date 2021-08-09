using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;

namespace ExplorerUISample
{
    internal class ProgressWindow : Window
    {
        public ProgressWindow()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("ExplorerUISample.ProgressWindow.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);
        }
    }
}