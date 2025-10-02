using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    [IsCsLocalized(true)]
    internal partial class RadioButtonsPage : Panel
    {
        public RadioButtonsPage()
        {
            InitializeComponent();

            changeValueButton.Click += (s, e) =>
            {
                if (radio21.IsChecked)
                    radio22.IsChecked = true;
                else
                if (radio22.IsChecked)
                    radio23.IsChecked = true;
                else
                    radio21.IsChecked = true;
            };
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            App.Log("RadioButton [" + rb.Text + "].IsChecked = " + rb.IsChecked);
        }
    }
}