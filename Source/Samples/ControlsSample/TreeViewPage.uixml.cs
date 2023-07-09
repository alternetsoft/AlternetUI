using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class TreeViewPage : Control
    {
        private IPageSite? site;
        private int supressExpandEvents = 0;
        private bool? slowSettingsEnabled;

        public TreeViewPage()
        {
            InitializeComponent();
            
            treeView.Items.ItemInserted += Items_ItemInserted;
            treeView.Items.ItemRemoved += Items_ItemRemoved;
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                treeView.ImageList = ResourceLoader.LoadImageLists().Small;
                AddDefaultItems();
                site = value;
            }
        }

        private void AddDefaultItems()
        {
            AddItems(10);
        }

        private static TreeViewItem? GetLastItem(TreeViewItem? parent, ICollection<TreeViewItem> children)
        {
            if (!children.Any())
                return parent;

            var child = children.Last();
            return GetLastItem(child, child.Items);
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            AddItems(1000);
        }

        private void AddItems(int count)
        {
            int start = treeView.Items.Count + 1;

            treeView.BeginUpdate();
            try
            {
                for (int i = start; i < start + count; i++)
                {
                    int imageIndex = i % 4;
                    var item = new TreeViewItem("Item " + i, imageIndex);
                    for (int j = 0; j < 3; j++)
                    {
                        var childItem = new TreeViewItem(item.Text + "." + j, imageIndex);
                        item.Items.Add(childItem);

                        if (i < 5)
                        {
                            for (int k = 0; k < 2; k++)
                            {
                                childItem.Items.Add(new TreeViewItem(item.Text + "." + k, imageIndex));
                            }
                        }
                    }

                    treeView.Items.Add(item);
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
            string selectedItemsString = selectedItems.Count > 100 ? "too many indices to display" : string.Join(",", selectedItems.Select(x => x.Text));
            site?.LogEvent($"TreeView: SelectionChanged. SelectedItems: ({selectedItemsString})");
        }

        private void TreeView_ExpandedChanged(object? sender, TreeViewItemExpandedChangedEventArgs e)
        {
            if (supressExpandEvents > 0)
                return;
            site?.LogEvent($"TreeView: ExpandedChanged. Item: '{e.Item.Text}', IsExpanded: {e.Item.IsExpanded}");
        }

        private void TreeView_BeforeLabelEdit(object? sender, TreeViewItemLabelEditEventArgs e)
        {
            e.Cancel = cancelBeforeLabelEditEventsCheckBox.IsChecked;
            site?.LogEvent($"TreeView: BeforeLabelEdit. Item: '{e.Item.Text}', Label: '{e.Label ?? "<null>"}'");
        }

        private void TreeView_AfterLabelEdit(object? sender, TreeViewItemLabelEditEventArgs e)
        {
            e.Cancel = cancelAfterLabelEditEventsCheckBox.IsChecked;
            site?.LogEvent($"TreeView: AfterLabelEdit. Item: '{e.Item.Text}', Label: '{e.Label ?? "<null>"}'");
        }

        private void TreeView_BeforeExpand(object? sender, TreeViewItemExpandedChangingEventArgs e)
        {
            if (supressExpandEvents > 0)
                return;
            e.Cancel = cancelBeforeExpandEventsCheckBox.IsChecked;
            site?.LogEvent($"TreeView: BeforeExpand. Item: '{e.Item.Text}'");
        }

        private void TreeView_BeforeCollapse(object? sender, TreeViewItemExpandedChangingEventArgs e)
        {
            if (supressExpandEvents > 0)
                return;
            e.Cancel = cancelBeforeCollapseEventsCheckBox.IsChecked;
            site?.LogEvent($"TreeView: BeforeCollapse. Item: '{e.Item.Text}'");
        }

        private void CancelBeforeExpandEventsCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (treeView != null)
                treeView.Enabled = enabledCheckBox.IsChecked;
        }

        private void EnabledCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (treeView != null)
                treeView.Enabled = enabledCheckBox.IsChecked;
        }

        private void ShowRootLinesCheckBox_CheckedChanged(object? sender, EventArgs e)
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
            treeView.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ? TreeViewSelectionMode.Multiple : TreeViewSelectionMode.Single;
        }

        private void AllowLabelEditingCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (treeView != null)
                treeView.AllowLabelEdit = allowLabelEditingCheckBox.IsChecked;

            beginSelectedLabelEditingButton.Enabled = allowLabelEditingCheckBox.IsChecked;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            treeView.BeginUpdate();
            IReadOnlyList<TreeViewItem> items = treeView.SelectedItems;
            treeView.ClearSelected();
            foreach (var item in items)
                item.Remove();
            treeView.EndUpdate();
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            treeView.Items.Add(new TreeViewItem("Item " + (treeView.Items.Count + 1), 0));
        }

        private void BeginSelectedLabelEditingButton_Click(object sender, EventArgs e)
        {
            treeView.SelectedItem?.BeginLabelEdit();
        }

        private void ExpandAllButton_Click(object sender, EventArgs e)
        {
            supressExpandEvents++;
            treeView.ExpandAll();
            supressExpandEvents--;
        }

        private void CollapseAllButton_Click(object sender, EventArgs e)
        {
            supressExpandEvents++;
            treeView.CollapseAll();
            supressExpandEvents--;
        }

        private void ExpandAllChildrenButton_Click(object sender, EventArgs e) => treeView.SelectedItem?.ExpandAll();

        private void CollapseAllChildrenButton_Click(object sender, EventArgs e) => treeView.SelectedItem?.CollapseAll();

        private void EnsureLastItemVisibleButton_Click(object sender, System.EventArgs e) => GetLastItem(null, treeView.Items)?.EnsureVisible();

        private void ScrollLastItemIntoViewButton_Click(object sender, System.EventArgs e) => GetLastItem(null, treeView.Items)?.ScrollIntoView();

        private void FocusLastItemButton_Click(object sender, System.EventArgs e)
        {
            var item = GetLastItem(null, treeView.Items);
            if (item != null)
            {
                treeView.SetFocus();
                item.IsFocused = true;
            }
        }

        private void TreeView_MouseLeftButtonDown(object sender, Alternet.UI.MouseButtonEventArgs e)
        {
            var result = treeView.HitTest(e.GetPosition(treeView));
            site?.LogEvent($"HitTest result: Item: '{result.Item?.Text ?? "<none>"}, Location: {result.Location}'");
        }

        private void ModifyLastItemButton_Click(object sender, System.EventArgs e)
        {
            var item = GetLastItem(null, treeView.Items);
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

        private void ClearItemsButton_Click(object sender, System.EventArgs e)
        {
            treeView.BeginUpdate();
            treeView.Items.Clear();
            treeView.EndUpdate();
        }

        private void AddLastItemSiblingButton_Click(object sender, System.EventArgs e)
        {
            var item = GetLastItem(null, treeView.Items);
            if (item != null)
            {
                var collection = item.Parent == null ? treeView.Items : item.Parent.Items;
                var newItem = new TreeViewItem(item.Text + " Sibling", item.ImageIndex ?? 0);
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
        }

        private void Items_ItemRemoved(object sender, Alternet.Base.Collections.CollectionChangeEventArgs<TreeViewItem> e)
        {
            UpdateSlowRecreate();
        }

        private void Items_ItemInserted(object sender, Alternet.Base.Collections.CollectionChangeEventArgs<TreeViewItem> e)
        {
            UpdateSlowRecreate();
        }
    }
}