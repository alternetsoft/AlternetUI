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
        private readonly ListBox control1 = new();
        private readonly ListBox control2 = new();
        private readonly ListBox control3 = new();
        private readonly ListBox control4 = new();
        private readonly ListBox control5 = new();
        private readonly ListBox control6 = new();
        private readonly LayoutPanel layoutPanel = new();

        static LayoutPanelPage()
        {
        }

        public LayoutPanelPage()
        {
            DoInsideLayout(Initialize);
        }

        public void Initialize()
        {
            static void Initialize(ListBox control)
            {
                control.Margin = 5;
                control.Size = (100, 100);
            }

            layoutPanel.BackgroundColor = Color.Yellow;

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

            Group(control1, control2, control3, control4, control5)
                .Action<ListBox>(Initialize).Parent(layoutPanel);

            layoutPanel.Parent = this;
        }

        internal void LogEventOnce(string s, bool once = true)
        {
            if(once)
                Application.LogReplace(s, s);
            else
                Application.Log(s);
        }
    }
}