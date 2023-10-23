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

        private readonly AnimationControl animation = new();
        
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
        }

        private void SelectComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            var url = selectComboBox.SelectedItem as string;
            if (url is null)
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