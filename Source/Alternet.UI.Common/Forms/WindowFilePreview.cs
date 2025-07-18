using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a window that previews files using different preview controls.
    /// </summary>
    public class WindowFilePreview : Window
    {
        private static WindowFilePreview? defaultWindow;

        private readonly SplittedPanel panel = new()
        {
            TopVisible = false,
            RightVisible = false,
        };

        private readonly FileListBox fileListBox = new()
        {
            HasBorder = false,
        };

        private readonly PreviewFile preview = new()
        {
        };

        private readonly LogListBox logListBox = new()
        {
            HasBorder = false,
        };

        private readonly Label pathLabel = new()
        {
            MarginBottom = 10,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowFilePreview"/> class.
        /// </summary>
        public WindowFilePreview()
        {
            Layout = LayoutStyle.Vertical;
            Padding = 10;

            pathLabel.Parent = this;

            /*
                PreviewUixml.ShowExceptionDialog = true;
                PreviewUixml.LoaderFlags =
                    UixmlLoader.Flags.ReportError | UixmlLoader.Flags.ShowErrorDialog;
                App.SetUnhandledExceptionModes(UnhandledExceptionMode.ThrowException);
            */

            Closing += PreviewSampleWindow_Closing;
            State = WindowState.Maximized;
            Margin = 10;
            Icon = App.DefaultIcon;
            Title = "Alternet.UI Preview File";
            Size = (900, 700);
            StartLocation = WindowStartLocation.CenterScreen;

            panel.LeftPanel.Width = 400;
            panel.BottomPanel.Height = 200;
            panel.VerticalAlignment = VerticalAlignment.Fill;
            panel.Parent = this;

            fileListBox.Parent = panel.LeftPanel;

            preview.Visible = false;
            preview.Parent = panel.FillPanel;

            logListBox.Parent = panel.BottomPanel;
            logListBox.BindApplicationLog();

            fileListBox.SelectionChanged += FileListBox_SelectionChanged;

            try
            {
                fileListBox.SelectedFolder = null;
            }
            catch
            {
                fileListBox.AddSpecialFolders();
            }

            preview.RegisterDefaultPreviewControls();
            preview.Visible = true;

            pathLabel.WordWrap = true;

            fileListBox.SelectedFolderChanged += (s, e) =>
            {
                pathLabel.Text = fileListBox.SelectedFolder ?? string.Empty;
            };
        }

        internal static new WindowFilePreview Default
        {
            get
            {
                if (defaultWindow is null)
                {
                    defaultWindow = new();
                    defaultWindow.Closing += Window_Closing;
                    defaultWindow.Disposed += Window_Disposed;
                }

                return defaultWindow;

                static void Window_Closing(object? sender, WindowClosingEventArgs e)
                {
                }

                static void Window_Disposed(object? sender, EventArgs e)
                {
                    defaultWindow = null;
                }
            }
        }

        /// <summary>
        /// Displays the file preview window.
        /// </summary>
        public static void ShowPreviewWindow()
        {
            ShowPreviewWindow(null);
        }

        /// <summary>
        /// Displays the file preview window for a specified file or folder.
        /// </summary>
        /// <param name="fileNameOrFolder">
        /// The file name or folder path to preview. If null, the default folder is selected.
        /// </param>
        public static void ShowPreviewWindow(string? fileNameOrFolder)
        {
            Default.fileListBox.SelectFolderByFileName(fileNameOrFolder);
            Default.ShowAndFocus();
        }

        internal void CopyAssetsAsync(string destFolder)
        {
            var thread1 = new Thread(ThreadAction1)
            {
                IsBackground = true,
            };

            thread1.Start();

            void ThreadAction1()
            {
                CopyAssets(destFolder);
            }
        }

        internal void CopyAssets(string destFolder)
        {
            var assembly = this.GetType().Assembly;

            var resources = assembly.GetManifestResourceNames();

            var number = 0;

            foreach (var item in resources)
            {
                var ext = PathUtils.GetExtensionLower(item);
                if (ext != "uixml")
                    continue;
                var destPath = Path.Combine(destFolder, item);
                if (File.Exists(destPath))
                    continue;
                using var stream = assembly.GetManifestResourceStream(item);
                if (stream is not null)
                {
                    StreamUtils.CopyStream(stream, destPath);
                    number++;
                }
            }

            if (number > 0)
            {
                App.InvokeIdleLog($"Extracted {number} uixml resources");
                App.InvokeIdle(() =>
                {
                    fileListBox.Reload();
                });
            }
        }

        private void PreviewSampleWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            preview.Reset();
        }

        private void SelectionChanged()
        {
            if (fileListBox.IsReloading)
            {
                preview.FileName = null;
                return;
            }

            var item = fileListBox.SelectedItem;

            if (item is null || item.Path is null || !fileListBox.ItemIsFile(item))
            {
                preview.FileName = null;
                return;
            }

            App.LogIf($"Preview: {item.Path}", false);

            preview.FileName = item.Path;

            preview.Visible = true;
            Refresh();
        }

        private void FileListBox_SelectionChanged(object? sender, EventArgs e)
        {
            App.AddIdleTask(SelectionChanged);
        }
    }
}
