using System;
using Alternet.UI;

namespace ControlsSample
{
    public partial class ExampleContextMenu : ContextMenu
    {
        public ExampleContextMenu()
        {
            InitializeComponent();
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
            => Log("ExampleContextMenu: Open");

        private void SaveMenuItem_Click(object sender, EventArgs e)
            => Log("ExampleContextMenu: Save");

        private void ExportToPdfMenuItem_Click(object sender, EventArgs e)
            => Log("ExampleContextMenu: Export to PDF");

        private void ExportToPngMenuItem_Click(object sender, EventArgs e)
            => Log("ExampleContextMenu: Export to PNG");

        private void ExampleMenuItem_Click(object sender, EventArgs e)
            => Log("ExampleContextMenu: item was clicked");
    }
}
