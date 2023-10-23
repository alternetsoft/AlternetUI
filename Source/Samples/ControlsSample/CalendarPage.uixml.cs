using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class CalendarPage : Control
    {
        private readonly Calendar calendar = new();
        private IPageSite? site;

        public CalendarPage()
        {
            InitializeComponent();

            calendar.Parent = mainPanel;
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