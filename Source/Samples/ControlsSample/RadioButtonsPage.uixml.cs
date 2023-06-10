using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class RadioButtonsPage : Control
    {
        private IPageSite? site;

        public RadioButtonsPage()
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

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            site?.LogEvent("Radio button " + rb.Text + " IsChecked changed to " + rb.IsChecked);
        }
    }
}