using Alternet.UI;
using System;

namespace DrawingSample
{
    partial class ClippingPageSettings : Control
    {
        private ClippingPage? page;

        public ClippingPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(ClippingPage page)
        {
            this.page = page;
            DataContext = page;

            foreach (var value in Enum.GetValues(typeof(ClippingPage.ClipOperation)))
                clipOperationComboBox.Items.Add(value);
        }

        private void ResetButton_Click(object sender, System.EventArgs e)
        {
            page?.ResetClipAreaParts();
        }
    }
}