using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class SliderPage : Control
    {
        private IPageSite? site;

        public SliderPage()
        {
            InitializeComponent();
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                progressBarControlSlider.Value = 1;
                site = value;
            }
        }

        private void Slider_ValueChanged(object? sender, EventArgs e)
        {
            site?.LogEvent("New slider value is: " + ((Slider)sender!).Value);
        }

        private void ProgressBarControlSlider_ValueChanged(object? sender, EventArgs e)
        {
            progressBar.Value = progressBarControlSlider.Value;
        }

        private void IncreaseAllButton_Click(object? sender, EventArgs e)
        {
            foreach (var slider in slidersPanel.Children.OfType<Slider>())
            {
                if (slider.Value < slider.Maximum)
                    slider.Value++;
            }
        }
    }
}