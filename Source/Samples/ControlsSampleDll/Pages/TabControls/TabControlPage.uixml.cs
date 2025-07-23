using System;
using System.Linq;
using System.Reflection;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class TabControlPage : Panel
    {
        private const string ResPrefixSmall = "embres:ControlsSampleDll.Resources.ToolBarPng.Small.";
        private const string ResPrefixLarge = "embres:ControlsSampleDll.Resources.ToolBarPng.Large.";

        private readonly TabControl tabControl = new()
        {
            Margin = 5,
        };

        private int newItemIndex;

        static TabControlPage()
        {
        }

        public TabControlPage()
        {
            DoInsideLayout(Fn);

            void Fn()
            {
                tabControl.Parent = this;
                InitializeComponent();

                var page0 = InsertPage();
                var page1 = InsertPage();
                var page2 = InsertPage();

                tabAlignmentComboBox.EnumType = typeof(TabAlignment);
                tabAlignmentComboBox.Value = TabAlignment.Top;
                tabAlignmentComboBox.ValueChanged +=
                    TabAlignmentComboBox_SelectedItemChanged;

                ImageSet image;

                if(UseSmallImages)
                {
                    image = ImageSet.FromUrl($"{ResPrefixSmall}Calendar16.png");
                }
                else
                {
                    image = ImageSet.FromUrl($"{ResPrefixLarge}Calendar32.png");
                }

                tabControl.SetTabImage(0, image);
                tabControl.SetTabSvg(2, KnownSvgImages.ImgGear, null, LightDarkColors.Blue);

                tabControl.TabSizeChanged += TabControl_TabSizeChanged;
                tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

                var button = tabControl.GetHeaderButtonAt(1);
                if(button is not null)
                {
                    button.ContextMenuStrip.Add("Sample command", () => App.Log("Sample command clicked"));
                    var item = button.ContextMenuStrip.Add(
                        "Disabled command",
                        () => App.Log("Disabled command clicked"));
                    item.Enabled = false;
                }

                var button2 = tabControl.GetHeaderButton(page2);
                if (button2 is not null)
                {
                    button2.ContextMenuStrip.Add(
                        "Another sample command",
                        () => App.Log("Another sample command clicked"));
                    var item = button2.ContextMenuStrip.Add(
                        "Disabled command 2",
                        () => App.Log("Disabled command 2 clicked"));
                    item.Enabled = false;
                }

                tabControl.HeaderControl.ContextMenuStrip.Add("Toggle vertical text", () =>
                {
                    if(tabControl.IsVerticalText)
                    {
                        tabControl.ImageToText = ImageToText.Horizontal;
                        tabControl.IsVerticalText = false;
                    }
                    else
                    {
                        tabControl.ImageToText = ImageToText.Vertical;
                        tabControl.IsVerticalText = true;
                    }
                });

                tabControl.HasCloseButton = true;
                tabControl.CloseButtonClick += (s, e) =>
                {
                    App.Log("Close button clicked");
                };
            }
        }

        private void TabControl_TabSizeChanged(object? sender, BaseEventArgs<AbstractControl> e)
        {
            App.DebugLogIf($"TabSizeChanged: {e.Value.Text}, {PixelFromDip(e.Value.Size)}, {e.Value.GetDPI()}", false);
        }

        private void TabControl_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var selected = tabControl.SelectedPage as TabPage;
            if(selected is not null)
                App.Log($"TabControl TabPage.Index: {selected.Index}");
            else
                App.Log($"TabControl SelectedIndex: {tabControl.SelectedIndex}");
        }

        private int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        private void ModifyPageTitleButton_Click(object? sender, EventArgs e)
        {
            var control = tabControl.SelectedControl;
            control?.SetTitle(control.Title + "X");
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

        private TabPage InsertPage(int? index = null)
        {
            var s = "Page " + GenItemIndex();
            TabPage page = new() 
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

            page.Title = s;

            tabControl.Insert(index, page);

            return page;
        }

        private void Page_Disposed(object? sender, EventArgs e)
        {
            App.DebugLogIf("Page Disposed", false);
        }

        private void ClearPagesButton_Click(object? sender, EventArgs e)
        {
            tabControl.RemoveAll();
        }

        private void TabAlignmentComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if(tabAlignmentComboBox.Value is TabAlignment tabAlignment)
                tabControl.TabAlignment = tabAlignment;

            var preferredSize = tabAlignmentComboBox.GetPreferredSize();
            tabAlignmentComboBox.InvalidateBestSize();
            var preferredSize2 = tabAlignmentComboBox.GetPreferredSize();

            App.DebugLogIf(
                $"tabAlignmentComboBox.PreferredSize = {preferredSize} {preferredSize2}",
                true);
        }
    }
}