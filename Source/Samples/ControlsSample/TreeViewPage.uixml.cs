using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class TreeViewPage : Control
    {
        private IPageSite? site;

        public TreeViewPage()
        {
            InitializeComponent();
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                treeView.ImageList = ResourceLoader.LoadImageLists().Small;
                AddItems(10);
                site = value;
            }
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            AddItems(5000);
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
            e.Cancel = cancelBeforeExpandEventsCheckBox.IsChecked;
            site?.LogEvent($"TreeView: BeforeExpand. Item: '{e.Item.Text}'");
        }

        private void TreeView_BeforeCollapse(object? sender, TreeViewItemExpandedChangingEventArgs e)
        {
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
            foreach (var item in treeView.SelectedItems)
                item.Remove();
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            treeView.Items.Add(new TreeViewItem("Item " + (treeView.Items.Count + 1), 0));
        }

        private void BeginSelectedLabelEditingButton_Click(object sender, EventArgs e)
        {
            treeView.SelectedItem?.BeginLabelEdit();
        }

        private void ExpandAllButton_Click(object sender, EventArgs e) => treeView.ExpandAll();

        private void CollapseAllButton_Click(object sender, EventArgs e) => treeView.CollapseAll();

        private void ExpandAllChildrenButton_Click(object sender, EventArgs e) => treeView.SelectedItem?.ExpandAll();

        private void CollapseAllChildrenButton_Click(object sender, EventArgs e) => treeView.SelectedItem?.CollapseAll();
    }
}