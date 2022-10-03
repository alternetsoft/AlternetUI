using Alternet.Drawing;
using Alternet.UI;
using System;


namespace CustomControlsSample
{
    internal partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UpdateText();
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            if (allowCloseWindowCheckBox == null)
                return;

            if (!allowCloseWindowCheckBox.IsChecked)
            {
                MessageBox.Show("Closing the window is not allowed. Set the check box to allow.", "Closing Not Allowed");
                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        private void Option1RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            MessageBox.Show(option1RadioButton.IsChecked.ToString(), "Option 1");
        }

        private void BlueButton_Click(object? sender, EventArgs e)
        {
            SetBrush(Brushes.LightBlue);
        }

        private void GreenButton_Click(object? sender, EventArgs e)
        {
            SetBrush(Brushes.LightGreen);
        }

        private void RedButton_Click(object? sender, EventArgs e)
        {
            SetBrush(Brushes.Pink);
        }

        private void SetBrush(Brush b) => customDrawnControl!.Brush = /*customCompositeControl!.Brush =*/ b;

        private void TextBox_TextChanged(object? sender, TextChangedEventArgs e)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            if (customDrawnControl == null)
                return;

            customDrawnControl!.Text = /*customCompositeControl!.Text =*/ textBox!.Text;
        }
    }
}