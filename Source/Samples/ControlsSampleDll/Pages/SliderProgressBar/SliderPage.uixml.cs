using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class SliderPage : Panel
    {
        public SliderPage()
        {
            InitializeComponent();

            mainTabControl.MinSizeGrowMode = WindowSizeToContentMode.Height;

            tickStyleComboBox.EnumType = typeof(SliderTickStyle);

            progressBarControlSlider.ValueChanged += ProgressBarControlSlider_ValueChanged;
            progressBarControlSlider.Value = 6;

            tickStyleComboBox.ValueChanged += TickStyleComboBox_SelectedItemChanged;
            tickStyleComboBox.Value = SliderTickStyle.None;

            progressBarControlSlider.TickStyle = SliderTickStyle.None;

            sliderv1.TickStyle = SliderTickStyle.None;
            sliderv2.TickStyle = SliderTickStyle.TopLeft;
            sliderv3.TickStyle = SliderTickStyle.BottomRight;
            sliderv4.TickStyle = SliderTickStyle.Both;

            sliderv4.ThumbControl.Shape = StdSlider.SliderThumb.ShapeType.Rectangle;

            sliderh1.TickStyle = SliderTickStyle.None;
            sliderh2.TickStyle = SliderTickStyle.TopLeft;
            sliderh3.TickStyle = SliderTickStyle.BottomRight;
            sliderh4.TickStyle = SliderTickStyle.Both;

            sliderh4.ThumbControl.Shape = StdSlider.SliderThumb.ShapeType.Rectangle;

            sliderh1.ValueDisplay = displayH1;
            sliderh2.ValueDisplay = displayH2;
            sliderh3.ValueDisplay = displayH3;
            sliderh4.ValueDisplay = displayH4;

            sliderv1.ValueDisplay = displayV1;
            sliderv2.ValueDisplay = displayV2;
            sliderv3.ValueDisplay = displayV3;
            sliderv4.ValueDisplay = displayV4;

            sliderh3.TickFrequency = 10;
            sliderh4.TickFrequency = 10;

            sliderv2.TickFrequency = 10;
            sliderv3.TickFrequency = 10;

            progressBarControlSlider.SetSpacerColorToDefault();

            sliderh3.SetFarSpacerColor(LightDarkColors.Green);
            sliderv1.SetFarSpacerColor(LightDarkColors.Green);
            sliderv2.SetFarSpacerColor(LightDarkColors.Red);

            sliderh1.SetBorderColors(Color.Transparent);
            sliderh1.SetBottomBorderColor(DefaultColors.BorderColor);
            sliderh1.UseControlColors(false);

            sliderh4.ValueFormat = "{0:0.00} miles";

            sliderh3.FormatValueForDisplay += (s, e) =>
            {
                e.FormattedValue = $"{e.Value} km";
            };
        }

        private void HasBorderButton_Click(object? sender, EventArgs e)
        {
            DoInsideLayout(() =>
            {
                foreach (var slider in GetAllSliders())
                {
                    slider.HasBorder = !slider.HasBorder;
                }
            });
        }

        private void ToggleColorsButton_Click(object? sender, EventArgs e)
        {
            DoInsideLayout(() =>
            {
                foreach (var slider in GetAllSliders())
                {
                    slider.UseControlColors(slider.ParentBackColor);
                }
            });
        }

        private void TickStyleComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (tickStyleComboBox.Value is not SliderTickStyle tick)
                return;
            progressBarControlSlider.TickStyle = tick;
        }

        private void Slider_ValueChanged(object? sender, EventArgs e)
        {
            App.LogNameValueReplace("Slider.Value",((StdSlider)sender!).Value);
        }

        private void ProgressBarControlSlider_ValueChanged(object? sender, EventArgs e)
        {
            Invoke(() =>
            {
                progressBar.Value = progressBarControlSlider.Value;
            });
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
                slider.Value++;
            }
        }

        private void DecreaseAllButton_Click(object? sender, EventArgs e)
        {
            foreach (var slider in GetAllSliders())
            {
                slider.Value--;
            }
        }

        public static IEnumerable<T> GetAllChildrenOfType<T>(params AbstractControl[] parents)
        {
            return parents.SelectMany(x => x.Children.OfType<T>());
        }

        private IEnumerable<StdSlider> GetAllSliders()
        {
            return GetAllChildrenOfType<StdSlider>(horizontalSlidersPanel, verticalSlidersGrid);
        }
    }
}