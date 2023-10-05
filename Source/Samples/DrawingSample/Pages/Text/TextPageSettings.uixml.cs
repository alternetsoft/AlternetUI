using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class TextPageSettings : Control
    {
        Label horzAlignLabel = new("Horz Align:");
        Label vertAlignLabel = new("Vert Align:");
        Label trimmingLabel = new("Trimming:");
        Label wrappingLabel = new("Wrapping:");
        ComboBox horizontalAlignmentComboBox = new();
        ComboBox verticalAlignmentComboBox = new();
        ComboBox wrappingComboBox = new();
        ComboBox trimmingComboBox = new();

        public TextPageSettings()
        {
            DoInsideLayout(() =>
            {
                InitializeComponent();

                ControlSet labels = new(horzAlignLabel, vertAlignLabel, trimmingLabel, wrappingLabel);
                ControlSet comboBoxes = new(
                    horizontalAlignmentComboBox,
                    verticalAlignmentComboBox,
                    trimmingComboBox,
                    wrappingComboBox);
                labels.Margin(new(0, 5, 10, 5)).VerticalAlignment(VerticalAlignment.Center);
                comboBoxes.Margin(new(0, 5, 0, 5)).IsEditable(false);
                var gridControls = ControlSet.GridFromColumns(labels, comboBoxes);

                LayoutFactory.SetupGrid(propGrid, gridControls);
            });
        }

        public void Initialize(TextPage page)
        {
            DataContext = page;

            customFontFamilyComboBox.Items.AddRange(FontFamily.Families);
            wrappingComboBox.AddEnumValues<TextWrapping>();
            trimmingComboBox.AddEnumValues<TextTrimming>();
            verticalAlignmentComboBox.AddEnumValues<TextVerticalAlignment>();
            horizontalAlignmentComboBox.AddEnumValues<TextHorizontalAlignment>();

            horizontalAlignmentComboBox.BindSelectedItem(nameof(TextPage.HorizontalAlignment));
            verticalAlignmentComboBox.BindSelectedItem(nameof(TextPage.VerticalAlignment));
            wrappingComboBox.BindSelectedItem(nameof(TextPage.Wrapping));
            trimmingComboBox.BindSelectedItem(nameof(TextPage.Trimming));
        }
    }
}