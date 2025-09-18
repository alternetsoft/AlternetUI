using Alternet.UI;

namespace ControlsSample
{
    public class SampleContextMenu : ContextMenu
    {
        public SampleContextMenu()
        {
            var openMenuItem = new MenuItem
            {
                Text = "_Open...",
                Shortcut = "Ctrl+Alt+1",
                SvgImage = KnownSvgImages.ImgFileOpen,
            };
            openMenuItem.Click += OpenMenuItem_Click;

            var saveMenuItem = new MenuItem
            {
                Text = "_Save...",
                Shortcut = "Ctrl+Alt+2",
                SvgImage = KnownSvgImages.ImgFileSave,
            };
            saveMenuItem.Click += SaveMenuItem_Click;

            var separator = new MenuItem { Text = "-" };

            var importMenuItem = new MenuItem
            {
                Text = "_Import sub-menu",
            };

            var importPdfMenuItem = new MenuItem
            {
                Text = "Import from P_DF",
            };

            importPdfMenuItem.Click += ExportToPdfMenuItem_Click;

            var importPngMenuItem = new MenuItem
            {
                Text = "Import from P_NG",
                IsEnabled = false,
            };
            importPngMenuItem.Click += ExportToPngMenuItem_Click;

            var exportMenuItem = new MenuItem
            {
                Text = "_Export sub-menu",
            };

            var exportToPdfMenuItem = new MenuItem
            {
                Text = "Export to P_DF",
            };

            exportToPdfMenuItem.Click += ExportToPdfMenuItem_Click;

            var exportToPngMenuItem = new MenuItem
            {
                Text = "Export to P_NG",
                IsEnabled = false,
            };
            exportToPngMenuItem.Click += ExportToPngMenuItem_Click;

            exportMenuItem.Items.Add(exportToPdfMenuItem);
            exportMenuItem.Items.Add(exportToPngMenuItem);

            importMenuItem.Items.Add(importPdfMenuItem);
            importMenuItem.Items.Add(importPngMenuItem);

            Items.Add(openMenuItem);
            Items.Add(saveMenuItem);
            Items.Add(separator);
            Items.Add(exportMenuItem);
            Items.Add(importMenuItem);
        }

        private void OpenMenuItem_Click(object? sender, EventArgs e)
        {
            App.Log("Handle open logic");
        }

        private void SaveMenuItem_Click(object? sender, EventArgs e)
        {
            App.Log("Handle save logic");
        }

        private void ExportToPdfMenuItem_Click(object? sender, EventArgs e)
        {
            App.Log("Handle export to PDF");
        }

        private void ExportToPngMenuItem_Click(object? sender, EventArgs e)
        {
            App.Log("Handle export to PNG");
        }
    }
}
