using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class RadioButtonsPage : Control
    {
        private readonly IPageSite site;

        public RadioButtonsPage(IPageSite site)
        {
            InitializeComponent();

            this.site = site;
        }
    }
}