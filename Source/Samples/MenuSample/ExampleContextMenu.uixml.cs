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
            Application.Log("Open");

        private void SaveMenuItem_Click(object sender, EventArgs e) =>
            Application.Log("Save");

        private void ExportToPdfMenuItem_Click(object sender, EventArgs e) =>
            Application.Log("Export to PDF");

        private void ExportToPngMenuItem_Click(object sender, EventArgs e) =>
            Application.Log("Export to PNG");
    }
}
