using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;
using System.ComponentModel;

namespace ControlsSample
{
    internal partial class SplitterPanelPage : Control
    {
        private IPageSite? site;
        private readonly ListBox? control1;
        private readonly ListBox? control2;
        private readonly ListBox? control3;
        private readonly ListBox? control4;
        private readonly SplitterPanel splitterPanel2;

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
            };

            control4 = new()
            {
                Margin = 5,
                Visible = false,
            };

            splitterPanel2 = new()
            {
                CanDoubleClick = false,
                CanMoveSplitter = true
            };

            control1.Items.Add("Control 1");
            control2.Items.Add("Control 2");
            control3.Items.Add("Control 3");
            control4.Items.Add("Control 4");

            splitterPanel.Children.Add(control1);
            splitterPanel.Children.Add(splitterPanel2);

            splitterPanel2.Children.Add(control2);
            splitterPanel2.Children.Add(control3);
            splitterPanel2.Children.Add(control4);

            splitterPanel.SplitterDoubleClick += SplitterPanel_SplitterDoubleClick;
            splitterPanel.SplitterMoved += SplitterPanel_SplitterMoved;
            splitterPanel.Unsplit += SplitterPanel_Unsplit;
            splitterPanel.SplitterMoving += SplitterPanel_SplitterMoving;
            splitterPanel.SplitterResize += SplitterPanel_SplitterResize;

            splitterPanel.SplitVertical(control1, splitterPanel2);
            splitterPanel2.SplitHorizontal(control2, control3);
        }

        private void LogEventOnce(string s)
        {
            if (site == null)
                return;

            if (site.LastEventMessage == s)
                return;

            site?.LogEvent(s);
        }

        private void SplitterPanel_SplitterResize(object? sender, CancelEventArgs e)
        {
            LogEventOnce("Splitter Panel: Splitter Resize");
        }

        private void SplitterPanel_SplitterMoving(
            object? sender, 
            SplitterPanelEventArgs e)
        {
            if (e.SashPosition < 20)
            {
                e.SashPosition = 20;
                e.Cancel = true;
            }

            if (LogMovingOnceCheckbox.IsChecked)
            {
                LogEventOnce("Splitter Panel: Splitter Moving");
                return;
            }
            var s = $"Sash Pos: {e.SashPosition}";
            LogEventOnce("Splitter Panel: Splitter Moving. "+s);
        }

        private void SplitterPanel_Unsplit(object? sender, CancelEventArgs e)
        {
            site?.LogEvent("Splitter Panel: Unsplit");
        }

        private void SplitterPanel_SplitterMoved(object? sender, EventArgs e)
        {
            site?.LogEvent("Splitter Panel: Splitter Moved");
        }

        private void SplitterPanel_SplitterDoubleClick(
            object? sender, 
            CancelEventArgs e)
        {
            e.Cancel = true;
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
            if (!splitterPanel2.IsSplit)
                return;
            if (splitterPanel2.Control2 == control3)
            {
                splitterPanel2.ReplaceControl(control3, control4);
            }
            else
            {
                splitterPanel2.ReplaceControl(control4, control3);
            }
        }

        private void SplitHorizontalButton_Click(object? sender, EventArgs e)
        {
            splitterPanel.SashPosition = 150;
            if (splitterPanel.IsSplit)
            {
                splitterPanel.IsSplitHorizontal = true;
                return;
            }
            splitterPanel.SplitHorizontal(control1, splitterPanel2);
        }

        private void SplitVerticalButton_Click(object? sender, EventArgs e)
        {
            if (splitterPanel.IsSplit)
            {
                splitterPanel.IsSplitVertical = true;
                return;
            }
            splitterPanel.SplitVertical(control1, splitterPanel2);
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