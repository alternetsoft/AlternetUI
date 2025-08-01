using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class TreeViewPage : Panel
    {
        private readonly PopupTreeView popupTreeView = new();
        private int suppressExpandEvents = 0;
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
                treeView.MouseMove += TreeView_MouseMove;

                var imageLists = DemoResourceLoader.LoadImageLists();

                if (App.SafeWindow.UseSmallImages)
                {
                    treeView.ImageList = imageLists.Small;
                    popupTreeView.MainControl.ImageList = imageLists.Small;
                }
                else
                {
                    treeView.ImageList = imageLists.Large;
                    popupTreeView.MainControl.ImageList = imageLists.Large;
                }

                AddDefaultItems();
                SetCustomColors();

                PropertyGridSample.ObjectInit.AddItems(popupTreeView.MainControl, 10);
                popupTreeView.AfterHide += PopupTreeView_AfterHide;
            }
        }

        private void TreeView_MouseMove(object sender, MouseEventArgs e)
        {
            var item = treeView.GetNodeAtMouseCursor();
            var s = item?.Text ?? "<none>";
            var prefix = "MouseMove. Item under mouse:";
            App.LogReplace($"{prefix} '{s}'", prefix);
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
        }

        private void TreeView_MouseUp(object? sender, MouseEventArgs e)
        {
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
            var prefix = "TreeView: SelectionChanged. SelectedItems:";
            App.LogReplace($"{prefix} ({s})", prefix);
        }

        private void TreeView_ExpandedChanged(object? sender, TreeViewEventArgs e)
        {
            if (suppressExpandEvents > 0)
                return;
            var exp = e.Item.IsExpanded;
            var prefix = "TreeView: ExpandedChanged. Item:";
            App.LogReplace($"{prefix} '{e.Item.Text}', IsExpanded: {exp}", prefix);
        }

        private void TreeView_BeforeLabelEdit(object? sender, TreeViewEditEventArgs e)
        {
            e.Cancel = cancelBeforeLabelEditEventsCheckBox.IsChecked;
            var s = e.Label ?? "<null>";
            var prefix = "TreeView: BeforeLabelEdit. Item:";
            App.LogReplace($"{prefix} '{e.Item.Text}', Label: '{s}'", prefix);
        }

        private void TreeView_AfterLabelEdit(
            object? sender,
            TreeViewEditEventArgs e)
        {
            e.Cancel = cancelAfterLabelEditEventsCheckBox.IsChecked;
            var s = e.Label ?? "<null>";
            var prefix = "TreeView: AfterLabelEdit. Item:";
            App.LogReplace($"{prefix} '{e.Item.Text}', Label: '{s}'", prefix);
        }

        private void TreeView_BeforeExpand(
            object? sender,
            TreeViewCancelEventArgs e)
        {
            if (suppressExpandEvents > 0)
                return;
            e.Cancel = cancelBeforeExpandEventsCheckBox.IsChecked;
            var prefix = "TreeView: BeforeExpand. Item:";
            App.LogReplace($"{prefix} '{e.Item.Text}'", prefix);
        }

        private void TreeView_BeforeCollapse(
            object? sender,
            TreeViewCancelEventArgs e)
        {
            if (suppressExpandEvents > 0)
                return;
            e.Cancel = cancelBeforeCollapseEventsCheckBox.IsChecked;
            var prefix = "TreeView: BeforeCollapse. Item:";
            App.LogReplace($"{prefix} '{e.Item.Text}'", prefix);
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
            suppressExpandEvents++;
            treeView.ExpandAll();
            suppressExpandEvents--;
        }

        private void CollapseAllButton_Click(object? sender, EventArgs e)
        {
            suppressExpandEvents++;
            treeView.CollapseAll();
            suppressExpandEvents--;
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
            var prefix = "HitTest result: Item:";
            App.LogReplace($"{prefix} '{s}', Location: {result.Location}", prefix);
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