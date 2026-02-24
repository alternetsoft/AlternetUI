using System;
using System.IO;
using System.Linq;
using Alternet.UI;


/*

2023-11-25: Thanks to @neoxeo! He added new gif animation and open animation file feature.
We approved his changes with little modifications.
 
*/

namespace ControlsSample
{
    internal partial class AnimationPage : VerticalStackPanel
    {
        private static readonly string ResPrefix = $"embres:ControlsSampleDll.Resources.Animation.";

        internal static readonly string AnimationPlant = $"{ResPrefix}Plant.gif";
        internal static readonly string AnimationHourGlass = $"{ResPrefix}HourGlass.gif";
        internal static readonly string AnimationSpinner = $"{ResPrefix}Spinner.gif";
        internal static readonly string AnimationAlternet = $"{ResPrefix}Alternet.gif";
        internal static readonly string AnimationCustom = "Open animation file (*.gif; *.ani)...";

        private readonly AnimationPlayer animation = new();

        private readonly PopupPictureBox popup = new()
        {
            Size = (200, 200),
            Title = "Frame 0",
        };
        
        private readonly EnumPicker selectComboBox = new()
        {
            Margin = 5,
        };

        Button playButton;
        Button stopButton;
        Button infoButton;
        Button showFrameButton;

        static AnimationPage()
        {
            AnimationPlayer.DefaultHandlerKind = AnimationPlayer.KnownHandler.Generic;
        }

        public AnimationPage()
        {
            Padding = 10;
            var defaultAnimationUrl = AnimationHourGlass;;

            animation.Parent = this;
            animation.LoadFromUrl(defaultAnimationUrl, AnimationType.Gif);
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

            List<string> animationUrls = new()
            {
                AnimationHourGlass,
                AnimationPlant,
                AnimationSpinner,
                AnimationAlternet,
                AnimationCustom
            };

            selectComboBox.AddRange(animationUrls);
            selectComboBox.Value = defaultAnimationUrl;
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
            var image = animation.GetFrame(0);
            popup.MainControl.Image = (Image)image;
            popup.ShowPopup(showFrameButton, new(DropDownAlignment.AfterStart, DropDownAlignment.AfterEnd));
        }

        private void ShowInfo()
        {
            App.Log($"Animation.IsOk: {animation.IsOk}");
            App.Log($"Animation.FrameCount: {animation.FrameCount}");
            App.Log($"Animation.FrameDelay[0]: {animation.GetDelay(0)}");
        }

        private void SelectComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (selectComboBox.Value is not string url)
                return;

            if (url == AnimationCustom)
            {
                var dialog = OpenFileDialog.Default;
                dialog.FileMustExist = true;
                dialog.Filter = FileMaskUtils.FileDialogFilterAllFiles;
                dialog.ShowAsync(this.ParentWindow, () =>
                {
                    if (File.Exists(dialog.FileName))
                    {
                        animation.Stop();
                        if (!animation.LoadFile(dialog.FileName!, AnimationType.Any))
                        {
                            App.Log($"Error loading file: {dialog.FileName}");
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
                animation.LoadFromUrl(url, AnimationType.Gif);
                animation.Play();
            }
        }
    }
}