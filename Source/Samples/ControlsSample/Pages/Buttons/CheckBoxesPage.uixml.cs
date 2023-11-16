using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class CheckBoxesPage : Control
    {
        private IPageSite? site;
        private readonly CheckBoxesPageDataContext dataContext = new ();

        public CheckBoxesPage()
        {
            dataContext.IsCheckedChanged += DataContext_IsCheckedChanged;
            DataContext = dataContext;

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

            checkBox.BindIsChecked(nameof(CheckBoxesPageDataContext.IsChecked));

            DataContext_IsCheckedChanged(null, EventArgs.Empty);
        }

        private void DataContext_IsCheckedChanged(object? sender, EventArgs e)
        {
            boundLabel.Text = $"DataContext.IsChecked: {dataContext.IsChecked}";
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
                site?.LogEvent("CheckBox.ThreeState is false, indeterminate state is not set.");
        }

        private void CheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            site?.LogEvent($"CheckBox.CheckState: {checkBox.CheckState}");
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

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        internal class CheckBoxesPageDataContext
        {
            private bool isChecked;

            public event EventHandler? IsCheckedChanged;

            public bool IsChecked
            {
                get
                {
                    return isChecked;
                }

                set
                {
                    if (isChecked == value)
                        return;
                    isChecked = value;
                    IsCheckedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}