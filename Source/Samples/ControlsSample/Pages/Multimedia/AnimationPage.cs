﻿using System;
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
        private static readonly string ResPrefix = $"embres:ControlsSample.Resources.Animation.";

        internal static readonly string AnimationPlant = $"{ResPrefix}Plant.gif";
        internal static readonly string AnimationHourGlass = $"{ResPrefix}HourGlass.gif";
        internal static readonly string AnimationSpinner = $"{ResPrefix}Spinner.gif";
        internal static readonly string AnimationAlternet = $"{ResPrefix}Alternet.gif";
        internal static readonly string AnimationCustom = "Open animation file (*.gif; *.ani)...";

        private readonly AnimationPlayer animation = new();
        private readonly PopupPictureBox popup = new();
        
        private readonly ComboBox selectComboBox = new()
        {
            Margin = 5,
        };

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

            selectComboBox.IsEditable = false;
            selectComboBox.Add(AnimationHourGlass);
            selectComboBox.Add(AnimationPlant);
            selectComboBox.Add(AnimationSpinner);
            selectComboBox.Add(AnimationAlternet);
            selectComboBox.Add(AnimationCustom);
            selectComboBox.SelectedItem = defaultAnimationUrl;
            selectComboBox.Parent = this;

            selectComboBox.SelectedItemChanged += SelectComboBox_SelectedItemChanged;

            AddVerticalStackPanel().AddButtons(
                ("Play", () => { animation.Play(); }),
                ("Stop", animation.Stop),
                ("Info", ShowInfo),
                ("Show frame 0", ShowFrame))
            .Margin(5).HorizontalAlignment(HorizontalAlignment.Left).SuggestedWidthToMax();
        }

        private void ShowFrame()
        {
            var image = animation.GetFrame(0);
            popup.MainControl.Image = (Image)image;
            popup.ShowPopup(animation);
        }

        private void ShowInfo()
        {
            Application.Log($"Animation.IsOk: {animation.IsOk}");
            Application.Log($"Animation.FrameCount: {animation.FrameCount}");
            Application.Log($"Animation.FrameDelay[0]: {animation.GetDelay(0)}");
        }

        private void SelectComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (selectComboBox.SelectedItem is not string url)
                return;

            if (url == AnimationCustom)
            {
                using var dialog = new OpenFileDialog();
                dialog.FileMustExist = true;
                var result = dialog.ShowModal(this.ParentWindow);
                if (result == ModalResult.Accepted)
                {
                    if (File.Exists(dialog.FileName))
                    {
                        animation.Stop();
                        if (!animation.LoadFile(dialog.FileName!, AnimationType.Any))
                        {
                            Application.Log($"Error loading file: {dialog.FileName}");
                        }
                        animation.Play();
                    }
                    else
                        Application.Log($"File not found: {dialog.FileName}");
                }
            }
            else
            {
                animation.Stop();
                animation.LoadFromUrl(url);
                animation.Play();
            }
        }
    }
}