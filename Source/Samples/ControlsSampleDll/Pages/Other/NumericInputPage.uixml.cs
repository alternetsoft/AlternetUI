﻿using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class NumericInputPage : Panel
    {
        static NumericInputPage()
        {
        }

        public NumericInputPage()
        {
            InitializeComponent();
            progressBarControlNumericUpDown.Value = 1;

            calcPanel.Click += (s, e) =>
            {
                if (!Keyboard.IsAltShiftPressed)
                    return;

                calculator.ParentBackColor = false;
                calcPanel.BackColor = SystemColors.Window;
            };

            intPicker.ValueChanged += (s,e)=>
            {
                progressBarControlNumericUpDown.Value = intPicker.Value;
            };

            intPicker2.SetPlusMinusImages(KnownButton.TextBoxUp,KnownButton.TextBoxDown);
            intPicker3.SetPlusMinusImages(KnownSvgImages.ImgAngleUp, KnownSvgImages.ImgAngleDown);
        }

        private void NumericUpDown_ValueChanged(object? sender, EventArgs e)
        {
            App.Log("New NumericUpDown value is: " + ((NumericUpDown)sender!).Value);
        }

        private void ProgressBarControlNumericUpDown_ValueChanged(
            object? sender, 
            EventArgs e)
        {
            intPicker.Value = (int)progressBarControlNumericUpDown.Value;
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