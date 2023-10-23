using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class AnimationPage : Control
    {
        private static readonly string ResPrefix = $"embres:ControlsSample.Resources.Animation.";

        internal static readonly string AnimationPlant = $"{ResPrefix}Plant.gif";
        internal static readonly string AnimationHourGlass = $"{ResPrefix}HourGlass.gif";
        internal static readonly string AnimationSpinner = $"{ResPrefix}Spinner.gif";

        private readonly AnimationPlayer animation = new();
        
        private readonly ComboBox selectComboBox = new()
        {
            Margin = 5,
        };

        private IPageSite? site;

        public AnimationPage()
        {
            var defaultAnimationUrl = AnimationPlant;

            InitializeComponent();
            animation.Parent = mainPanel;
            animation.LoadFromUrl(defaultAnimationUrl);
            animation.Play();

            selectComboBox.IsEditable = false;
            selectComboBox.Add(AnimationPlant);
            selectComboBox.Add(AnimationHourGlass);
            selectComboBox.Add(AnimationSpinner);
            selectComboBox.SelectedItem = defaultAnimationUrl;
            selectComboBox.Parent = mainPanel;

            selectComboBox.SelectedItemChanged += SelectComboBox_SelectedItemChanged;

            var buttonPanel = mainPanel.AddHorizontalStackPanel();
            buttonPanel.AddButton("Play", animation.Play);
            buttonPanel.AddButton("Stop", animation.Stop);
            buttonPanel.ChildrenSet.Margin(5).SuggestedWidthToMax();
        }

        private void SelectComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (selectComboBox.SelectedItem is not string url)
                return;
            animation.Stop();
            animation.LoadFromUrl(url);
            animation.Play();
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }
    }
}