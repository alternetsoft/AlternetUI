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
            DataContext = page;

            foreach (var family in FontFamily.Families)
                customFontFamilyComboBox.Items.Add(family.Name);

            customFontFamilyComboBox.SelectedItem = page.CustomFontFamilyName;
        }

        private void CustomFontFamilyComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            page.CustomFontFamilyName = ((ComboBox)sender!).SelectedItem?.ToString() ?? throw new Exception();
        }
    }
}