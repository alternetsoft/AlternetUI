using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class SplitterPanelPage : Control
    {
        private IPageSite? site;

        public SplitterPanelPage()
        {
            InitializeComponent();

            ListBox control1 = new() 
            {
                Margin = 5,
            };

            ListBox control2 = new()
            {
                Margin = 5,
            };

            control1.Items.Add("Control 1");
            control2.Items.Add("Control 2");

            splitterPanel.Children.Add(control1);
            splitterPanel.Children.Add(control2);

            splitterPanel.SplitVertically(200);
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