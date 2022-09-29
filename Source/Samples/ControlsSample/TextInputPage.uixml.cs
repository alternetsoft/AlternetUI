using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class TextInputPage : Control
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

        private void AddLetterAButton_Click(object? sender, EventArgs e)
        {
            foreach (var textBox in textBoxesPanel.Children.OfType<TextBox>())
                textBox.Text += "A";
        }
    }
}