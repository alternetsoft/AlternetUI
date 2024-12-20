using Alternet.UI;
using System;

namespace MenuSample
{
    public partial class ExampleContextMenu : ContextMenu
    {
        public ExampleContextMenu()
        {
            InitializeComponent();
        }

        private void OpenMenuItem_Click(object sender, EventArgs e) =>
            App.Log("Open from context menu");

        private void SaveMenuItem_Click(object sender, EventArgs e) =>
            App.Log("Save from context menu");

        private void ExportToPdfMenuItem_Click(object sender, EventArgs e) =>
            App.Log("Export to PDF from context menu");

        private void ExportToPngMenuItem_Click(object sender, EventArgs e) =>
            App.Log("Export to PNG from context menu");
    }
}
