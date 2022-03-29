using System;
using Alternet.UI;

namespace LayoutSample
{
    public partial class GridLayoutPropertiesWindow : Window
    {
        public GridLayoutPropertiesWindow()
        {
            InitializeComponent();

            containerAlignmentControl.Control = subjectGroupBox;
            
            subjectColumnWidthControl.Value = subjectGridColumn.Width;
            subjectRowHeightControl.Value = subjectGridRow.Height;
        }

        Thickness IncreaseThickness(Thickness value)
        {
            const int D = 10;
            return new Thickness(value.Left + D, value.Top + D, value.Right + D, value.Bottom + D);
        }

        private void SubjectColumnWidthControl_ValueChanged(object? sender, EventArgs e)
        {
            subjectGridColumn.Width = subjectColumnWidthControl.Value;
            subjectColumnWidthLabel.Text = subjectColumnWidthControl.Value.ToString();
        }

        private void SubjectRowHeightControl_ValueChanged(object? sender, EventArgs e)
        {
            subjectGridRow.Height = subjectRowHeightControl.Value;
            subjectRowHeightLabel.Text = subjectRowHeightControl.Value.ToString();
        }

        private void IncreaseButtonMarginButton_Click(object? sender, EventArgs e) =>
            subjectButton.Margin = IncreaseThickness(subjectButton.Margin);

        private void IncreaseButtonPaddingButton_Click(object? sender, EventArgs e) =>
            subjectButton.Padding = IncreaseThickness(subjectButton.Padding);

        private void IncreaseContainerMarginButton_Click(object? sender, EventArgs e) =>
            subjectGroupBox.Margin = IncreaseThickness(subjectGroupBox.Margin);

        private void IncreaseContainerPaddingButton_Click(object? sender, EventArgs e) =>
            subjectGroupBox.Padding = IncreaseThickness(subjectGroupBox.Padding);
    }
}