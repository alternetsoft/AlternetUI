using System;
using System.Linq;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class TextInputPage : Control
    {
        private const string LoremIpsum = "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. Suspendisse tincidunt orci vitae arcu congue commodo. Proin fermentum rhoncus dictum.";
        
        private IPageSite? site;

        public TextInputPage()
        {
            InitializeComponent();
            multiLineTextBox.EmptyTextHint = "Sample Hint";
            textBox1.EmptyTextHint = "Sample Hint";
            multiLineTextBox.Text = LoremIpsum;
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
            multiLineTextBox.Text = LoremIpsum;
            multiLineTextBox.IsRichEdit = true;

            ITextBoxTextAttr ta = TextBox.CreateTextAttr();

            ta.SetTextColor(Color.Red);
            ta.SetBackgroundColor(Color.Yellow);

            multiLineTextBox.SetStyle(6, 15, ta);
        }
    }
}