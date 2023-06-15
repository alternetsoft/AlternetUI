using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class CheckBoxesPage : Control
    {
        private IPageSite? site;

        public CheckBoxesPage()
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