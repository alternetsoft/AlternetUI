using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class RadioButtonsPage : Control
    {
        public RadioButtonsPage()
        {
            InitializeComponent();
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            Application.Log("Radio button " + rb.Text + " IsChecked changed to " + rb.IsChecked);
        }
    }
}