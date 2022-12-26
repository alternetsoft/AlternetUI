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
        }

        private void SetFocusToTextBox1Button_Click(object sender, System.EventArgs e)
        {
            textBox1.SetFocus();
        }

        private void SetFocusToNextControlButton_Click(object sender, System.EventArgs e)
        {
            
        }
    }
}