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
        private readonly CardPanelHeader panelHeader = new();
        private readonly ListBox? control1 = new();
        private readonly ListBox? control2 = new();
        private readonly ListBox? control3 = new();
        private readonly ListBox? control4 = new();
        private readonly SplitterPanel splitterPanel2;
        private bool firstTimeCalled;
        private string info1 = string.Empty;
        private string info2 = string.Empty;
        private bool info1Changed;
        private bool info2Changed;

        static SplitterPanelPage()
        {
        }

        public SplitterPanelPage()
        {
            InitializeComponent();

            panelHeader.Add("Actions", pageActions);
            panelHeader.Add("Settings", pageProps);
            pageControl.Children.Prepend(panelHeader);
            panelHeader.SelectFirstTab();

            Group(control1, control2, control3, control4).Margin(5);
            control4.Visible = false;

            splitterPanel2 = new()
            {
                CanDoubleClick = false,
                CanMoveSplitter = true,
                MinPaneSize = 20,
            };

            AppUtils.RepeatAction(3, ()=>control1.Items.Add("Control 1"));
            AppUtils.RepeatAction(3, ()=>control2.Items.Add("Control 2"));
            AppUtils.RepeatAction(3, ()=>control3.Items.Add("Control 3"));
            AppUtils.RepeatAction(3, ()=>control4.Items.Add("Control 4"));

            Group(control1, splitterPanel2).Parent(splitterPanel);
            Group(control2, control3, control4).Parent(splitterPanel2);

            splitterPanel.SplitterDoubleClick += SplitterPanel_SplitterDoubleClick;
            splitterPanel.SplitterMoved += SplitterPanel_SplitterMoved;
            splitterPanel.Unsplit += SplitterPanel_Unsplit;
            splitterPanel.SplitterMoving += SplitterPanel_SplitterMoving;
            splitterPanel.SplitterResize += SplitterPanel_SplitterResize;

            splitterPanel2.SplitterMoving += SplitterPanel2_SplitterMoving;
            splitterPanel2.SplitterDoubleClick += SplitterPanel_SplitterDoubleClick;
            splitterPanel2.SplitterMoved += SplitterPanel_SplitterMoved;
            splitterPanel2.Unsplit += SplitterPanel_Unsplit;
            splitterPanel2.SplitterResize += SplitterPanel_SplitterResize;

            splitterPanel.SplitVertical(control1, splitterPanel2);
            splitterPanel2.SplitHorizontal(control2, control3);

            Application.Current.Idle += Current_Idle;

            LogMovingCheckbox.CheckedChanged += LogMovingCheckbox_CheckedChanged;
        }

        private void LogMovingCheckbox_CheckedChanged(object? sender, EventArgs e)
        {
            if (!firstTimeCalled)
            {
                firstTimeCalled = true;
                Application.LogNameValue("DefaultNativeSashSize", splitterPanel.DefaultSashSize);
            }

            var log = LogMovingCheckbox.IsChecked;
            label1.Visible = log;
            label2.Visible = log;
        }

        private void Current_Idle(object? sender, EventArgs e)
        {
            if (!LogMovingCheckbox.IsChecked)
                return;
            if(info1Changed)
            {
                label1.Text = info1;
                info1Changed = false;
            }

            if (info2Changed)
            {
                label2.Text = info2;
                info2Changed = false;
            }
        }

        private void LogEvent(string s, string? prefix = null, bool once = true)
        {
            if (!LogMovingCheckbox.IsChecked)
                return;

            prefix ??= s;

            if(once)
                Application.LogReplace(s, prefix);
            else
                Application.Log(s);
        }

        private void SplitterPanel_SplitterResize(
            object? sender,
            SplitterPanelEventArgs e)
        {
            var index = sender == splitterPanel ? 1 : 2;
            var s = $"Splitter Panel {index}: Splitter Resize";
            LogEvent(s);
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
            if (e.SashPosition > splitterPanel.MaxSashPosition - 100)
            {
                e.SashPosition = splitterPanel.MaxSashPosition - 100;
                e.Cancel = true;
            }

            var s2 = $"S1.SashPos: {e.SashPosition}/{splitterPanel.MaxSashPosition}/"+Environment.NewLine+
                $"S1.Width: {splitterPanel.ClientSize.Width}" + Environment.NewLine +
                $"S1.Height: {splitterPanel.ClientSize.Height}";
            info1 = s2;
            info1Changed = true;
        }

        private void SplitterPanel2_SplitterMoving(
            object? sender,
            SplitterPanelEventArgs e)
        {
            if (e.SashPosition < 20)
            {
                e.SashPosition = 20;
                e.Cancel = true;
            }

            var s2 = $"S2.SashPos: {e.SashPosition}/{splitterPanel2.MaxSashPosition}/"+Environment.NewLine+
                $"S2.Width: {splitterPanel2.ClientSize.Width}" + Environment.NewLine +
                $"S2.Height: {splitterPanel2.ClientSize.Height}";
            info2 = s2;
            info2Changed = true;
        }

        private void SplitterPanel_Unsplit(
            object? sender,
            SplitterPanelEventArgs e)
        {
            var index = sender == splitterPanel ? 1 : 2;
            LogEvent($"Splitter Panel {index}: Unsplit");
        }

        private void SplitterPanel_SplitterMoved(
            object? sender,
            SplitterPanelEventArgs e)
        {
            var index = sender == splitterPanel ? 1 : 2;
            LogEvent($"Splitter Panel {index}: Splitter Moved");
        }

        private void SplitterPanel_SplitterDoubleClick(
            object? sender,
            SplitterPanelEventArgs e)
        {
            e.Cancel = true;
            var index = sender == splitterPanel ? 1 : 2;
            LogEvent($"Splitter Panel {index}: Double click. X: {e.X}, Y: {e.Y}");
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
            if(splitterPanel.MinPaneSize == 150)
                splitterPanel.MinPaneSize = 0;
            else
                splitterPanel.MinPaneSize = 150;
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