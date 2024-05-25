using System;
using Alternet.UI;

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
            foreach (var value in Enum.GetValues(typeof(ClippingPage.ClipOperation)))
                clipOperationComboBox.Items.Add(value);
            clipOperationComboBox.SelectedItem = ClippingPage.ClipOperation.Subtract;
        }

        private void ClipOperationComboBox_Changed(object? sender, System.EventArgs e)
        {
            if(page is not null
                && clipOperationComboBox.SelectedItem is ClippingPage.ClipOperation operation)
                page.SelectedClipOperation = operation;
        }

        private void ResetButton_Click(object? sender, System.EventArgs e)
        {
            page?.ResetClipAreaParts();
        }
    }
}