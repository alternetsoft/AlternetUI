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
        static LayoutPanelPage()
        {
        }

        public LayoutPanelPage()
        {
            Margin = 10;
            DoInsideLayout(Initialize);
        }

        public void Initialize()
        {
            static Control Create(params string[] items)
            {
                ListBox listBox = new()
                {
                    HasBorder = false,
                };

                Border border = new();

                listBox.Parent = border;
                listBox.Items.AddRange(items);
                return border;
            }

            var control1 = Create("Control 1", "Dock = Right");
            var control2 = Create("Control 2", "Dock = Left");
            var control3 = Create("Control 3", "Dock = Top");
            var control4 = Create("Control 4", "Dock = Bottom");
            var control5 = Create("Control 5", "Dock = Fill");
            LayoutPanel layoutPanel = new();

            LayoutPanel.SetDock(control1, DockStyle.Right);
            LayoutPanel.SetDock(control2, DockStyle.Left);
            LayoutPanel.SetDock(control3, DockStyle.Top);
            LayoutPanel.SetDock(control4, DockStyle.Bottom);
            LayoutPanel.SetDock(control5, DockStyle.Fill);

            Group(control5, control1, control2, control3, control4).Margin(5).Size(100).Parent(layoutPanel);

            layoutPanel.SuggestedHeight = 300;
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