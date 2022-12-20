using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class ScrollingPage : Control
    {
        private IPageSite? site;

        public ScrollingPage()
        {
            InitializeComponent();

            for (int i = 0; i < 50; i++)
            {
                stackPanel.Children.Add(new Button("Button " + i));
            }
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