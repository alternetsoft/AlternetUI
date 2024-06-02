using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class CheckBoxesPage : Control
    {
        public CheckBoxesPage()
        {
            InitializeComponent();

            threeStatesCheckBox.CheckedChanged += ThreeStatesCheckBox_CheckedChanged;
            alignRightCheckBox.CheckedChanged += AlignRightCheckBox_CheckedChanged;
            allowAllStatesForUserCheckBox.CheckedChanged += AllowAllStatesForUserCheckBox_CheckedChanged;
            checkBox.CheckedChanged += CheckBox_CheckedChanged;

            ControlSet buttons = new(
              isCheckedFalseButton,
              isCheckedTrueButton,
              checkStateUncheckedButton,
              textEmptyButton,
              checkStateCheckedButton,
              checkStateIndeterminateButton);
            buttons.SuggestedWidthToMax();

            isCheckedFalseButton.Click += IsCheckedFalseButton_Click;
            isCheckedTrueButton.Click += IsCheckedTrueButton_Click;
            checkStateUncheckedButton.Click += CheckStateUncheckedButton_Click;
            checkStateCheckedButton.Click += CheckStateCheckedButton_Click;
            checkStateIndeterminateButton.Click += CheckStateIndeterminateButton_Click;
            textEmptyButton.Click += TextEmptyButton_Click;
        }

        private void TextEmptyButton_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(checkBox.Text))
                checkBox.Text = "Some Text";
            else
                checkBox.Text = string.Empty;
        }

        private void IsCheckedFalseButton_Click(object? sender, EventArgs e)
        {
            checkBox.IsChecked = false;
        }

        private void IsCheckedTrueButton_Click(object? sender, EventArgs e)
        {
            checkBox.IsChecked = true;
        }

        private void CheckStateUncheckedButton_Click(object? sender, EventArgs e)
        {
            checkBox.CheckState = CheckState.Unchecked;
        }

        private void CheckStateCheckedButton_Click(object? sender, EventArgs e)
        {
            checkBox.CheckState = CheckState.Checked;
        }

        private void CheckStateIndeterminateButton_Click(object? sender, EventArgs e)
        {
            if (checkBox.ThreeState)
                checkBox.CheckState = CheckState.Indeterminate;
            else
                App.Log("CheckBox.ThreeState is false, indeterminate state is not set.");
        }

        private void CheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            App.Log($"CheckBox.CheckState: {checkBox.CheckState}");
        }

        private void AllowAllStatesForUserCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            checkBox.AllowAllStatesForUser = allowAllStatesForUserCheckBox.IsChecked;
        }

        private void AlignRightCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            checkBox.AlignRight = alignRightCheckBox.IsChecked;
        }

        private void ThreeStatesCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            checkBox.ThreeState = threeStatesCheckBox.IsChecked;
        }
    }
}