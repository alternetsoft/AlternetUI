using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alternet.UI;

namespace ControlsSample
{
    internal class PreviewSampleWindow : Window
    {
        private readonly SplittedPanel panel = new()
        {
            TopVisible = false,
            RightVisible = false,
        };

        private readonly FileListBox fileListBox = new()
        {
            HasBorder = false,
        };

        private readonly PreviewUixml preview = new()
        {

        };

        private readonly LogListBox logListBox = new()
        {
            HasBorder = false,
        };

        public PreviewSampleWindow()
        {
            Closing += PreviewSampleWindow_Closing;
            State = WindowState.Normal;
            Margin = 10;
            Icon = Application.DefaultIcon;
            Title = "Alternet.UI Preview File Sample";
            Size = (900, 700);
            StartLocation = WindowStartLocation.CenterScreen;
            panel.LeftPanel.Width = 400;
            panel.BottomPanel.Height = 200;
            panel.Parent = this;
            fileListBox.Parent = panel.LeftPanel;
            fileListBox.SearchPattern = "*.uixml";
            preview.Parent = panel.FillPanel;
            logListBox.Parent = panel.BottomPanel;
            logListBox.Log("Select uixml or other supported file and it will be previewed");
            logListBox.Log("This demo is under development.");
            fileListBox.SelectionChanged += FileListBox_SelectionChanged;
            logListBox.BindApplicationLog();

            try
            {
                var samplesFolder = Path.GetFullPath("Samples/Uixml");
                if (!Directory.Exists(samplesFolder))
                    Directory.CreateDirectory(samplesFolder);
                fileListBox.SelectedFolder = samplesFolder;
                CopyAssetsAsync(samplesFolder);
            }
            catch
            {
                fileListBox.AddSpecialFolders();
            }

            void CopyAssetsAsync(string destFolder)
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

            void CopyAssets(string destFolder)
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
                    StreamUtils.CopyStream(stream, destPath);
                    number++;
                }

                if (number > 0)
                {
                    Application.InvokeIdleLog($"Extracted {number} uixml resources");
                    Application.InvokeIdle(() =>
                    {
                        fileListBox.Reload();
                    });
                }
            }
        }

        private void PreviewSampleWindow_Closing(object sender, WindowClosingEventArgs e)
        {
            preview.Reset();
        }

        void SelectionChanged()
        {
            var item = fileListBox.SelectedItem;

            Application.LogIf($"Preview file: {item?.Path}", true);

            if (item is null || item.Path is null || !item.IsFile)
            {
                preview.FileName = null;
                return;
            }

            var ext = item.ExtensionLower;
            if (ext == "uixml")
                preview.FileName = item.Path;
            else
                preview.FileName = null;
        }

        private void FileListBox_SelectionChanged(object sender, EventArgs e)
        {
            Application.AddIdleTask(SelectionChanged);
        }
    }
}
