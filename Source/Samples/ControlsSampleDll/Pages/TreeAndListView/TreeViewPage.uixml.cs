﻿using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class TreeViewPage : Control
    {
        private readonly PopupTreeView popupTreeView = new();
        private int supressExpandEvents = 0;
        private bool? slowSettingsEnabled;
        private int newItemIndex = 0;

        public TreeViewPage()
        {
            InitializeComponent();

            DoInsideLayout(Init);

            void Init()
            {
                Group(
                    ensureLastItemVisibleButton,
                    scrollLastItemIntoViewButton,
                    focusLastItemButton,
                    modifyLastItemButton).SuggestedWidthToMax();

                treeView.Items.ItemInserted += Items_ItemInserted;
                treeView.Items.ItemRemoved += Items_ItemRemoved;
                treeView.MouseUp += TreeView_MouseUp;
                treeView.MouseLeftButtonUp += TreeView_MouseLeftButtonUp;

                treeView.ImageList = DemoResourceLoader.LoadImageLists().Small;
                AddDefaultItems();
                SetCustomColors();

                popupTreeView.MainControl.ImageList = DemoResourceLoader.LoadImageLists().Small;
                AddItems(popupTreeView.MainControl, 10);
                popupTreeView.AfterHide += PopupTreeView_AfterHide;
            }
        }

        private void PopupTreeView_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupTreeView.MainControl.SelectedItem?.Text ?? "<null>";
            App.Log($"AfterHide PopupResult: {popupTreeView.PopupResult}, Item: {resultItem}");
        }

        private void ShowPopupButton_Click(object? sender, EventArgs e)
        {
            popupTreeView.ShowPopup(showPopupButton);
        }

        private void TreeView_MouseLeftButtonUp(object? sender, MouseEventArgs e)
        {
            /*site?.LogEvent($"TreeView: MouseLeftButtonUp");*/
        }

        private void TreeView_MouseUp(object? sender, MouseEventArgs e)
        {
            /*site?.LogEvent($"TreeView: MouseUp");*/
        }

        internal void SetCustomColors()
        {
            var item = treeView.Items[2];
            item.Text = "Bold item";
            item.IsBold = true;

            item = treeView.Items[4];
            item.TextColor = Color.White;
            item.BackgroundColor = Color.Gray;
            item.Text = "Item with custom colors";
        }

        private void AddDefaultItems()
        {
            AddItems(treeView, 10);
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            AddItems(treeView, 1000);
        }

        private int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        private void AddItems(TreeView tree, int count)
        {
            treeView.BeginUpdate();
            try
            {
                for (int i = 0; i < count; i++)
                {
                    int imageIndex = i % 4;
                    var item = new TreeViewItem(
                        "Item " + GenItemIndex(),
                        imageIndex);
                    for (int j = 0; j < 3; j++)
                    {
                        var childItem = new TreeViewItem(
                            item.Text + "." + j,
                            imageIndex);
                        item.Items.Add(childItem);

                        if (i < 5)
                        {
                            for (int k = 0; k < 2; k++)
                            {
                                childItem.Items.Add(
                                    new TreeViewItem(
                                        item.Text + "." + k,
                                        imageIndex));
                            }
                        }
                    }

                    tree.Items.Add(item);
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        private void TreeView_SelectionChanged(object? sender, EventArgs e)
        {
            var selectedItems = treeView.SelectedItems;
            string s = selectedItems.Count > 100 ?
                "too many indices to display" :
                string.Join(",", selectedItems.Select(x => x.Text));
            App.Log($"TreeView: SelectionChanged. SelectedItems: ({s})");
        }

        private void TreeView_ExpandedChanged(object? sender, TreeViewEventArgs e)
        {
            if (supressExpandEvents > 0)
                return;
            var exp = e.Item.IsExpanded;
            App.Log($"TreeView: ExpandedChanged. Item: '{e.Item.Text}', IsExpanded: {exp}");
        }

        private void TreeView_BeforeLabelEdit(object? sender, TreeViewEditEventArgs e)
        {
            e.Cancel = cancelBeforeLabelEditEventsCheckBox.IsChecked;
            var s = e.Label ?? "<null>";
            App.Log($"TreeView: BeforeLabelEdit. Item: '{e.Item.Text}', Label: '{s}'");
        }

        private void TreeView_AfterLabelEdit(
            object? sender,
            TreeViewEditEventArgs e)
        {
            e.Cancel = cancelAfterLabelEditEventsCheckBox.IsChecked;
            var s = e.Label ?? "<null>";
            App.Log($"TreeView: AfterLabelEdit. Item: '{e.Item.Text}', Label: '{s}'");
        }

        private void TreeView_BeforeExpand(
            object? sender,
            TreeViewCancelEventArgs e)
        {
            if (supressExpandEvents > 0)
                return;
            e.Cancel = cancelBeforeExpandEventsCheckBox.IsChecked;
            App.Log($"TreeView: BeforeExpand. Item: '{e.Item.Text}'");
        }

        private void TreeView_BeforeCollapse(
            object? sender,
            TreeViewCancelEventArgs e)
        {
            if (supressExpandEvents > 0)
                return;
            e.Cancel = cancelBeforeCollapseEventsCheckBox.IsChecked;
            App.Log($"TreeView: BeforeCollapse. Item: '{e.Item.Text}'");
        }

        private void CancelBeforeExpandEventsCheckBox_CheckedChanged(
            object? sender,
            EventArgs e)
        {
            if (treeView != null)
                treeView.Enabled = enabledCheckBox.IsChecked;
        }

        private void EnabledCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (treeView != null)
                treeView.Enabled = enabledCheckBox.IsChecked;
        }

        private void ShowRootLinesCheckBox_CheckedChanged(
            object? sender,
            EventArgs e)
        {
            if (treeView != null)
                treeView.ShowRootLines = showRootLinesCheckBox.IsChecked;
        }

        private void ShowLinesCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (treeView != null)
                treeView.ShowLines = showLinesCheckBox.IsChecked;
        }

        private void ShowExpandButtons_CheckedChanged(object? sender, EventArgs e)
        {
            if (treeView != null)
                treeView.ShowExpandButtons = showExpandButtonsCheckBox.IsChecked;
        }

        private void FullRowSelectCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (treeView != null)
                treeView.FullRowSelect = fullRowSelectCheckBox.IsChecked;
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            treeView.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ?
                TreeViewSelectionMode.Multiple : TreeViewSelectionMode.Single;
        }

        private void AllowLabelEditingCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (treeView != null)
                treeView.AllowLabelEdit = allowLabelEditingCheckBox.IsChecked;

            beginSelectedLabelEditingButton.Enabled = allowLabelEditingCheckBox.IsChecked;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            treeView.RemoveSelected();
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            treeView.Items.Add(new TreeViewItem("Item " + GenItemIndex(), 0));
        }

        private void BeginSelectedLabelEditingButton_Click(object? sender, EventArgs e)
        {
            treeView.SelectedItem?.BeginLabelEdit();
        }

        private void ExpandAllButton_Click(object? sender, EventArgs e)
        {
            supressExpandEvents++;
            treeView.ExpandAll();
            supressExpandEvents--;
        }

        private void CollapseAllButton_Click(object? sender, EventArgs e)
        {
            supressExpandEvents++;
            treeView.CollapseAll();
            supressExpandEvents--;
        }

        private void ExpandAllChildrenButton_Click(object? sender, EventArgs e) =>
            treeView.SelectedItem?.ExpandAll();

        private void CollapseAllChildrenButton_Click(object? sender, EventArgs e) =>
            treeView.SelectedItem?.CollapseAll();

        private void EnsureLastItemVisibleButton_Click(object? sender, System.EventArgs e)
            => treeView.LastItem?.EnsureVisible();

        private void ScrollLastItemIntoViewButton_Click(object? sender, System.EventArgs e)
            => treeView.LastItem?.ScrollIntoView();

        private void FocusLastItemButton_Click(object? sender, System.EventArgs e)
        {
            treeView.SelectAndShowItem(treeView.LastItem);
        }

        private void TreeView_MouseLeftButtonDown(object? sender, MouseEventArgs e)
        {
            var result = treeView.HitTest(Mouse.GetPosition(treeView));
            var s = result.Item?.Text ?? "<none>";
            App.Log($"HitTest result: Item: '{s}, Location: {result.Location}'");
        }

        private void ModifyLastItemButton_Click(object? sender, System.EventArgs e)
        {
            var item = treeView.LastItem;
            if (item != null)
            {
                item.EnsureVisible();
                item.Text += "X";

                var imageIndex = item.ImageIndex + 1;
                if (imageIndex >= treeView.ImageList!.Images.Count)
                    imageIndex = 0;

                item.ImageIndex = imageIndex;
            }
        }

        private void ClearItemsButton_Click(object? sender, System.EventArgs e)
        {
            treeView.RemoveAll();
        }

        private void AddLastItemSiblingButton_Click(
            object? sender,
            EventArgs e)
        {
            var item = treeView.LastItem;
            if (item != null)
            {
                var collection = item.Parent == null ?
                    treeView.Items : item.Parent.Items;
                var newItem =
                    new TreeViewItem(item.Text + " Sibling", item.ImageIndex ?? 0);
                collection.Insert(collection.IndexOf(item), newItem);
                newItem.EnsureVisible();
            }
        }

        private bool SlowRecreate
        {
            get
            {
                bool result = treeView.Items.Count > 1000;
                return result;
            }
        }

        private void UpdateSlowRecreate()
        {
            var fastRecreate = !SlowRecreate;

            if (slowSettingsEnabled != null && fastRecreate == (bool)slowSettingsEnabled)
                return;

            slowSettingsEnabled = fastRecreate;

            showRootLinesCheckBox.Enabled = fastRecreate;
            showLinesCheckBox.Enabled = fastRecreate;
            showExpandButtonsCheckBox.Enabled = fastRecreate;
            fullRowSelectCheckBox.Enabled = fastRecreate;
            allowMultipleSelectionCheckBox.Enabled = fastRecreate;
            allowLabelEditingCheckBox.Enabled = fastRecreate;
            hasBorderButton.Enabled = fastRecreate;
        }

        private void EditorButton_Click(object? sender, System.EventArgs e)
        {
            DialogFactory.EditItemsWithListEditor(treeView);
        }

        private void HasBorderButton_Click(object? sender, System.EventArgs e)
        {
            treeView.HasBorder = !treeView.HasBorder;
        }

        private void Items_ItemRemoved(object? sender, int index, TreeViewItem item)
        {
            UpdateSlowRecreate();
        }

        private void Items_ItemInserted(object? sender, int index, TreeViewItem item)
        {
            UpdateSlowRecreate();
        }
    }
}