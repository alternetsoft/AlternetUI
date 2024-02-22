using System;
using System.Linq;
using System.Reflection;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class TabControlPage : Control
    {
        private const string ResPrefixSmall = "embres:ControlsSample.Resources.ToolBarPng.Small.";
        private const string ResPrefixLarge = "embres:ControlsSample.Resources.ToolBarPng.Large.";

        private readonly TabControl tabControl = new()
        {
            Margin = 5,
        };

        private int newItemIndex;

        public TabControlPage()
        {
            DoInsideLayout(Fn);

            void Fn()
            {
                tabControl.Parent = this;
                tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
                InitializeComponent();

                InsertPage();
                InsertPage();
                InsertPage();

                tabAlignmentComboBox.AddEnumValues(TabAlignment.Top);
                tabAlignmentComboBox.SelectedItemChanged +=
                    TabAlignmentComboBox_SelectedItemChanged;

                ImageSet image;
                ImageSet svgImage;

                if(GetDPI().Width <= 96)
                {
                    image = ImageSet.FromUrl($"{ResPrefixSmall}Calendar16.png");
                    svgImage = KnownSvgImages.GetForSize(GetSvgColor(), 16).ImgGear;
                }
                else
                {
                    image = ImageSet.FromUrl($"{ResPrefixLarge}Calendar32.png");
                    svgImage = KnownSvgImages.GetForSize(GetSvgColor(), 32).ImgCircle;
                }

                tabControl.SetTabImage(0, image);
                tabControl.SetTabImage(2, svgImage);
            }
        }

        private void TabControl_SelectedIndexChanged(object? sender, EventArgs e)
        {
            Application.Log("TabControl SelectedIndexChanged");
        }

        private int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        private void ModifyPageTitleButton_Click(object? sender, EventArgs e)
        {
            var index = tabControl.SelectedIndex;
            tabControl.SetTitle(index, tabControl.GetTitle(index) + "X");
        }

        private void InsertLastPageSiblingButton_Click(object? sender, EventArgs e)
        {
            if (tabControl.TabCount == 0)
            {
                InsertPage();
                return;
            }

            InsertPage(tabControl.TabCount - 1);
        }

        private void RemoveSelectedPageButton_Click(object? sender, EventArgs e)
        {
            tabControl.RemoveAt(tabControl.SelectedIndex);
        }

        private void AppendPageButton_Click(object? sender, EventArgs e)
        {
            InsertPage();
        }

        private void InsertPage(int? index = null)
        {
            var s = "Page " + GenItemIndex();
            Control page = new() 
            {
                Padding = 5,
            };

            VerticalStackPanel panel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Padding = 5,
            };

            page.Children.Add(panel);

            for (int i=1; i < 4; i++)
            {
                var button = new Button()
                {
                    Text = s + " Button " + i.ToString(),
                    Margin = 5,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
                panel.Children.Add(button);
            }

            page.Disposed += Page_Disposed;

            if(index == null)
                tabControl.Add(s, page);
            else
                tabControl.Insert(index.Value, s, page);
        }

        private void Page_Disposed(object? sender, EventArgs e)
        {
            Application.Log("Page Disposed");
        }

        private void ClearPagesButton_Click(object? sender, EventArgs e)
        {
            tabControl.RemoveAll();
        }

        private void TabAlignmentComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if(tabAlignmentComboBox.SelectedItem is TabAlignment tabAlignment)
                tabControl.TabAlignment = tabAlignment;
        }
    }
}