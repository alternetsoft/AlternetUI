using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class ImagesPageSettings : Panel
    {
        ImagesPage? page;

        public ImagesPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(ImagesPage page)
        {
            this.page = page;

            ListControlItem[] modes =
            [
                new("LowQuality", InterpolationMode.LowQuality),
                new("MediumQuality", InterpolationMode.MediumQuality),
                new("HighQuality", InterpolationMode.HighQuality),
                new("NearestNeighbor", InterpolationMode.NearestNeighbor),
            ];

            interpolationModeComboBox.Items.AddRange(modes);
            var item = interpolationModeComboBox.FindItemWithValue(page.InterpolationMode);
            interpolationModeComboBox.Value = item;
        }

        private void InterpolationModeComboBox_Changed(object? sender, EventArgs e)
        {
            if (page is not null && interpolationModeComboBox.Value is InterpolationMode im)
                page.InterpolationMode = im;
        }
    }
}