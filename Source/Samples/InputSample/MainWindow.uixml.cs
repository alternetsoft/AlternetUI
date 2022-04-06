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

        int messageNumber;

        private void LogMessage(string m)
        {
            lb.Items.Add(m);
            lb.SelectedIndex = lb.Items.Count - 1;
        }

        private void LogKey(KeyEventArgs e, string objectName, string eventName) => LogMessage($"{++messageNumber} {objectName}_{eventName} [{e.Key}], Rep: {e.IsRepeat}");

        private void HelloButton_KeyDown(object sender, KeyEventArgs e) => LogKey(e, "HelloButton", "KeyDown");

        private void StackPanel_KeyDown(object sender, KeyEventArgs e) => LogKey(e, "StackPanel", "KeyDown");

        private void Window_KeyDown(object sender, KeyEventArgs e) => LogKey(e, "Window", "KeyDown");

        private void HelloButton_KeyUp(object sender, KeyEventArgs e) => LogKey(e, "HelloButton", "KeyUp");

        private void StackPanel_KeyUp(object sender, KeyEventArgs e) => LogKey(e, "StackPanel", "KeyUp");

        private void Window_KeyUp(object sender, KeyEventArgs e) => LogKey(e, "Window", "KeyUp");

        private void HelloButton_PreviewKeyDown(object sender, KeyEventArgs e) => LogKey(e, "HelloButton", "PreviewKeyDown");

        private void StackPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            LogKey(e, "StackPanel", "PreviewKeyDown");
            if (handlePreviewEvents.IsChecked)
                e.Handled = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e) => LogKey(e, "Window", "PreviewKeyDown");

        private void HelloButton_PreviewKeyUp(object sender, KeyEventArgs e) => LogKey(e, "HelloButton", "PreviewKeyUp");

        private void StackPanel_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            LogKey(e, "StackPanel", "PreviewKeyUp");
            if (handlePreviewEvents.IsChecked)
                e.Handled = true;
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e) => LogKey(e, "Window", "PreviewKeyUp");
    }
}