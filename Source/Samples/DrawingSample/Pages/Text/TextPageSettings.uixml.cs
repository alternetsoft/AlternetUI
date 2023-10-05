using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class TextPageSettings : Control
    {
        private readonly Label horzAlignLabel = new("Horz Align:");
        private readonly Label vertAlignLabel = new("Vert Align:");
        private readonly Label trimmingLabel = new("Trimming:");
        private readonly Label wrappingLabel = new("Wrapping:");
        private readonly ComboBox horizontalAlignmentComboBox = new();
        private readonly ComboBox verticalAlignmentComboBox = new();
        private readonly ComboBox wrappingComboBox = new();
        private readonly ComboBox trimmingComboBox = new();

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