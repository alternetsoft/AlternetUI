using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class RadioButtonsPage : Control
    {
        private IPageSite site;

        public RadioButtonsPage()
        {
            InitializeComponent();
        }

        public IPageSite Site
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
            site.LogEvent("Radio button " + rb.Text + " IsChecked changed to " + rb.IsChecked);
        }
    }
}