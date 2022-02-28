using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class ProgressBarPage : Control
    {
        private IPageSite site;

        public ProgressBarPage()
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