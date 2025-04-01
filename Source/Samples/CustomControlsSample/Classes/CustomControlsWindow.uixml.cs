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
        };
        
        private readonly FancySlider knobControl = new()
        {
            Minimum = 0,
            Maximum = 100,
        };
        
        private readonly FancyProgressBar gaugeControl = new()
        {
            Minimum = 0,
            Maximum = 100,
            VerticalAlignment = VerticalAlignment.Center,
        };

        public CustomControlsWindow()
        {
            Icon = App.DefaultIcon;
            colorPicker.PopupWindow.Required();

            InitializeComponent();

            DoInsideLayout(Fn);

            void Fn()
            {
                colorPicker.Parent = colorGroupBox;

                colorComboLabel.Parent = colorGroupBox;
                colorCombo.Parent = colorGroupBox;
                
                ticTacToe.Parent = ticTacToeStackPanel;
                
                knobControl.Parent = slidersStackPanel;
                gaugeControl.Parent = slidersStackPanel;
            }

            SetSizeToContent();

            knobControl.ValueChanged += Slider_ValueChanged;
            slider1.ValueChanged += Slider_ValueChanged;

            slider1.Value = 35;

            // Logs LongTap event.
            knobControl.CanLongTap = true;
            knobControl.LongTap += TicTacToe_LongTap;
        }

        private void TicTacToe_LongTap(object sender, LongTapEventArgs e)
        {
            App.Log("Fancy slider: LongTap");
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