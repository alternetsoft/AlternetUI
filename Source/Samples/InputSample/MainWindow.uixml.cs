using System;
using Alternet.UI;

namespace InputSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int n;

        private void HelloButton_KeyDown(object sender, KeyEventArgs e)
        {
            Msg($"{++n} HelloButton_KeyDown [{e.Key}], Rep: {e.IsRepeat}");
        }

        private void Msg(string m)
        {
            lb.Items.Add(m);
            lb.SelectedIndex = lb.Items.Count - 1;
            //lb.ScrollIntoView(lb.SelectedItem);
        }

        private void StackPanel_KeyDown(object sender, KeyEventArgs e)
        {
            Msg($"{++n} StackPanel_KeyDown [{e.Key}], Rep: {e.IsRepeat}");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Msg($"{++n} Window_KeyDown [{e.Key}], Rep: {e.IsRepeat}");
        }
    }
}