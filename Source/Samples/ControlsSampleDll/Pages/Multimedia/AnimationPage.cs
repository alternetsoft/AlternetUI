using System;
using System.IO;
using System.Linq;

using Alternet.UI;
using Alternet.Drawing;
using System.Collections.Generic;

namespace ControlsSample
{
    public partial class AnimationPage : VerticalStackPanel
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

        private readonly XButton playButton;
        private readonly XButton stopButton;
        private readonly XButton infoButton;
        private readonly XButton showFrameButton;

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

            public AnimatedImageItem(string name, IAnimatedImageSource image)
            {
                Text = name;
                AnimatedImage = image;
            }

            public string? Url { get; set; }

            public IAnimatedImageSource? AnimatedImage { get; set; }
        }

        public AnimationPage()
        {
            selectComboBox.UsePopupItemAsValue = true;

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

                // Uses animated image for the light theme but it's properties 
                // OtherThemeVariant and IsDark are property configured for the dark theme.
                // So, AnimationPlayer will choose the light theme animated image for the light theme
                // and dark theme animated image for the dark theme.
                new ("Svg Hour Glass", KnownAnimatedImages.SvgHourGlass32Light),

                // This is an example of how to create a blinking animation from a static image.
                // The blinking animation is created by using the CreateBlinking method of the
                // AnimatedImage class, which takes a static image and creates an animated image
                // that blinks on and off.
                new ("Blinking ", AnimatedImage.CreateBlinking(KnownSvgImages.ImgBell, 64).Light),

                // This is an example of how to create a blinking animation from a static image with a custom color.
                // The blinking animation is created by using the CreateBlinkingColor method of the
                // AnimatedImage class, which takes a static image and creates an animated image
                // that cycles through a set of colors, creating a blinking effect. In this case,
                // the color of the second frame is set to blue. The first frame uses the default svg color.
                new ("Blinking Color", AnimatedImage.CreateBlinkingColor(KnownSvgImages.ImgClock, 64, ThemedColors.Blue).Light),

                new (AnimationCustom, AnimationCustom),
            };

            selectComboBox.AddRange(animationUrls);
            selectComboBox.Value = defaultItem;
            selectComboBox.Parent = this;
            selectComboBox.ValueChanged += SelectComboBox_SelectedItemChanged;

            var stackPanel = new VerticalStackPanel(this);

            playButton = new XButton("Play", () => { animation.Play(); });
            stopButton = new XButton("Stop", animation.Stop);
            infoButton = new XButton("Info", ShowInfo);
            showFrameButton = new XButton("Frame 0", ShowFrame);

            new ControlSet(playButton, stopButton, infoButton, showFrameButton)
            .Margin(5).HorizontalAlignment(HorizontalAlignment.Left)
            .Parent(stackPanel).MaxWidthOnSizeChanged();
        }

        private void ShowFrame()
        {
            var bitmap = animation.GetFrame(0);

            if (bitmap != null)
            {
                popup.MainControl.Image = (Image)bitmap;
                popup.ShowPopup(showFrameButton, new(DropDownAlignment.AfterStart, DropDownAlignment.AfterEnd));
            }
        }

        private void ShowInfo()
        {
            var url = animation.AnimationUrl;

            if (url is not null)
                App.Log($"Animation.Url: {url}");
            App.Log($"Animation.IsOk: {animation.IsOk}");

            AnimatedImageExtractor.LogFrames(animation.GetAnimatedImage()?.Frames, 10, LogWriter.Application);
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
                        App.Log($"Loaded animation from url: {item.Url}");
                    }
                }
                else
                    if (item.AnimatedImage is not null)
                    {
                        animation.IsAnimationScaled = item.AnimatedImage.AnimatedImageSet is null;
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