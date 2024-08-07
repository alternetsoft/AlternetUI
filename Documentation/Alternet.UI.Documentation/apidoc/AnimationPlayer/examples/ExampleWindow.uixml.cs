using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples
{
    public partial class MainWindow : Window
    {
        private static readonly string ResPrefix
            = AssemblyUtils.GetImageUrlInAssembly(typeof(MainWindow).Assembly,"");

        internal static readonly string AnimationPlant = $"{ResPrefix}Plant.gif";
        internal static readonly string AnimationHourGlass = $"{ResPrefix}HourGlass.gif";

        public MainWindow()
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