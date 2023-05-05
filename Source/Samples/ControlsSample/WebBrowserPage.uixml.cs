using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class WebBrowserPage : Control
    {
        private IPageSite? site;

        public WebBrowserPage()
        {
            InitializeComponent();
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