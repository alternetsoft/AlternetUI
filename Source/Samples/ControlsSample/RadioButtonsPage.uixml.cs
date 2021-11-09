using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class RadioButtonsPage : Control
    {
        private readonly IPageSite site;

        public RadioButtonsPage(IPageSite site)
        {
            InitializeComponent();

            this.site = site;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            site.LogEvent("Radio button " + rb.Text + " IsChecked changed to " + rb.IsChecked);
        }
    }
}