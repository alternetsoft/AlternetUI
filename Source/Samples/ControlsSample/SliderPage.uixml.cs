using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class SliderPage : Control
    {
        private IPageSite? site;

        public SliderPage()
        {
            InitializeComponent();

            foreach (var item in Enum.GetValues(typeof(SliderTickStyle)))
                tickStyleComboBox.Items.Add(item ?? throw new Exception());
            tickStyleComboBox.SelectedIndex = 0;
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

        private void TickStyleComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            foreach (var slider in GetAllSliders())
                slider.TickStyle = (SliderTickStyle)(tickStyleComboBox.SelectedItem ?? throw new InvalidOperationException());
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
            foreach (var slider in GetAllSliders())
            {
                if (slider.Value < slider.Maximum)
                    slider.Value++;
            }
        }

        private IEnumerable<Slider> GetAllSliders()
        {
            return new Control[] { horizontalSlidersPanel, verticalSlidersGrid }.SelectMany(x => x.Children.OfType<Slider>());
        }
    }
}