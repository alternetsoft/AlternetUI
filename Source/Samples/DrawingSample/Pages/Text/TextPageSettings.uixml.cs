using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class TextPageSettings : Control
    {
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
        }
    }
}