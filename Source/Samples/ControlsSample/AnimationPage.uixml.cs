using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class AnimationPage : Control
    {
        private readonly AnimationControl animation = new();
        private IPageSite? site;

        public AnimationPage()
        {
            InitializeComponent();
            animation.Parent = mainPanel;
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