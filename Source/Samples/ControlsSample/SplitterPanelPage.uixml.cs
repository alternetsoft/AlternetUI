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

            splitterPanel.SplitVertically(control1, control2, 200);
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        private void SplitHorizontalButton_Click(object? sender, EventArgs e)
        {
        }

        private void SplitVerticalButton_Click(object? sender, EventArgs e)
        {
        }

        private void UnsplitButton_Click(object? sender, EventArgs e)
        {
        }

        private void SashVisibleButton_Click(object? sender, EventArgs e)
        {
        }

        private void SetMinPaneSizeButton_Click(object? sender, EventArgs e)
        {
        }

        private void SetSashPositionButton_Click(object? sender, EventArgs e)
        {
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