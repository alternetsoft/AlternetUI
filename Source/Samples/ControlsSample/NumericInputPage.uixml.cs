using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class NumericInputPage : Control
    {
        private IPageSite? site;

        public NumericInputPage()
        {
            InitializeComponent();

            NumericUpDown edit1 = numberUpDown1;
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                progressBarControlNumericUpDown.Value = 1;
                site = value;
            }
        }

        private void NumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            site?.LogEvent("New NumericUpDown value is: " + ((NumericUpDown)sender!).Value);
        }

        private void ProgressBarControlNumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            progressBar.Value = (int)progressBarControlNumericUpDown.Value;
        }

        private void IncreaseAllButton_Click(object? sender, EventArgs e)
        {
            foreach (var numericUpDown in numericUpDownsPanel.Children.OfType<NumericUpDown>())
            {
                if (numericUpDown.Value < numericUpDown.Maximum)
                    numericUpDown.Value++;
            }
        }
    }
}