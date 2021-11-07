using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class NumericInputPage : Control
    {
        private readonly IPageSite site;

        public NumericInputPage(IPageSite site)
        {
            InitializeComponent();
            progressBarControlNumericUpDown.Value = 1;

            this.site = site;
        }

        private void NumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            site.LogEvent("New NumericUpDown value is: " + ((NumericUpDown)sender!).Value);
        }

        private void ProgressBarControlNumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            progressBar.Value = (int)progressBarControlNumericUpDown.Value;
        }

        private void IncreaseAllButton_Click(object? sender, EventArgs e)
        {
            foreach (var NumericUpDown in numericUpDownsPanel.Children.OfType<NumericUpDown>())
            {
                if (NumericUpDown.Value < NumericUpDown.Maximum)
                    NumericUpDown.Value++;
            }
        }
    }
}