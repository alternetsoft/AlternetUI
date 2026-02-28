using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ProgressBarPage : Panel
    {
        public ProgressBarPage()
        {
            InitializeComponent();

            mainTabControl.MinSizeGrowMode = WindowSizeToContentMode.Height;

            progressH1.ValueDisplay = displayH1;
            progressH2.ValueDisplay = displayH2;
            progressH3.ValueDisplay = displayH3;
            progressH4.ValueDisplay = displayH4;
            progressH5.ValueDisplay = displayH5;

            progressV1.ValueDisplay = displayV1;
            progressV2.ValueDisplay = displayV2;
            progressV3.ValueDisplay = displayV3;
            progressV4.ValueDisplay = displayV4;
            progressV5.ValueDisplay = displayV5;
        }

        private void IncreaseAllButton_Click(object? sender, EventArgs e)
        {
            foreach (var progressBar in GetAllProgressBars())
            {
                if (progressBar.Value < progressBar.Maximum)
                    progressBar.Value++;
            }
        }

        private void DecreaseAllButton_Click(object? sender, EventArgs e)
        {
            foreach (var progressBar in GetAllProgressBars())
            {
                if (progressBar.Value > progressBar.Minimum)
                    progressBar.Value--;
            }
        }

        private IEnumerable<StdProgressBar> GetAllProgressBars()
        {
            return new AbstractControl[]
            {
                verticalProgressBarsGrid,
                horizontalProgressBarsPanel
            }.SelectMany(x => x.Children.OfType<StdProgressBar>());
        }

        private void IndeterminateCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            foreach (var progressBar in GetAllProgressBars())
                progressBar.IsIndeterminate = indeterminateCheckBox.IsChecked;
        }
    }
}