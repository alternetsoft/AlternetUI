using Alternet.Drawing;
using Alternet.UI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomControlsSample
{
    internal partial class CustomControlsWindow : Window
    {
        private readonly SpeedColorButton colorPicker = new()
        {
            Value = Color.Red,
            HorizontalAlignment = HorizontalAlignment.Left,
            MinimumSize = 32,
            VerticalAlignment = VerticalAlignment.Center,
            UseTheme = SpeedButton.KnownTheme.StaticBorder,
        };

        private readonly ColorComboBox colorCombo = new()
        {
            Value = Color.Red,
            VerticalAlignment = VerticalAlignment.Center,
        };

        private readonly Label colorComboLabel = new("ComboBox:")
        {
            VerticalAlignment = VerticalAlignment.Center,
        };

        private readonly TicTacToeControl ticTacToe = new()
        {
            Margin = 10,
        };
        
        private readonly FancySlider knobControl = new()
        {
            Minimum = 0,
            Maximum = 100,
            Margin = new Thickness(0, 0, 5, 0),
        };
        
        private readonly FancyProgressBar gaugeControl = new()
        {
            Minimum = 0,
            Maximum = 100,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5, 0, 0, 0),
        };

        private readonly ColorPicker nativeColorPicker = new()
        {
            Value = Color.Red,
            VerticalAlignment = VerticalAlignment.Center,
        };

        public CustomControlsWindow()
        {
            Icon = App.DefaultIcon;

            InitializeComponent();

            DoInsideLayout(Fn);

            void Fn()
            {
                colorGroupBox.AddLabel("Native:");

                nativeColorPicker.SuggestedHeight = 32;
                nativeColorPicker.Parent = colorGroupBox;

                colorGroupBox.AddLabel("SpeedButton:");
                colorPicker.Parent = colorGroupBox;

                colorComboLabel.Parent = colorGroupBox;
                colorCombo.Parent = colorGroupBox;
                ticTacToe.Parent = ticTacToeStackPanel;
                knobControl.Parent = slidersStackPanel;
                gaugeControl.Parent = slidersStackPanel;
            }
            this.SetSizeToContent();
            Slider_ValueChanged(null, EventArgs.Empty);
            knobControl.ValueChanged += Slider_ValueChanged;
        }

        private void Slider_ValueChanged(object? sender, EventArgs e)
        {
            if (sender is not Slider slider)
                return;

            var v = slider.Value;

            gaugeControl.Value = v;
            knobControl.Value = v;
            progressBar1.Value = v;
            slider1.Value = v;
        }
    }
}