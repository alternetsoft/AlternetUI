using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class TextInputPage : Control
    {
        private IPageSite? site;

        public TextInputPage()
        {
            InitializeComponent();
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        private void TextBox_ValueChanged(object? sender, TextChangedEventArgs e)
        {
            site?.LogEvent("New TextBox value is: " + ((TextBox)sender!).Text);
        }

        private void HasBorderButton_Click(object? sender, EventArgs e)
        {
            foreach (var textBox in textBoxesPanel.Children.OfType<TextBox>())
                textBox.HasBorder = !textBox.HasBorder;
        }

        private void AddLetterAButton_Click(object? sender, EventArgs e)
        {
            foreach (var textBox in textBoxesPanel.Children.OfType<TextBox>())
                textBox.Text += "A";
        }

        private void RichEditButton_Click(object? sender, EventArgs e)
        {
            multiLineTextBox.IsRichEdit = true;

        }
    }
}