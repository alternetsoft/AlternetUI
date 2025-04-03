using System;
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
            Title = "VirtualListBox as TreeView Sample";
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

            ListControlUtils.AddTestItems(treeView.RootItem, 10, ItemInitialize);

            void ItemInitialize(TreeControlItem item)
            {
            }

            treeView.ContextMenu.Add("Add", AddNewItem);
            treeView.ContextMenu.Add("Add child", AddNewChildItem);
            
            treeView.ContextMenu.Add("Add many items", () =>
            {
                ListControlUtils.AddTestItems(treeView.RootItem, 1000);
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
                    = ListControlItem.GetCheckBoxSize(treeView).Width + incValue;
                }
                else
                {
                    ListControlItem.CheckBoxSizeOverride
                    = ListControlItem.CheckBoxSizeOverride.Value + incValue;
                }

                Graphics.DebugElementId = treeView.RootItem.FirstChild?.UniqueId;
                treeView.Invalidate();
            });
            treeView.ContextMenu.Add("Toggle enabled", () =>
            {
                treeView.Enabled = !treeView.Enabled;
            });
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
            App.IdleLog($"{GetType()}{counter} Closed");
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            App.IdleLog($"{GetType()}{counter} Disposed");
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            base.OnClosing(e);
            App.IdleLog($"{GetType()}{counter} Closing");
        }
    }
}
