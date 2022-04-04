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

        private void helloButton_KeyDown(object sender, KeyEventArgs e)
        {
            Msg($"{++n} helloButton_KeyDown, Rep: {e.IsRepeat}");
        }

        private void Msg(string m)
        {
            lb.Items.Add(m);
            lb.SelectedIndex = lb.Items.Count - 1;
            //lb.ScrollIntoView(lb.SelectedItem);
        }

        private void StackPanel_KeyUp(object sender, KeyEventArgs e)
        {
            Msg($"{++n} StackPanel_KeyUp, Rep: {e.IsRepeat}");
        }
    }
}