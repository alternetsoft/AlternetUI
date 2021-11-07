using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class CheckBoxesPage : Control
    {
        private readonly IPageSite site;

        public CheckBoxesPage(IPageSite site)
        {
            InitializeComponent();

            this.site = site;
        }
    }
}