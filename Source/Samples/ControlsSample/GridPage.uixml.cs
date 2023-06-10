using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class GridPage : Control
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