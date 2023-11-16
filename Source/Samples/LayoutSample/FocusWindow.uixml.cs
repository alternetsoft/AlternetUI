using System;
using Alternet.UI;

namespace LayoutSample
{
    public partial class FocusWindow : Window
    {
        public FocusWindow()
        {
            InitializeComponent();

            textBox1.SetFocus();
            UpdateTextBox1IsFocusedValueLabel();
            SetSizeToContent();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.N)
            {
                var focusedControl = GetFocusedControl();
                if (focusedControl == null)
                    return;

                e.Handled = true;
                focusedControl.FocusNextControl();
            }
        }

        private void SetFocusToTextBox1Button_Click(object sender, System.EventArgs e)
        {
            textBox1.SetFocus();
        }

        private void UpdateTextBox1IsFocusedValueLabel()
        {
            textBox1IsFocusedValueLabel.Text = textBox1.IsFocused.ToString();
        }

        private void TextBox1_GotFocus(object sender, EventArgs e)
        {
            UpdateTextBox1IsFocusedValueLabel();
        }

        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            UpdateTextBox1IsFocusedValueLabel();
        }
    }
}