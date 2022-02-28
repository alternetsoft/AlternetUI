using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class CheckBoxesPage : Control
    {
        private IPageSite site;

        public CheckBoxesPage()
        {
            InitializeComponent();
        }

        public IPageSite Site
        {
            get => site;

            set
            {
                site = value;
            }
        }
    }
}