using System;
using Alternet.UI;

namespace LayoutSample
{
    public partial class StackLayoutPropertiesWindow : Window
    {
        public StackLayoutPropertiesWindow()
        {
            InitializeComponent();

            containerAlignmentControl.Control = subjectGroupBox;
        }

        Thickness IncreaseThickness(Thickness value)
        {
            const int D = 10;
            return new Thickness(value.Left + D, value.Top + D, value.Right + D, value.Bottom + D);
        }

        private void IncreaseButtonMarginButton_Click(object? sender, EventArgs e) =>
            subjectButton.Margin = IncreaseThickness(subjectButton.Margin);

        private void IncreaseButtonPaddingButton_Click(object? sender, EventArgs e) =>
            subjectButton.Padding = IncreaseThickness(subjectButton.Padding);

        private void IncreaseContainerMarginButton_Click(object? sender, EventArgs e) =>
            subjectGroupBox.Margin = IncreaseThickness(subjectGroupBox.Margin);

        private void IncreaseContainerPaddingButton_Click(object? sender, EventArgs e) =>
            subjectGroupBox.Padding = IncreaseThickness(subjectGroupBox.Padding);

        private void HorizontalContainerLayoutCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (subjectPanel == null)
                return;

            subjectPanel.Orientation = ((CheckBox)sender!).IsChecked ? StackPanelOrientation.Horizontal : StackPanelOrientation.Vertical;
        }
    }
}