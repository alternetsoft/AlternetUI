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
        private ListBox? control1;
        private ListBox? control2;
        private ListBox? control3;

        static SplitterPanelPage()
        {
        }

        public SplitterPanelPage()
        {
            InitializeComponent();

            control1 = new()
            {
                Margin = 5,
            };

            control2 = new()
            {
                Margin = 5,
            };

            control3 = new()
            {
                Margin = 5,
                Visible = false,
            };

            control1.Items.Add("Control 1");
            control2.Items.Add("Control 2");
            control3.Items.Add("Control 3");

            splitterPanel.Children.Add(control1);
            splitterPanel.Children.Add(control2);
            splitterPanel.Children.Add(control3);

            splitterPanel.SplitterDoubleClick += SplitterPanel_SplitterDoubleClick;
            splitterPanel.SplitterMoved += SplitterPanel_SplitterMoved;
            splitterPanel.Unsplit += SplitterPanel_Unsplit;
            splitterPanel.SplitterMoving += SplitterPanel_SplitterMoving;

            splitterPanel.SplitVertical(control1, control2);
        }

        private void SplitterPanel_SplitterMoving(object? sender, EventArgs e)
        {
            const string s = "Splitter Panel: Splitter Moving";

            if (site == null)
                return;

            if (site.LastEventMessage == s)
                return;

            site?.LogEvent(s);
        }

        private void SplitterPanel_Unsplit(object? sender, EventArgs e)
        {
            site?.LogEvent("Splitter Panel: Unsplit");
        }

        private void SplitterPanel_SplitterMoved(object? sender, EventArgs e)
        {
            site?.LogEvent("Splitter Panel: Splitter Moved");
        }

        private void SplitterPanel_SplitterDoubleClick(object? sender, EventArgs e)
        {
            site?.LogEvent("Splitter Panel: Double click");
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        private void ReplaceControlButton_Click(object? sender, EventArgs e)
        {
            if (!splitterPanel.IsSplit)
                return;
            if (splitterPanel.Control2 == control2)
            {
                splitterPanel.ReplaceControl(control2, control3);
            }
            else
            {
                splitterPanel.ReplaceControl(control3, control2);
            }
        }

        private void SplitHorizontalButton_Click(object? sender, EventArgs e)
        {
            if (splitterPanel.IsSplit)
            {
                splitterPanel.IsSplitHorizontal = true;
                return;
            }
            splitterPanel.SplitHorizontal(control1, control2);
        }

        private void SplitVerticalButton_Click(object? sender, EventArgs e)
        {
            if (splitterPanel.IsSplit)
            {
                splitterPanel.IsSplitVertical = true;
                return;
            }
            splitterPanel.SplitVertical(control1, control2);
        }

        private void UnsplitButton_Click(object? sender, EventArgs e)
        {
            splitterPanel.DoUnsplit();
        }

        private void SashVisibleButton_Click(object? sender, EventArgs e)
        {
            splitterPanel.SashVisible = !splitterPanel.SashVisible;
        }

        private void SetMinPaneSizeButton_Click(object? sender, EventArgs e)
        {
            if(splitterPanel.MinimumPaneSize == 150)
                splitterPanel.MinimumPaneSize = 0;
            else
                splitterPanel.MinimumPaneSize = 150;
        }

        private void SetSashPositionButton_Click(object? sender, EventArgs e)
        {
            if(splitterPanel.SashPosition == 200)
                splitterPanel.SashPosition = 400;
            else
                splitterPanel.SashPosition = 200;
        }

        private void SashGravity00Button_Click(object? sender, EventArgs e)
        {
            splitterPanel.SashGravity = 0.0;
        }

        private void SashGravity05Button_Click(object? sender, EventArgs e)
        {
            splitterPanel.SashGravity = 0.5;
        }

        private void SashGravity10Button_Click(object? sender, EventArgs e)
        {
            splitterPanel.SashGravity = 1.0;
        }
    }
}