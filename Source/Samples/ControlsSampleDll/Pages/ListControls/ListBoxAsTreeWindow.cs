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

            treeView.ContextMenu.Add("Change tree buttons", treeView.SelectNextTreeButton);
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
