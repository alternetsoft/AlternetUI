﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alternet.UI;

namespace ControlsSample
{
    internal class ListBoxAsTreeWindow : Window
    {
        internal bool IsDebugInfoLogged = false;

        private static int globalCounter;

        private readonly int counter;
        private readonly AbstractControl? statusPanel;

        private readonly VirtualTreeControl treeView = new()
        {
        };

        private readonly ToolBar statusBar = new()
        {
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        public ListBoxAsTreeWindow()
        {
            counter = ++globalCounter;
            Size = (800, 600);
            Title = "VirtualTreeControl Sample";
            StartLocation = WindowStartLocation.ScreenTopRight;
            Layout = LayoutStyle.Vertical;

            treeView.Margin = 10;
            treeView.VerticalAlignment = VerticalAlignment.Fill;
            treeView.Parent = this;
            
            statusBar.Parent = this;

            statusBar.SetBorderAndMargin(AnchorStyles.Top);

            statusBar.MinHeight = 24;
            statusPanel = new Label("Ready");
            statusBar.AddControl(statusPanel);

            ActiveControl = treeView;

            ListControlUtils.AddTestItems(treeView.RootItem, 10);

            treeView.ContextMenu.Add("Add", AddNewItem);
            treeView.ContextMenu.Add("Add child", AddNewChildItem);
            
            treeView.ContextMenu.Add("Add many items", () =>
            {
                ListControlUtils.AddTestItems(treeView.RootItem, 1000);
                treeView.SelectLastItemAndScroll();
            });

            treeView.ContextMenu.AddSeparator();
            treeView.ContextMenu.Add("Remove", treeView.RemoveSelectedItem);
            treeView.ContextMenu.Add("Clear", treeView.Clear);
            treeView.ContextMenu.Add("Rename", RenameSelectedItem);
            treeView.ContextMenu.AddSeparator();
            treeView.ContextMenu.Add("Change tree buttons", treeView.SelectNextTreeButton);

            treeView.ContextMenu.Add("Inc checkbox size", () =>
            {
                int incValue = 4;

                if(ListControlItem.CheckBoxSizeOverride is null)
                {
                    ListControlItem.CheckBoxSizeOverride
                    = ListControlItem.GetCheckBoxSize(treeView.ListBox).Width + incValue;
                }
                else
                {
                    ListControlItem.CheckBoxSizeOverride
                    = ListControlItem.CheckBoxSizeOverride.Value + incValue;
                }

#if DEBUG
                Graphics.DebugElementId = treeView.RootItem.FirstChild?.UniqueId;
#endif
                FormUtils.InvalidateAll();
            });

            treeView.ContextMenu.Add("Toggle enabled", () =>
            {
                treeView.Enabled = !treeView.Enabled;
            });

            treeView.ContextMenu.Add("Change root item", () =>
            {
                TreeControlRootItem item = new();
                ListControlUtils.AddTestItems(item, 10, ItemInitialize);

                void ItemInitialize(TreeControlItem item)
                {
                    item.Text += "a";
                }

                treeView.RootItem = item;
            });

            var lastChild = treeView.RootItem.LastChild;
            var item = treeView.RootItem.GetItem(3);

            if (lastChild is not null)
            {
                lastChild.IsBold = true;
            }

            if (CommandLineArgs.ParseAndGetIsDark())
            {
                treeView.BackColor = DefaultColors.GetWindowBackColor(true);
                treeView.ForeColor = DefaultColors.GetWindowForeColor(true);
            }

            if (item is not null)
            {
                if (treeView.IsDarkBackground)
                {
                    item.ForegroundColor = DefaultColors.GetWindowForeColor(true);
                    item.BackgroundColor = DefaultColors.GetWindowBackColor(true).Lighter();
                }
                else
                {
                    item.ForegroundColor = Color.Black;
                    item.BackgroundColor = Color.Beige;
                }
            }
        }

        public void AddNewChildItem()
        {
            var item = new Alternet.UI.TreeControlItem();
            item.Text = "item " + Alternet.UI.LogUtils.GenNewId();
            item.SvgImage = Alternet.UI.KnownColorSvgImages.ImgLogo;
            treeView.AddChild(treeView.SelectedItem, item, true);
        }

        public void AddNewItem()
        {
            var item = new Alternet.UI.TreeControlItem();
            item.Text = "item " + Alternet.UI.LogUtils.GenNewId();
            item.SvgImage = Alternet.UI.KnownColorSvgImages.ImgLogo;
            treeView.Add(item, true);
        }

        public void RenameSelectedItem()
        {
            var item = treeView.SelectedItem;
            if (item is null)
                return;
            item.Text += "a";
            treeView.Invalidate();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            App.LogIf($"{GetType()}{counter} Closed", false);
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            App.LogIf($"{GetType()}{counter} Disposed", false);
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);
            App.LogIf($"{GetType()}{counter} Closing", false);
        }
    }
}
