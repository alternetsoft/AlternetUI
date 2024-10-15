using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class SliderPage : Control
    {
        public SliderPage()
        {
            InitializeComponent();

            clearTicksButton.Visible = App.IsWindowsOS || App.IsLinuxOS;

            foreach (SliderTickStyle item in Enum.GetValues(typeof(SliderTickStyle)))
            {
                if (item == SliderTickStyle.Both && !App.IsWindowsOS)
                    continue;
                tickStyleComboBox.Items.Add(item);
            }

            tickStyleComboBox.SelectedItemChanged += TickStyleComboBox_SelectedItemChanged;
            tickStyleComboBox.SelectedIndex = 0;
            progressBarControlSlider.Value = 1;
        }

        private void TickStyleComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (tickStyleComboBox.SelectedItem is null)
                return;

            this.DoInsideUpdate(() =>
            {
                foreach (var slider in GetAllSliders())
                {
                    slider.TickStyle = (SliderTickStyle)(tickStyleComboBox.SelectedItem);
                }
            });
        }

        private void Slider_ValueChanged(object? sender, EventArgs e)
        {
            App.Log("New slider value is: " + ((Slider)sender!).Value);
        }

        private void ProgressBarControlSlider_ValueChanged(object? sender, EventArgs e)
        {
            progressBar.Value = progressBarControlSlider.Value;
        }

        private void ClearTicksButton_Click(object? sender, EventArgs e)
        {
            foreach (var slider in GetAllSliders())
            {
                slider.ClearTicks();
            }
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
            return new AbstractControl[] { horizontalSlidersPanel, verticalSlidersGrid }
                .SelectMany(x => x.Children.OfType<Slider>());
        }
    }
}