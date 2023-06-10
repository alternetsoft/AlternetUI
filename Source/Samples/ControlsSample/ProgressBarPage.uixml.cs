using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ProgressBarPage : Control
    {
        private IPageSite? site;

        public ProgressBarPage()
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

        private void IncreaseAllButton_Click(object? sender, EventArgs e)
        {
            foreach (var progressBar in GetAllProgressBars())
            {
                if (progressBar.Value < progressBar.Maximum)
                    progressBar.Value++;
            }
        }

        private IEnumerable<ProgressBar> GetAllProgressBars()
        {
            return new Control[] { verticalProgressBarsGrid, horizontalProgressBarsPanel }.SelectMany(x => x.Children.OfType<ProgressBar>());
        }

        private void IndeterminateCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            foreach (var progressBar in GetAllProgressBars())
                progressBar.IsIndeterminate = indeterminateCheckBox.IsChecked;
        }
    }
}