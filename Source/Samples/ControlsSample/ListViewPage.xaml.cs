using Alternet.UI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ControlsSample
{
    internal class ListViewPage : Control
    {
        private ListView listView;
        private ComboBox viewComboBox;
        private CheckBox allowMultipleSelectionCheckBox;
        private readonly IPageSite site;

        public ListViewPage(IPageSite site)
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("ControlsSample.ListViewPage.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            listView = (ListView)FindControl("listView");
            listView.SelectionChanged += ListView_SelectionChanged;

            var imageLists = ResourceLoader.LoadImageLists();
            listView.SmallImageList = imageLists.Small;
            listView.LargeImageList = imageLists.Large;

            listView.Columns.Add(new ListViewColumn("Column 1"));
            listView.Columns.Add(new ListViewColumn("Column 2"));

            AddItems(10);

            ((Button)FindControl("addItemButton")).Click += AddItemButton_Click;
            ((Button)FindControl("removeItemButton")).Click += RemoveItemButton_Click;
            ((Button)FindControl("addManyItemsButton")).Click += AddManyItemsButton_Click;

            allowMultipleSelectionCheckBox = (CheckBox)FindControl("allowMultipleSelectionCheckBox");
            allowMultipleSelectionCheckBox.CheckedChanged += AllowMultipleSelectionCheckBox_CheckedChanged;

            viewComboBox = (ComboBox)FindControl("viewComboBox");
            viewComboBox.SelectedItemChanged += ViewComboBox_SelectedItemChanged;
            foreach(var item in Enum.GetValues(typeof(ListViewView)))
                viewComboBox.Items.Add(item ?? throw new Exception());
            viewComboBox.SelectedIndex = 0;

            this.site = site;
        }

        private void ViewComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            listView.View = (ListViewView)(viewComboBox.SelectedItem ?? throw new InvalidOperationException());
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            AddItems(5000);
        }

        private void AddItems(int count)
        {
            int start = listView.Items.Count + 1;

            listView.BeginUpdate();
            try
            {
                for (int i = start; i < start + count; i++)
                    listView.Items.Add(new ListViewItem(new[] { "Item " + i, "Some Info " + i }, i % 4));
            }
            finally
            {
                listView.EndUpdate();
            }
        }

        private void ListView_SelectionChanged(object? sender, EventArgs e)
        {
            string selectedIndicesString = listView.SelectedIndices.Count > 100 ? "too many indices to display" : string.Join(",", listView.SelectedIndices);
            site.LogEvent($"ListView: SelectionChanged. SelectedIndices: ({selectedIndicesString})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            listView.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ? ListViewSelectionMode.Multiple : ListViewSelectionMode.Single;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            if (listView.Items.Count > 0)
                listView.Items.RemoveAt(listView.Items.Count - 1);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            listView.Items.Add(new ListViewItem("Item " + (listView.Items.Count + 1)));
        }
    }
}