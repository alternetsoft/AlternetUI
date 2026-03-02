using System;
using System.IO;
using System.Linq;

using Alternet.UI;

namespace ControlsSample
{
    internal partial class AnimationPage : VerticalStackPanel
    {
        private static readonly string ResPrefix = $"embres:ControlsSampleDll.Resources.Animation.";

        internal static readonly string AnimationPlant = $"{ResPrefix}Plant.gif";
        internal static readonly string AnimationSpinner = $"{ResPrefix}Spinner.gif";
        internal static readonly string AnimationAlternet = $"{ResPrefix}Alternet.gif";
        internal static readonly string AnimationCustom = "Open animation file (*.gif, *.webp)...";

        private readonly AnimationPlayer animation = new();

        private readonly PopupPictureBox popup = new()
        {
            Size = (200, 200),
            Title = "Frame 0",
        };

        private readonly ListPicker selectComboBox = new()
        {
            Margin = 5,
        };

        private readonly Button playButton;
        private readonly Button stopButton;
        private readonly Button infoButton;
        private readonly Button showFrameButton;

        static AnimationPage()
        {
        }

        public class AnimatedImageItem : ListControlItem
        {
            public AnimatedImageItem(string name, string url)
            {
                Text = name;
                Url = url;
            }

            public AnimatedImageItem(string name, AnimatedImageSet image)
            {
                Text = name;
                AnimatedImageSet = image;
            }

            public AnimatedImageItem(string name, AnimatedImage image)
            {
                Text = name;
                AnimatedImage = image;
            }

            public string? Url { get; set; }

            public AnimatedImageSet? AnimatedImageSet { get; set; }

            public AnimatedImage? AnimatedImage { get; set; }
        }

        public AnimationPage()
        {
            Padding = 10;

            animation.Parent = this;
            animation.IsAnimationScaled = true;
            animation.AnimatedImage = KnownAnimatedImages.HourGlass;
            animation.Play();

            new HorizontalLine
            {
                Margin = 5,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            }.Parent = this;

            new Label
            {
                Text = "Select animation to play:",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = 5,
            }.Parent = this;

            AnimatedImageItem defaultItem = new("Hour Glass", KnownAnimatedImages.HourGlass);

            List<AnimatedImageItem> animationUrls = new()
            {
                defaultItem,
                new ("Plant", AnimationPlant),
                new ("Spinner", AnimationSpinner),
                new ("Alternet", AnimationAlternet),
                new ("Dual Ring", KnownAnimatedImages.DualRingSet),
                new (AnimationCustom, AnimationCustom),
            };

            selectComboBox.AddRange(animationUrls);
            selectComboBox.Value = defaultItem;
            selectComboBox.Parent = this;
            selectComboBox.ValueChanged += SelectComboBox_SelectedItemChanged;

            var stackPanel = new VerticalStackPanel(this);

            playButton = new Button("Play", () => { animation.Play(); });
            stopButton = new Button("Stop", animation.Stop);
            infoButton = new Button("Info", ShowInfo);
            showFrameButton = new Button("Frame 0", ShowFrame);

            new ControlSet(playButton, stopButton, infoButton, showFrameButton)
            .Margin(5).HorizontalAlignment(HorizontalAlignment.Left)
            .Parent(stackPanel).MaxWidthOnSizeChanged();
        }

        private void ShowFrame()
        {
            var url = animation.AnimationUrl;
            var bitmap = AnimatedImageExtractor.GetFrame(url, 0);

            if (bitmap != null)
            {
                popup.MainControl.Image = (Image)bitmap;
                popup.ShowPopup(showFrameButton, new(DropDownAlignment.AfterStart, DropDownAlignment.AfterEnd));
            }
        }

        private void ShowInfo()
        {
            var url = animation.AnimationUrl;

            App.Log($"Animation.IsOk: {animation.IsOk}");

            AnimatedImageExtractor.LogFrames(url, 10, LogWriter.Application);
        }

        private void SelectComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (selectComboBox.Value is not AnimatedImageItem item)
                return;

            if (item.Text == AnimationCustom)
            {
                var dialog = OpenFileDialog.Default;
                dialog.FileMustExist = true;

                var filter = FileMaskUtils.ToFileDialogFilter(
                    new(FileDialogFilterItem.Kind.AnimationFiles),
                    new(".gif"),
                    new(".webp"));

                dialog.Filter = filter;

                dialog.ShowAsync(this.ParentWindow, () =>
                {
                    if (File.Exists(dialog.FileName))
                    {
                        animation.Stop();
                        animation.IsAnimationScaled = true;
                        if (!animation.LoadFile(dialog.FileName))
                        {
                            App.Log($"Error loading file: {dialog.FileName}");
                        }
                        else
                        {
                            selectComboBox.Value = dialog.FileName;
                        }

                        animation.Play();
                    }
                    else
                        App.Log($"File not found: {dialog.FileName}");
                });
            }
            else
            {
                animation.Stop();

                if (item.Url is not null)
                {
                    var result = animation.LoadFromUrl(item.Url);
                    animation.IsAnimationScaled = true;
                    if (!result)
                    {
                        App.Log($"Error loading animation from url: {item.Url}");
                    }
                    else
                    {

                    }
                }
                else
                    if (item.AnimatedImageSet is not null)
                    {
                        animation.IsAnimationScaled = false;

                        animation.AnimatedImage = item.AnimatedImageSet.GetImage(ScaleFactor);
                    }
                    else
                        if (item.AnimatedImage is not null)
                        {
                            animation.IsAnimationScaled = true;
                            animation.AnimatedImage = item.AnimatedImage;
                        }
                        else
                        {
                            App.Log($"No animation source for item: {item.Text}");
                        }

                animation.Play();
            }
        }
    }
}