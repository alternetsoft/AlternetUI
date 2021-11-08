using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class TextInputPage : Control
    {
        private readonly IPageSite site;

        public TextInputPage(IPageSite site)
        {
            InitializeComponent();

            this.site = site;
        }

        private void TextBox_ValueChanged(object? sender, EventArgs e)
        {
            site.LogEvent("New TextBox value is: " + ((TextBox)sender!).Text);
        }

        private void AddLetterAButton_Click(object? sender, EventArgs e)
        {
            foreach (var textBox in textBoxesPanel.Children.OfType<TextBox>())
                textBox.Text += "A";
        }
    }
}