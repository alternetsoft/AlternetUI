using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    internal class TreeViewPage : Control
    {
        private readonly IPageSite site;
        private TreeView treeView;
        private CheckBox allowMultipleSelectionCheckBox;

        public TreeViewPage(IPageSite site)
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("ControlsSample.TreeViewPage.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            treeView = (TreeView)FindControl("treeView");
            treeView.SelectionChanged += TreeView_SelectionChanged;

            treeView.ImageList = ResourceLoader.LoadImageLists().Small;

            AddItems(10);

            ((Button)FindControl("addItemButton")).Click += AddItemButton_Click;
            ((Button)FindControl("removeItemButton")).Click += RemoveItemButton_Click;
            ((Button)FindControl("addManyItemsButton")).Click += AddManyItemsButton_Click;

            allowMultipleSelectionCheckBox = (CheckBox)FindControl("allowMultipleSelectionCheckBox");
            allowMultipleSelectionCheckBox.CheckedChanged += AllowMultipleSelectionCheckBox_CheckedChanged;

            this.site = site;
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
                    treeView.Items.Add(new TreeViewItem("Item " + i, i % 4));
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
            site.LogEvent($"TreeView: SelectionChanged. SelectedItems: ({selectedItemsString})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            treeView.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ? TreeViewSelectionMode.Multiple : TreeViewSelectionMode.Single;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            foreach (var item in treeView.SelectedItems)
                item.Remove();
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            treeView.Items.Add(new TreeViewItem("Item " + (treeView.Items.Count + 1)));
        }
    }
}