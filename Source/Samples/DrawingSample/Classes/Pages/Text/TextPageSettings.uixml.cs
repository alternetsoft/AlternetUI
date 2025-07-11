using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class TextPageSettings : Panel
    {
        private readonly Label horzAlignLabel = new("Horz Align:");
        private readonly Label vertAlignLabel = new("Vert Align:");
        private readonly Label trimmingLabel = new("Trimming:");
        private readonly Label wrappingLabel = new("Wrapping:");
        private readonly EnumPicker horizontalAlignmentComboBox = new();
        private readonly EnumPicker verticalAlignmentComboBox = new();
        private readonly EnumPicker wrappingComboBox = new();
        private readonly EnumPicker trimmingComboBox = new();

        public TextPageSettings()
        {
            DoInsideLayout(() =>
            {
                InitializeComponent();

                var labels = Group(horzAlignLabel, vertAlignLabel, trimmingLabel, wrappingLabel);
                var comboBoxes = Group(
                    horizontalAlignmentComboBox,
                    verticalAlignmentComboBox,
                    trimmingComboBox,
                    wrappingComboBox);

                Group(
                    trimmingComboBox,
                    wrappingComboBox).Enabled(false);

                labels.Margin(new(0, 5, 10, 5)).VerticalAlignment(VerticalAlignment.Center);
                comboBoxes.Margin(new(0, 5, 0, 5)).IsEditable(false);
                var gridControls = ControlSet.GridFromColumns(labels, comboBoxes);

                propertyGrid.Visible = DebugUtils.IsDebugDefined;

                propGrid.Setup(gridControls);
            });
        }

        public void Initialize(TextPage page)
        {
            DataContext = page;

            if (DebugUtils.IsDebugDefined)
            {
                /*
                AddProperty(
                    page.wrappedControl,
                    nameof(AbstractControl.HorizontalAlignment),
                    "Block Horz");

                AddProperty(
                    page.wrappedControl,
                    nameof(AbstractControl.VerticalAlignment),
                    "Block Vert");
                */

                AddProperty(
                    page.textFormat,
                    nameof(TextFormat.PaddingTop));

                AddProperty(
                    page.textFormat,
                    nameof(TextFormat.PaddingBottom));

                AddProperty(
                    page.textFormat,
                    nameof(TextFormat.PaddingLeft));

                AddProperty(
                    page.textFormat,
                    nameof(TextFormat.PaddingRight));

                AddProperty(
                    page.textFormat,
                    nameof(TextFormat.Distance));
            }

            void AddProperty(object obj, string name, string? label = null)
            {
                var prop = propertyGrid.CreateProperty(label, name, obj, name);
                propertyGrid.Add(prop);
            }

            propertyGrid.ApplyFlags = PropertyGridApplyFlags.SetValueAndReloadAll;

            propertyGrid.FitColumns();
            propertyGrid.PropertyChanged += (s, e) =>
            {
                page.InvalidateParagraphs();
            };

            toggleTextButton.Click += (s, e) =>
            {
                page.ShortText = !page.ShortText;
            };

            wrappingComboBox.EnumType = typeof(TextWrapping);
            trimmingComboBox.EnumType = typeof(TextTrimming);
            verticalAlignmentComboBox.EnumType = typeof(TextVerticalAlignment);
            horizontalAlignmentComboBox.EnumType = typeof(TextHorizontalAlignment);

            horizontalAlignmentComboBox.Value = page.HorizontalAlignment;
            horizontalAlignmentComboBox.ValueChanged += (s, e) =>
            {
                page.HorizontalAlignment = 
                    horizontalAlignmentComboBox.ValueAs<TextHorizontalAlignment>();
            };

            verticalAlignmentComboBox.Value = page.VerticalAlignment;
            verticalAlignmentComboBox.ValueChanged += (s, e) =>
            {
                page.VerticalAlignment
                    = verticalAlignmentComboBox.ValueAs<TextVerticalAlignment>();
            };

            wrappingComboBox.Value = page.Wrapping;
            wrappingComboBox.ValueChanged += (s, e) =>
            {
                page.Wrapping = wrappingComboBox.ValueAs<TextWrapping>();
            };

            trimmingComboBox.Value = page.Trimming;
            trimmingComboBox.ValueChanged += (s, e) =>
            {
                page.Trimming = trimmingComboBox.ValueAs<TextTrimming>();
            };

            fontSizeSlider.Value = (int)page.FontSize;
            fontSizeSlider.ValueChanged += (s, e) =>
            {
                page.FontSize = fontSizeSlider.Value;
            };

            boldCheckBox.IsChecked = page.Bold;
            boldCheckBox.CheckedChanged += (s, e) =>
            {
                page.Bold = boldCheckBox.IsChecked;
            };

            italicCheckBox.IsChecked = page.Italic;
            italicCheckBox.CheckedChanged += (s, e) =>
            {
                page.Italic = italicCheckBox.IsChecked;
            };

            underlinedCheckBox.IsChecked = page.Underlined;
            underlinedCheckBox.CheckedChanged += (s, e) =>
            {
                page.Underlined = underlinedCheckBox.IsChecked;
            };

            strikethroughCheckBox.IsChecked = page.Strikethrough;
            strikethroughCheckBox.CheckedChanged += (s, e) =>
            {
                page.Strikethrough = strikethroughCheckBox.IsChecked;
            };

            textWidthLimitEnabledCheckBox.IsChecked = page.TextWidthLimitEnabled;
            textWidthLimitEnabledCheckBox.CheckedChanged += (s, e) =>
            {
                page.TextWidthLimitEnabled = textWidthLimitEnabledCheckBox.IsChecked;
            };

            textHeightSetCheckBox.IsChecked = page.TextHeightSet;
            textHeightSetCheckBox.CheckedChanged += (s, e) =>
            {
                page.TextHeightSet = textHeightSetCheckBox.IsChecked;
            };

            textWidthLimitSlider.Minimum = page.MinTextWidthLimit;
            textWidthLimitSlider.Maximum = page.MaxTextWidthLimit;
            textWidthLimitSlider.Value = page.TextWidthLimit;
            textWidthLimitSlider.ValueChanged += (s, e) => {
                page.TextWidthLimit = textWidthLimitSlider.Value;
            };

            textHeightValueSlider.Minimum = page.MinTextHeightValue;
            textHeightValueSlider.Maximum = page.MaxTextHeightValue;
            textHeightValueSlider.Value= page.TextHeightValue;
            textHeightValueSlider.ValueChanged += (s, e) => {
                page.TextHeightValue = textHeightValueSlider.Value;
            };

            customFontFamilyComboBox.Value = page.CustomFontFamilyName;
            customFontFamilyComboBox.ValueChanged += (s, e) =>
            {
                page.CustomFontFamilyName = customFontFamilyComboBox.ValueAs<string>()
                    ?? Control.DefaultFont.Name;
            };

            GetChildrenRecursive().Action<Slider>((c) => c.ClearTicks());
        }
    }
}