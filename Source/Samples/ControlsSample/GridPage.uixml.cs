using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class GridPage : Control
    {
        private IPageSite? site;

        public GridPage()
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