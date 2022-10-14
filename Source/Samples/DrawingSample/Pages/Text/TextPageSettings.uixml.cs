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
            DataContext = page;

            foreach (var family in FontFamily.Families)
                customFontFamilyComboBox.Items.Add(family.Name);

            foreach (var value in Enum.GetValues(typeof(TextHorizontalAlignment)))
                horizontalAlignmentComboBox.Items.Add(value!);

            foreach (var value in Enum.GetValues(typeof(TextVerticalAlignment)))
                verticalAlignmentComboBox.Items.Add(value!);

            foreach (var value in Enum.GetValues(typeof(TextTrimming)))
                trimmingComboBox.Items.Add(value!);

            foreach (var value in Enum.GetValues(typeof(TextWrapping)))
                wrappingComboBox.Items.Add(value!);
        }
    }
}