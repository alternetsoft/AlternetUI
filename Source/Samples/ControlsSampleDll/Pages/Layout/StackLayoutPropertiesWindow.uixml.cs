using System;
using Alternet.Drawing;
using Alternet.UI;

namespace LayoutSample
{
    public partial class StackLayoutPropertiesWindow : Window
    {
        private readonly AlignmentControl containerAlignmentControl;
        private readonly VerticalStackPanel dockedSettings = new();
        private readonly ListBox dockedControl = new()
        {
            Dock = DockStyle.Left,
        };
        private readonly Splitter splitter = new()
        {
            Dock = DockStyle.Left,
        };
        private readonly Label dockedLabel = new("Dock side")
        {
            Margin = (10, 10, 10, 5),
        };
        private readonly ComboBox dockedEdit = new()
        {
            Margin = (10, 0, 10, 10),
        };

        public StackLayoutPropertiesWindow()
        {
            InitializeComponent();

            dockedControl.Add("Docked");
            subjectGroupBox.MinimumSize = 370;
            dockedSettings.Title = "Docked";
            dockedSettings.Parent = tabControlPanel;
            dockedLabel.Parent = dockedSettings;
            dockedEdit.Parent = dockedSettings;
            dockedEdit.IsEditable = false;
            splitter.Parent = subjectPanel;
            dockedControl.Parent = subjectPanel;

            dockedEdit.Add(DockStyle.Left);
            dockedEdit.Add(DockStyle.Top);
            dockedEdit.Add(DockStyle.Right);
            dockedEdit.Add(DockStyle.Bottom);
            dockedEdit.SelectedItem = DockStyle.Left;
            dockedEdit.SelectedItemChanged += DockedEdit_SelectedItemChanged;

            tabControlPanel.Add(dockedSettings);

            containerAlignmentControl = new AlignmentControl();
            containerStackPanel.Children.Add(containerAlignmentControl);
            containerAlignmentControl.Control = subjectGroupBox;

            var buttonAlignmentControl = new AlignmentControl();
            buttonAlignmentControl.Parent = buttonPanel;
            buttonAlignmentControl.Control = subjectButton;
        }

        private void DockedEdit_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (dockedEdit.SelectedItem is not DockStyle dockStyle)
                return;

            subjectPanel.DoInsideLayout(() =>
            {
                dockedControl.Size = 100;
                splitter.Dock = dockStyle;
                dockedControl.Dock = dockStyle;
            });
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