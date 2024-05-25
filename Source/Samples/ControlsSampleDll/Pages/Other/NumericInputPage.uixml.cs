using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class NumericInputPage : Control
    {
        public NumericInputPage()
        {
            InitializeComponent();
            progressBarControlNumericUpDown.Value = 1;
        }

        private void NumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            Application.Log("New NumericUpDown value is: " + ((NumericUpDown)sender!).Value);
        }

        private void ProgressBarControlNumericUpDown_ValueChanged(
            object? sender, 
            EventArgs e)
        {
            progressBar.Value = (int)progressBarControlNumericUpDown.Value;
        }

        private void HasBorderButton_Click(object? sender, EventArgs e)
        {
        }

        private void IncreaseAllButton_Click(object? sender, EventArgs e)
        {
            numericUpDownsPanel.ForEachChild<NumericUpDown>(
                (x) => { x.IncrementValue(); });
        }
    }
}