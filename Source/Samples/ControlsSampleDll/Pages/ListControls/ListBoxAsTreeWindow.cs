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
        private TreeControlItem rootItem = new();

        private readonly VirtualListBox listBox = new()
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

            listBox.Margin = 10;
            listBox.VerticalAlignment = VerticalAlignment.Fill;
            listBox.Parent = this;
            listBox.SelectionUnderImage = false;
            listBox.CheckBoxVisible = true;
            listBox.CheckOnClick = false;
            listBox.CheckImageUnchecked = KnownSvgImages.ImgSquarePlus;
            listBox.CheckImageChecked = KnownSvgImages.ImgSquareMinus;
            
            statusBar.Parent = this;

            statusBar.SetBorderAndMargin(AnchorStyles.Top);

            statusBar.MinHeight = 24;
            statusPanel = new Label("Ready");
            statusBar.AddControl(statusPanel);

            ActiveControl = listBox;

            ListControlUtils.AddTestItems(rootItem, 10, ItemInitialize);

            void ItemInitialize(TreeControlItem item)
            {
            }

            listBox.SelectionUnderImage = true;

            void LoadItems()
            {
                var collection = rootItem.EnumExpandedItems().ToArray();

                const int indentPx = 16;

                foreach (var item in collection)
                {
                    var indentLevel = item.IndentLelel - 1;

                    item.ForegroundMargin = (indentPx * indentLevel, 0, 0, 0);
                    item.CheckBoxVisible = item.HasItems;
                    item.CheckState = item.IsExpanded ? CheckState.Checked : CheckState.Unchecked;
                    item.SvgImage = KnownColorSvgImages.ImgLogo;
                }

                VirtualListBoxItems items = new(collection);
                listBox.SetItemsFast(items, VirtualListBox.SetItemsKind.ChangeField);
            }

            LoadItems();

            listBox.MouseDown += (s, e) =>
            {
                var itemIndex = listBox.HitTestCheckBox(e.Location);
                if (itemIndex is null)
                    return;
                if (listBox.Items[itemIndex.Value] is not TreeControlItem item)
                    return;
                item.IsExpanded = !item.IsExpanded;
                LoadItems();
            };

            listBox.DoubleClick += (s, e) =>
            {
                if (listBox.SelectedItem is not TreeControlItem item)
                    return;
                item.IsExpanded = !item.IsExpanded;
                LoadItems();
            };
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
