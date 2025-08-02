using System;
using Alternet.Drawing;
using Alternet.UI;

namespace LayoutSample
{
    public partial class StackLayoutPropertiesWindow : Window
    {
        private readonly AlignmentControl containerAlignmentControl;
        private readonly VerticalStackPanel dockedSettings = new();
        private readonly StdListBox dockedControl = new()
        {
            Dock = DockStyle.Left,
            MinWidth = 150,
        };
        private readonly Splitter splitter = new()
        {
            Dock = DockStyle.Left,
        };
        private readonly Label dockedLabel = new("Dock side")
        {
            Margin = (10, 10, 10, 5),
        };
        private readonly EnumPicker dockedEdit = new()
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
            splitter.Parent = subjectPanel;
            dockedControl.Parent = subjectPanel;

            dockedEdit.Items.Add(new("Left", DockStyle.Left));
            dockedEdit.Items.Add(new("Top", DockStyle.Top));
            dockedEdit.Items.Add(new("Right", DockStyle.Right));
            dockedEdit.Items.Add(new("Bottom", DockStyle.Bottom));
            dockedEdit.Value = DockStyle.Left;
            dockedEdit.ValueChanged += DockedEdit_SelectedItemChanged;

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
            if (dockedEdit.Value is not DockStyle dockStyle)
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