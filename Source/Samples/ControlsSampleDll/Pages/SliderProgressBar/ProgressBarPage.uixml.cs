﻿using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ProgressBarPage : Control
    {
        public ProgressBarPage()
        {
            InitializeComponent();
			VerticalProgressBarsGroupBox.Visible = !App.IsMacOS;
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
            return new AbstractControl[]
            {
                verticalProgressBarsGrid,
                horizontalProgressBarsPanel
            }.SelectMany(x => x.Children.OfType<ProgressBar>());
        }

        private void IndeterminateCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            foreach (var progressBar in GetAllProgressBars())
                progressBar.IsIndeterminate = indeterminateCheckBox.IsChecked;
        }
    }
}