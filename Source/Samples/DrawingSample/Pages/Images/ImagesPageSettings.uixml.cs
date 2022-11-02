using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class ImagesPageSettings : Control
    {
        public ImagesPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(ImagesPage page)
        {
            DataContext = page;

            foreach (var value in Enum.GetValues(typeof(InterpolationMode)))
                interpolationModeComboBox.Items.Add(value!);
        }
    }
}