using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;
using Alternet.UI.Extensions;
using Alternet.Drawing;

using SkiaSharp;
using System.Diagnostics;

namespace ControlsSample
{
    internal class SkiaSharpExamplesWindow : Window
    {
        private readonly SplittedPanel mainPanel = new()
        {
            RightPanelWidth = 350,
            TopVisible = false,
            BottomVisible = false,
            LeftVisible = false,
        };

        private readonly ActionsListBox actionsListBox = new()
        {
            HasBorder = false,
        };

        private readonly PictureBox pictureBox = new()
        {
            ImageStretch = false,
        };

        private readonly SideBarPanel rightPanel = new()
        {
        };

        private SkiaSharpSample.SampleBase? currentSample;

        public SkiaSharpExamplesWindow()
        {
            Layout = LayoutStyle.Vertical;

            mainPanel.Parent = this;
            mainPanel.VerticalAlignment = VerticalAlignment.Fill;

            rightPanel.Parent = mainPanel.RightPanel;

            rightPanel.Add(GenericStrings.TabTitlePages, actionsListBox);

            pictureBox.Parent = mainPanel.CenterPanel;

            Title = "SkiaSharp MegaDemo";

            Size = (900, 700);

            SkiaSharpSample.SamplePlatforms platforms = 0;

            if (App.IsWindowsOS)
                platforms = SkiaSharpSample.SamplePlatforms.WindowsDesktop;
            else
            if (App.IsMacOS)
                platforms = SkiaSharpSample.SamplePlatforms.OSX;
            if (App.IsIOS)
                platforms = SkiaSharpSample.SamplePlatforms.iOS;
            if (App.IsAndroidOS)
                platforms = SkiaSharpSample.SamplePlatforms.Android;
            else
            if (App.IsLinuxOS)
                platforms = SkiaSharpSample.SamplePlatforms.WindowsDesktop;
            else
                platforms = SkiaSharpSample.SamplePlatforms.WindowsDesktop;

            var samples = SkiaSharpSample.SamplesManager.GetSamples(platforms);

            SkiaSharpSample.SamplesManager.OpenFile += SamplesManager_OpenFile;

            var memorySamples = samples
                .Where(s => s.SupportedBackends.HasFlag(SkiaSharpSample.SampleBackends.Memory));

            foreach (var sample in memorySamples)
            {
                sample.Init();
                sample.RefreshRequested += OnRefreshRequested;

                var newItem = new TreeViewItem(sample.Title);
                newItem.Action = () =>
                {
                    App.AddBackgroundInvokeAction(() =>
                    {
                        if (DisposingOrDisposed)
                            return;
                        currentSample = sample;
                        Draw(sample.DrawSample);
                    });
                };

                actionsListBox.Add(newItem);
            }

            if (actionsListBox.RootItem.ItemCount > 0)
            {
                actionsListBox.SelectionChanged += ActionsListBox_SelectionChanged;
                actionsListBox.ListBox.SelectedItemIsBold = true;

                App.AddIdleTask(() =>
                {
                    actionsListBox.SelectFirstItemAndScroll();
                    DrawSelectedItem();
                    actionsListBox.ListBox.Refresh();
                });
            }

            pictureBox.Click += PictureBox_Click;
        }

        private void PictureBox_Click(object? sender, EventArgs e)
        {
            currentSample?.Tap();
        }

        private void SamplesManager_OpenFile(string obj)
        {
            AppUtils.ShellExecute(obj);
        }

        private void OnRefreshRequested(object? sender, EventArgs e)
        {
            if (sender is not SkiaSharpSample.SampleBase sample)
                return;
            string title = sample.Title;
            if (title == (actionsListBox.SelectedItem as ListControlItem)?.Text)
            {
                DrawSelectedItem();
            }
        }

        private void Draw(Action<SKCanvas,int,int> action)
        {
            RectI rect = (0, 0, PixelFromDip(pictureBox.Width), PixelFromDip(pictureBox.Height));

            SKBitmap bitmap = new(rect.Width, rect.Height);

            SKCanvas canvas = new(bitmap);

            canvas.Clear(Color.White);

            action(canvas, rect.Width, rect.Height);

            var image = (Image)bitmap;
            pictureBox.Image = image;
            pictureBox.Refresh();
        }

        private void ActionsListBox_SelectionChanged(object? sender, EventArgs e)
        {
            DrawSelectedItem();
        }

        private void DrawSelectedItem()
        {
            Invoke(() =>
            {
                if (actionsListBox.SelectedItem is not ListControlItem item)
                    return;
                item.Action?.Invoke();
            });
        }
    }
}