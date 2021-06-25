using Alternet.UI;
using System;

namespace ControlsSample
{
    internal class ListBoxPage : Control
    {
        private ListBox listBox;

        public ListBoxPage()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("ControlsSample.ListBoxPage.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            listBox = (ListBox)FindControl("listBox");

            listBox.Items.Add("One");
            listBox.Items.Add("Two");
            listBox.Items.Add("Three");
        }
    }
}