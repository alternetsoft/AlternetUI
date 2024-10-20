using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class AnimationPlayerWindow : Window
    {
        private static readonly string ResPrefix
            = AssemblyUtils.GetImageUrlInAssembly(typeof(AnimationPlayerWindow).Assembly,"Resources.");

        internal static readonly string AnimationPlant = $"{ResPrefix}Plant.gif";
        internal static readonly string AnimationHourGlass = $"{ResPrefix}HourGlass.gif";

        public AnimationPlayerWindow()
        {
            InitializeComponent();
            CreateAnimationPlayer();
        }

        public AnimationPlayer CreateAnimationPlayer()
        {
            #region CSharpCreation
            AnimationPlayer result = new();

            result.Parent = mainPanel;
            result.LoadFromUrl(AnimationHourGlass, AnimationType.Gif);
            result.Play();

            return result;
            #endregion
        }
    }
}