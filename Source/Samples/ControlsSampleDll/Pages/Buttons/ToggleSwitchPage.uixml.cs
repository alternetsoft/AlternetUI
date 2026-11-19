using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    [IsCsLocalized(true)]
    public partial class ToggleSwitchPage : Panel
    {
        public ToggleSwitchPage()
        {
            InitializeComponent();

            checkBox.PreferredSizeMode = PreferredSizeMode.Content;

            threeStatesCheckBox.CheckedChanged += ThreeStatesCheckBox_CheckedChanged;
            allowAllStatesForUserCheckBox.CheckedChanged += AllowAllStatesForUserCheckBox_CheckedChanged;
            checkBox.CheckedChanged += CheckBox_CheckedChanged;

            ControlSet buttons = new(
              isCheckedFalseButton,
              isCheckedTrueButton,
              checkStateUncheckedButton,
              textEmptyButton,
              checkStateCheckedButton,
              checkStateIndeterminateButton);
            buttons.MinWidth(200);

            isCheckedFalseButton.Click += IsCheckedFalseButton_Click;
            isCheckedTrueButton.Click += IsCheckedTrueButton_Click;
            checkStateUncheckedButton.Click += CheckStateUncheckedButton_Click;
            checkStateCheckedButton.Click += CheckStateCheckedButton_Click;
            checkStateIndeterminateButton.Click += CheckStateIndeterminateButton_Click;
            textEmptyButton.Click += TextEmptyButton_Click;

            checkBox.Parent?.ContextMenuStrip.Add("Toggle accent border", () =>
            {
                if(checkBox.Item.Border == BorderSettings.AccentBorder)
                {
                    checkBox.Item.Border = BorderSettings.TransparentBorder;
                }
                else
                {
                    checkBox.Item.Border = BorderSettings.AccentBorder;
                }

                checkBox.Invalidate();
            });
        }

        private void TextEmptyButton_Click(object? sender, EventArgs e)
        {
            if (!checkBox.TextVisible)
            {
                checkBox.TextVisible = true;
            }
            else
            {
                checkBox.TextVisible = false;
            }
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
            {
                checkBox.CheckState = CheckState.Indeterminate;
            }
            else
                App.Log($"CheckBox.ThreeState = {GenericStrings.BoolFalse}. {GenericStrings.IndeterminateStateIsNotSet}.");
        }

        private void CheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            App.Log($"CheckBox.CheckState: {checkBox.CheckState}");
        }

        private void AllowAllStatesForUserCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            checkBox.AllowAllStatesForUser = allowAllStatesForUserCheckBox.IsChecked;
            App.Log($"CheckBox.AllowAllStatesForUser = {checkBox.AllowAllStatesForUser}");
        }

        private void ThreeStatesCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            checkBox.ThreeState = threeStatesCheckBox.IsChecked;
            App.Log($"CheckBox.ThreeState = {checkBox.ThreeState}");
        }
    }
}