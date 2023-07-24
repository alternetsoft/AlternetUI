using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.ComponentModel;

namespace ControlsSample
{
    internal partial class LayoutPanelPage : Control
    {
        private IPageSite? site;
        private readonly ListBox? control1;
        private readonly ListBox? control2;
        private readonly ListBox? control3;
        private readonly ListBox? control4;
        private readonly ListBox? control5;
        private readonly ListBox? control6;

        static LayoutPanelPage()
        {
        }

        public LayoutPanelPage()
        {
            InitializeComponent();

            control1 = new()
            {
                Margin = 5,
                Bounds = new(0, 0, 100, 100),
            };

            control2 = new()
            {
                Margin = 5,
                Bounds = new(150, 0, 100, 100),
            };

            control3 = new()
            {
                Margin = 5,
                Bounds = new(0, 150, 100, 100),
            };

            control4 = new()
            {
                Margin = 5,
                Bounds = new(150, 150, 100, 100),
            };

            control5 = new()
            {
                Margin = 5,
                Bounds = new(300, 300, 100, 100),
            };

            control6 = new()
            {
                Margin = 5,
                Bounds = new(500, 500, 100, 100),
            };

            control1.Items.Add("Control 1");
            control1.Items.Add("Dock = Right");

            control2.Items.Add("Control 2");
            control2.Items.Add("Dock = Left");

            control3.Items.Add("Control 3");
            control3.Items.Add("Dock = Top");

            control4.Items.Add("Control 4");
            control4.Items.Add("Dock = Bottom");

            control5.Items.Add("Control 5");
            control5.Items.Add("Dock = Fill");

            control6.Items.Add("Control 6");
            control6.Items.Add("Dock = None");

            LayoutPanel.SetDock(control1, DockStyle.Right);
            LayoutPanel.SetDock(control2, DockStyle.Left);
            LayoutPanel.SetDock(control3, DockStyle.Top);
            LayoutPanel.SetDock(control4, DockStyle.Bottom);
            LayoutPanel.SetDock(control5, DockStyle.Fill);

            layoutPanel.Children.Add(control5);
            layoutPanel.Children.Add(control1);
            layoutPanel.Children.Add(control2);
            layoutPanel.Children.Add(control3);
            layoutPanel.Children.Add(control4);
            //layoutPanel.Children.Add(control6);
            //layoutPanel.PerformLayout();

        }

        private void LogEventOnce(string s, bool once = true)
        {
            if (site == null)
                return;

            if (site.LastEventMessage == s && once)
                return;

            site?.LogEvent(s);
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