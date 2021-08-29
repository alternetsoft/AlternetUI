using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class TextPageSettings : Control
    {
        private readonly TextPage page;

        public TextPageSettings(TextPage page)
        {
            InitializeComponent();

            this.page = page;

            fontSizeSlider.Value = (int)page.FontSize;
            fontSizeSlider.ValueChanged += FontSizeSlider_ValueChanged;

            boldCheckBox.CheckedChanged += (o, e) => ApplyFontStyle(FontStyle.Bold, ((CheckBox)o!).IsChecked);
            italicCheckBox.CheckedChanged += (o, e) => ApplyFontStyle(FontStyle.Italic, ((CheckBox)o!).IsChecked);
            underlinedCheckBox.CheckedChanged += (o, e) => ApplyFontStyle(FontStyle.Underlined, ((CheckBox)o!).IsChecked);
            strikethroughCheckBox.CheckedChanged += (o, e) => ApplyFontStyle(FontStyle.Strikethrough, ((CheckBox)o!).IsChecked);

            foreach (var family in FontFamily.Families)
                customFontFamilyComboBox.Items.Add(family.Name);

            customFontFamilyComboBox.SelectedItem = page.CustomFontFamilyName;
            customFontFamilyComboBox.SelectedItemChanged += CustomFontFamilyComboBox_SelectedItemChanged;
        }

        private void CustomFontFamilyComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            page.CustomFontFamilyName = ((ComboBox)sender!).SelectedItem?.ToString() ?? throw new Exception();
        }

        private void FontSizeSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.FontSize = ((Slider)sender!).Value;
        }

        private void ApplyFontStyle(FontStyle style, bool value)
        {
            if (value)
                page.FontStyle |= style;
            else
                page.FontStyle &= ~style;
        }
    }
}