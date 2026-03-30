using System;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.UI.Extensions;

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

        private readonly UpDownAndLabel dockedLeftMarginEdit = new("Margin.Left");
        private readonly UpDownAndLabel dockedTopMarginEdit = new("Margin.Top");
        private readonly UpDownAndLabel dockedRightMarginEdit = new("Margin.Right");
        private readonly UpDownAndLabel dockedBottomMarginEdit = new("Margin.Bottom");

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

            subjectPanel.LayoutFlags |= LayoutFlags.UseMarginsWhenDock;

            dockedEdit.Items.Add(new("Left", DockStyle.Left));
            dockedEdit.Items.Add(new("Top", DockStyle.Top));
            dockedEdit.Items.Add(new("Right", DockStyle.Right));
            dockedEdit.Items.Add(new("Bottom", DockStyle.Bottom));
            dockedEdit.Items.Add(new("LeftAutoSize", DockStyle.LeftAutoSize));
            dockedEdit.Items.Add(new("TopAutoSize", DockStyle.TopAutoSize));
            dockedEdit.Items.Add(new("RightAutoSize", DockStyle.RightAutoSize));
            dockedEdit.Items.Add(new("BottomAutoSize", DockStyle.BottomAutoSize));
            dockedEdit.Value = DockStyle.Left;
            dockedEdit.ValueChanged += DockedEdit_SelectedItemChanged;

            dockedLeftMarginEdit.Value = (int)dockedControl.Margin.Left;
            dockedTopMarginEdit.Value = (int)dockedControl.Margin.Top;
            dockedRightMarginEdit.Value = (int)dockedControl.Margin.Right;
            dockedBottomMarginEdit.Value = (int)dockedControl.Margin.Bottom;

            GroupOf<UpDownAndLabel>(dockedLeftMarginEdit, dockedTopMarginEdit, dockedRightMarginEdit, dockedBottomMarginEdit)
            .Margin(10, 0, 10, 10).Parent(dockedSettings).LabelSuggestedWidthToMax()
            .Action((c)=>
            {
                c.ValueChanged += OnDockedMarginChanged;
                c.MainControl.Minimum = 0;
                c.MainControl.Maximum = 50;
                c.MainControl.SmallChange = 2;
            });

            void OnDockedMarginChanged(object? sender, EventArgs e)
            {
                var left = dockedLeftMarginEdit.Value;
                var top = dockedTopMarginEdit.Value;
                var right = dockedRightMarginEdit.Value;
                var bottom = dockedBottomMarginEdit.Value;

                dockedControl.Margin = new Thickness(left, top, right, bottom);
            }

            tabControlPanel.Add(dockedSettings);

            containerAlignmentControl = new AlignmentControl();
            containerStackPanel.Children.Add(containerAlignmentControl);
            containerAlignmentControl.Control = subjectGroupBox;

            var buttonAlignmentControl = new AlignmentControl();
            buttonAlignmentControl.Parent = buttonPanel;
            buttonAlignmentControl.Control = subjectButton;

            tabControlPanel.MinSizeGrowMode = WindowSizeToContentMode.Width;
        }

        private void DockedEdit_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (dockedEdit.Value is not DockStyle dockStyle)
                return;

            subjectPanel.DoInsideLayout(() =>
            {
                dockedControl.Size = 100;
                splitter.Dock = dockStyle.WithoutAutoSize();
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