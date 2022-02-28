using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class TextPageSettings : Control
    {
        private TextPage page;

        public TextPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(TextPage page)
        {
            this.page = page;

            fontSizeSlider.Value = (int)page.FontSize;

            foreach (var family in FontFamily.Families)
                customFontFamilyComboBox.Items.Add(family.Name);

            customFontFamilyComboBox.SelectedItem = page.CustomFontFamilyName;
        }

        private void CustomFontFamilyComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            page.CustomFontFamilyName = ((ComboBox)sender!).SelectedItem?.ToString() ?? throw new Exception();
        }

        private void FontSizeSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.FontSize = ((Slider)sender!).Value;
        }

        private void BoldCheckBox_CheckedChanged(object? sender, EventArgs e) => ApplyFontStyle(FontStyle.Bold, ((CheckBox)sender!).IsChecked);

        private void ItalicCheckBox_CheckedChanged(object? sender, EventArgs e) => ApplyFontStyle(FontStyle.Italic, ((CheckBox)sender!).IsChecked);

        private void UnderlinedCheckBox_CheckedChanged(object? sender, EventArgs e) => ApplyFontStyle(FontStyle.Underlined, ((CheckBox)sender!).IsChecked);

        private void StrikethroughCheckBox_CheckedChanged(object? sender, EventArgs e) => ApplyFontStyle(FontStyle.Strikethrough, ((CheckBox)sender!).IsChecked);

        private void ApplyFontStyle(FontStyle style, bool value)
        {
            if (value)
                page.FontStyle |= style;
            else
                page.FontStyle &= ~style;
        }
    }
}