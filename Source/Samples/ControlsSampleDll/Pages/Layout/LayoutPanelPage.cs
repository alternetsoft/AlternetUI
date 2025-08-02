using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.ComponentModel;

namespace ControlsSample
{
    internal partial class LayoutPanelPage : StackPanel
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
            HorizontalStackPanel btnPanel = new();
            btnPanel.Visible = false;

            ComboBox comboBox = new();
            comboBox.Parent = btnPanel;
            comboBox.Margin = 5;

            btnPanel.Parent = this;

            static AbstractControl Create(params string[] items)
            {
                StdListBox listBox = new()
                {
                };

                listBox.Items.AddRange(items);
                return listBox;
            }

            var cRight = Create("Control 1", "Dock = Right");
            var cLeft = Create("Control 2", "Dock = Left");
            var cTop = Create("Control 3", "Dock = Top");
            var cBottom = Create("Control 4", "Dock = Bottom");
            var cFill = Create("Control 5", "Dock = Fill");
            LayoutPanel layoutPanel = new();

            comboBox.BindEnumProp(layoutPanel, nameof(LayoutPanel.Layout));

            cRight.Dock = DockStyle.Right;
            cLeft.Dock = DockStyle.Left;
            cTop.Dock = DockStyle.Top;
            cBottom.Dock = DockStyle.Bottom;
            cFill.Dock = DockStyle.Fill;

            var sLeft = new Splitter
            {
                Dock = DockStyle.Left,
            };

            var sRight = new Splitter
            {
                Dock = DockStyle.Right,
            };

            var sTop = new Splitter
            {
                Dock = DockStyle.Top,
            };

            var sBottom = new Splitter
            {
                Dock = DockStyle.Bottom,
            };

            Group(cFill, cRight, cLeft, cTop, cBottom).Size(100);

            Group(cFill, sRight, cRight, sLeft, cLeft, sTop, cTop, sBottom, cBottom).Parent(layoutPanel);

            layoutPanel.SuggestedHeight = 300;
            layoutPanel.Parent = this;
        }

        internal void LogEventOnce(string s, bool once = true)
        {
            if(once)
                App.LogReplace(s, s);
            else
                App.Log(s);
        }
    }
}