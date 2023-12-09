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
        private static readonly string ResPrefix = $"embres:ControlsSample.Resources.Animation.";

        internal static readonly string AnimationPlant = $"{ResPrefix}Plant.gif";
        internal static readonly string AnimationHourGlass = $"{ResPrefix}HourGlass.gif";
        internal static readonly string AnimationSpinner = $"{ResPrefix}Spinner.gif";
        internal static readonly string AnimationAlternet = $"{ResPrefix}Alternet.gif";
        internal static readonly string AnimationCustom = "Open animation file (*.gif; *.ani)...";

        private readonly AnimationPlayer animation = new();            
        
        private readonly ComboBox selectComboBox = new()
        {
            Margin = 5,
        };

        public AnimationPage()
        {
            Padding = 10;
            var defaultAnimationUrl = AnimationPlant;

            animation.Parent = this;
            animation.UseGeneric = true;
            animation.LoadFromUrl(defaultAnimationUrl, AnimationType.Gif);
            animation.Play();

            selectComboBox.IsEditable = false;
            selectComboBox.Add(AnimationPlant);
            selectComboBox.Add(AnimationHourGlass);
            selectComboBox.Add(AnimationSpinner);
            selectComboBox.Add(AnimationAlternet);
            selectComboBox.Add(AnimationCustom);
            selectComboBox.SelectedItem = defaultAnimationUrl;
            selectComboBox.Parent = this;

            selectComboBox.SelectedItemChanged += SelectComboBox_SelectedItemChanged;

            var buttonPanel = AddHorizontalStackPanel();
            buttonPanel.AddButton("Play", animation.Play);
            buttonPanel.AddButton("Stop", animation.Stop);
            buttonPanel.ChildrenSet.Margin(5).SuggestedWidthToMax();
        }

        private void SelectComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (selectComboBox.SelectedItem is not string url)
                return;

            if (url == AnimationCustom)
            {
                using var dialog = new OpenFileDialog();
                dialog.FileMustExist = true;
                var result = dialog.ShowModal(this);
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