using System;
using System.Linq;

using Alternet.UI;

namespace ControlsSample
{
    internal partial class NumericInputPage : Panel
    {
        private readonly PopupCalculator popupCalculator = new();

        static NumericInputPage()
        {
        }

        public NumericInputPage()
        {
            InitializeComponent();

            calcPanel.Click += (s, e) =>
            {
                if (!Keyboard.IsAltShiftPressed)
                    return;

                calculator.ParentBackColor = false;
                calcPanel.BackColor = SystemColors.Window;
            };

            intPicker2.SetPlusMinusImages(KnownButton.TextBoxUp, KnownButton.TextBoxDown);
            intPicker3.SetPlusMinusImages(KnownSvgImages.ImgAngleUp, KnownSvgImages.ImgAngleDown);

            intPicker1.ValueChanged += IntPicker_ValueChanged;
            intPicker2.ValueChanged += IntPicker_ValueChanged;
            intPicker3.ValueChanged += IntPicker_ValueChanged;
            intPicker4.ValueChanged += IntPicker_ValueChanged;

            popupCalculator.AfterHide += PopupListBox_AfterHide;

            showPopupButton.HorizontalAlignment = HorizontalAlignment.Left;
            showPopupButton.Click += (s, e) =>
            {
                popupCalculator.ShowPopup(showPopupButton);
            };

            calcSettings.AddInput("Show display", calculator, nameof(calculator.IsDisplayVisible));
            calcSettings.AddInput("Show operator buttons", calculator, nameof(calculator.ShowOperatorButtons));
            calcSettings.AddInput("Show parenthesis buttons", calculator, nameof(calculator.ShowParenthesisButtons));
            calcSettings.AddInput("Show clear button", calculator, nameof(calculator.ShowClearButton));
            calcSettings.AddInput("Show toggle sign button", calculator, nameof(calculator.ShowToggleSignButton));
            calcSettings.AddInput("Show decimal point button", calculator, nameof(calculator.ShowDecimalPointButton));
            calcSettings.AddInput("Show clear last button", calculator, nameof(calculator.ShowClearLastButton));
        }

        private void PopupListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupCalculator.MainControl.AsDouble;
            App.Log($"AfterHide PopupResult: {popupCalculator.PopupResult}, Value: {resultItem}");
        }

        private void IntPicker_ValueChanged(object? sender, EventArgs e)
        {
            App.LogNameValueReplace("IntPicker.ValueChanged", (sender as IntPicker)?.Value);
        }

        private void NumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            App.Log("New NumericUpDown value is: " + ((NumericUpDown)sender!).Value);
        }

        private void HasBorderButton_Click(object? sender, EventArgs e)
        {
        }

        private void IncAll(int value)
        {
            numericUpDownsPanel.ForEachChild<NumericUpDown>(
                (x) => { x.IncrementValue(value); });
            numericUpDownsPanel.ForEachChild<IntPicker>(
                (x) => { x.IncrementValue(value); });
        }

        private void DecreaseAllButton_Click(object? sender, EventArgs e)
        {
            IncAll(-1);
        }

        private void IncreaseAllButton_Click(object? sender, EventArgs e)
        {
            IncAll(1);
        }
    }
}