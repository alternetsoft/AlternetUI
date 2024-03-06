using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class ImagesPageSettings : Control
    {
        ImagesPage? page;

        public ImagesPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(ImagesPage page)
        {
            this.page = page;
            foreach (var value in Enum.GetValues(typeof(InterpolationMode)))
                interpolationModeComboBox.Items.Add(value!);
            interpolationModeComboBox.SelectedItem = page.InterpolationMode;
        }

        private void InterpolationModeComboBox_Changed(object? sender, EventArgs e)
        {
            if (page is not null && interpolationModeComboBox.SelectedItem is not null)
                page.InterpolationMode = (InterpolationMode)interpolationModeComboBox.SelectedItem;
        }
    }
}