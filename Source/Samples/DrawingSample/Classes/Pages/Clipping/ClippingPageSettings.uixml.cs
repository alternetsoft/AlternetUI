using System;
using Alternet.UI;

namespace DrawingSample
{
    partial class ClippingPageSettings : Panel
    {
        private ClippingPage? page;

        public ClippingPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(ClippingPage page)
        {
            this.page = page;
            clipOperationComboBox.EnumType = typeof(ClippingPage.ClipOperation);
            clipOperationComboBox.Value = ClippingPage.ClipOperation.Subtract;
        }

        private void ClipOperationComboBox_Changed(object? sender, System.EventArgs e)
        {
            if(page is not null
                && clipOperationComboBox.Value is ClippingPage.ClipOperation operation)
                page.SelectedClipOperation = operation;
        }

        private void ResetButton_Click(object? sender, System.EventArgs e)
        {
            page?.ResetClipAreaParts();
        }
    }
}