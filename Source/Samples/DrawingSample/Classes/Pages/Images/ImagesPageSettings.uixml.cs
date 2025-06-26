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
            interpolationModeComboBox.EnumType = typeof(InterpolationMode);
            interpolationModeComboBox.Value = page.InterpolationMode;
        }

        private void InterpolationModeComboBox_Changed(object? sender, EventArgs e)
        {
            if (page is not null && interpolationModeComboBox.Value is InterpolationMode im)
                page.InterpolationMode = im;
        }
    }
}