using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class ProgressBarPage : Control
    {
        private readonly IPageSite site;

        public ProgressBarPage(IPageSite site)
        {
            InitializeComponent();

            this.site = site;
        }

        private void IncreaseAllButton_Click(object? sender, EventArgs e)
        {
            foreach (var progressBar in progressBarsPanel.Children.OfType<ProgressBar>())
            {
                if (progressBar.Value < progressBar.Maximum)
                    progressBar.Value++;
            }
        }
    }
}