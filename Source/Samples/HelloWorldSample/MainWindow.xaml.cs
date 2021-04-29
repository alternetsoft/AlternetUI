using Alternet.UI;
using System;
using System.ComponentModel;

namespace HelloWorldSample
{
    internal class MainWindow : Window
    {
        private CheckBox allowCloseWindowCheckBox;

        public MainWindow()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("HelloWorldSample.MainWindow.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            allowCloseWindowCheckBox = (CheckBox)FindControl("allowCloseWindowCheckBox");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!allowCloseWindowCheckBox.IsChecked)
            {
                MessageBox.Show("Closing the window is not allowed. Set the check box to allow.", "Closing Not Allowed");
                e.Cancel = true;
            }

            base.OnClosing(e);
        }
    }
}