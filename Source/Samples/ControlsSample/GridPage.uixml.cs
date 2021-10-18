using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class GridPage : Control
    {
        private readonly IPageSite site;

        public GridPage(IPageSite site)
        {
            InitializeComponent();

            this.site = site;
        }
    }
}