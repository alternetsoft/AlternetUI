using Alternet.UI;
using System;
using System.Linq;

namespace LayoutSample
{
    public partial class FocusWindow : Window
    {
        public FocusWindow()
        {
            InitializeComponent();

            textBox1.SetFocus();
            UpdateTextBox1IsFocusedValueLabel();
        }

        private void SetFocusToTextBox1Button_Click(object sender, System.EventArgs e)
        {
            textBox1.SetFocus();
        }

        private void SetFocusToNextControlButton_Click(object sender, System.EventArgs e)
        {
            
        }

        private void UpdateTextBox1IsFocusedValueLabel()
        {
            textBox1IsFocusedValueLabel.Text = textBox1.IsFocused.ToString();
        }

        private void TextBox1_GotFocus(object sender, Alternet.UI.RoutedEventArgs e)
        {
            UpdateTextBox1IsFocusedValueLabel();
        }

        private void TextBox1_LostFocus(object sender, Alternet.UI.RoutedEventArgs e)
        {
            UpdateTextBox1IsFocusedValueLabel();
        }
    }
}