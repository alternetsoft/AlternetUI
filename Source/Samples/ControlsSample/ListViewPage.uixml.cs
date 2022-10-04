using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class ListViewPage : Control
    {
        private IPageSite? site;

        public ListViewPage()
        {
            InitializeComponent();
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                var imageLists = ResourceLoader.LoadImageLists();
                listView.SmallImageList = imageLists.Small;
                listView.LargeImageList = imageLists.Large;

                listView.Columns.Add(new ListViewColumn("Column 1"));
                listView.Columns.Add(new ListViewColumn("Column 2"));

                AddItems(10);

                foreach (var item in Enum.GetValues(typeof(ListViewView)))
                    viewComboBox.Items.Add(item ?? throw new Exception());
                viewComboBox.SelectedIndex = 0;

                site = value;
            }
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
            site?.LogEvent($"ListView: SelectionChanged. SelectedIndices: ({selectedIndicesString})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            listView.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ? ListViewSelectionMode.Multiple : ListViewSelectionMode.Single;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            foreach (var item in listView.SelectedItems.ToArray())
                listView.Items.Remove(item);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            listView.Items.Add(new ListViewItem("Item " + (listView.Items.Count + 1)));
        }
    }
}