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
        private readonly RichTextBox richText = new()
        {
            HasBorder = false,
        };

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

        public PreviewSampleWindow()
        {
            Closing += PreviewSampleWindow_Closing;
            State = WindowState.Normal;
            Margin = 10;
            Icon = App.DefaultIcon;
            Title = "Alternet.UI Preview File Sample";
            Size = (900, 700);
            StartLocation = WindowStartLocation.CenterScreen;
            panel.LeftPanel.Width = 400;
            panel.BottomPanel.Height = 200;
            panel.Parent = this;
            fileListBox.Parent = panel.LeftPanel;
            richText.Parent = panel.FillPanel;
            preview.Visible = false;
            preview.Parent = panel.FillPanel;
            logListBox.Parent = panel.BottomPanel;
            logListBox.Log("This demo is under development.");
            fileListBox.SelectionChanged += FileListBox_SelectionChanged;
            logListBox.BindApplicationLog();
            LoadWelcomePage();

            try
            {
                var appFolder = PathUtils.GetAppFolder();
                var samplesFolder = Path.Combine(appFolder, "Samples/Uixml");
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
                    if(stream is not null)
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
        }

        private void PreviewSampleWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            preview.Reset();
        }

        void SelectionChanged()
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

            App.LogIf($"Preview: {item.Path}", true);

            preview.FileName = item.Path;

            richText.Visible = false;
            preview.Visible = true;
            Refresh();
        }

        public void LoadWelcomePage()
        {
            var r = richText;

            r.ReadOnly = true;

            VerticalAlignment = VerticalAlignment.Stretch;
            HorizontalAlignment = HorizontalAlignment.Stretch;

            var baseFontSize = (int)Control.DefaultFont.SizeInPoints;

            r.SetDefaultStyle(r.CreateTextAttr());

            r.BeginUpdate();
            r.BeginSuppressUndo();

            r.BeginParagraphSpacing(0, 20);

            r.ApplyAlignmentToSelection(TextBoxTextAttrAlignment.Center);

            r.BeginBold();
            r.BeginFontSize(baseFontSize + 15);


            r.NewLine(2);
            r.WriteText("Preview Example");
            r.EndFontSize();
            r.EndBold();

            r.NewLine();
            r.BeginFontSize(baseFontSize + 2);
            r.WriteText("Select uixml or other supported file");
            r.NewLine();
            r.WriteText("and it will be previewed");

            r.EndFontSize();

            r.NewLine();

            var logoImage = Image.FromUrl("embres:ControlsSampleDll.Resources.logo128x128.png");
            r.WriteImage(logoImage);

            r.NewLine();

            r.NewLine(2);
            r.WriteText("Currently supported: uixml, html, images, sounds, txt.");
            r.NewLine(2);

            r.EndSuppressUndo();
            r.EndUpdate();
            r.ReadOnly = true;
            r.AutoUrlOpen = true;
            r.AutoUrlModifiers = Alternet.UI.ModifierKeys.None;
        }

        private void FileListBox_SelectionChanged(object? sender, EventArgs e)
        {
            App.AddIdleTask(SelectionChanged);
        }
    }
}
