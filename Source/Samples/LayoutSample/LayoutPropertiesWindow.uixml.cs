using System;
using Alternet.UI;

namespace LayoutSample
{
    public partial class LayoutPropertiesWindow : Window
    {
        public LayoutPropertiesWindow()
        {
            InitializeComponent();
        }

        Thickness IncreaseThickness(Thickness value)
        {
            const int D = 10;
            return new Thickness(value.Left + D, value.Top + D, value.Right + D, value.Bottom + D);
        }

        private void IncreaseButtonMarginButton_Click(object? sender, EventArgs e)
        {
            subjectButton.Margin = IncreaseThickness(subjectButton.Margin);
        }

        private void IncreaseButtonPaddingButton_Click(object? sender, EventArgs e)
        {
            subjectButton.Padding = IncreaseThickness(subjectButton.Padding);
        }
    }
}