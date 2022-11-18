using Alternet.UI;
using System;
using System.Collections.Generic;
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

                InitializeColumns();

                AddItems(50);

                foreach (var item in Enum.GetValues(typeof(ListViewView)))
                    viewComboBox.Items.Add(item ?? throw new Exception());
                viewComboBox.SelectedIndex = 0;

                foreach (var item in Enum.GetValues(typeof(ListViewGridLinesDisplayMode)))
                    gridLinesComboBox.Items.Add(item ?? throw new Exception());
                gridLinesComboBox.SelectedIndex = 0;

                foreach (var item in Enum.GetValues(typeof(ListViewColumnWidthMode)))
                    columnWidthModeComboBox.Items.Add(item ?? throw new Exception());
                columnWidthModeComboBox.SelectedIndex = 0;

                site = value;
            }
        }

        private void InitializeColumns()
        {
            listView.Columns.Add(new ListViewColumn("Column 1"));
            listView.Columns.Add(new ListViewColumn("Column 2"));
        }

        private void ViewComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            listView.View = (ListViewView)(viewComboBox.SelectedItem ?? throw new InvalidOperationException());
        }

        private void GridLinesComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            listView.GridLinesDisplayMode = (ListViewGridLinesDisplayMode)(gridLinesComboBox.SelectedItem ?? throw new InvalidOperationException());
        }

        private void ColumnWidthModeComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            listView.Columns[1].WidthMode = (ListViewColumnWidthMode)(columnWidthModeComboBox.SelectedItem ?? throw new InvalidOperationException());
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
            var item = new ListViewItem("Item " + (listView.Items.Count + 1), 1);
            listView.Items.Add(item);
            item.EnsureVisible();
        }

        private void ListView_BeforeLabelEdit(object? sender, ListViewItemLabelEditEventArgs e)
        {
            e.Cancel = cancelBeforeLabelEditEventsCheckBox.IsChecked;
            site?.LogEvent($"ListView: BeforeLabelEdit. Item: '{listView.Items[e.ItemIndex].Text}', Label: '{e.Label ?? "<null>"}'");
        }

        private void ListView_AfterLabelEdit(object? sender, ListViewItemLabelEditEventArgs e)
        {
            e.Cancel = cancelAfterLabelEditEventsCheckBox.IsChecked;
            site?.LogEvent($"ListView: AfterLabelEdit. Item: '{listView.Items[e.ItemIndex].Text}', Label: '{e.Label ?? "<null>"}'");
        }

        private void ListView_ColumnClick(object? sender, ListViewColumnEventArgs e)
        {
            site?.LogEvent($"ListView: ColumnClick. Column title: '{listView.Columns[e.ColumnIndex].Title}''");
        }

        private void AllowLabelEditingCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (listView != null)
                listView.AllowLabelEdit = allowLabelEditingCheckBox.IsChecked;

            beginSelectedLabelEditingButton.Enabled = allowLabelEditingCheckBox.IsChecked;
        }

        private void BeginSelectedLabelEditingButton_Click(object sender, EventArgs e)
        {
            listView.SelectedItem?.BeginLabelEdit();
        }

        ListViewItem? GetLastItem()
        {
            return listView.Items.LastOrDefault();
        }

        private void EnsureLastItemVisibleButton_Click(object sender, System.EventArgs e) => GetLastItem()?.EnsureVisible();

        private void FocusLastItemButton_Click(object sender, System.EventArgs e)
        {
            var item = GetLastItem();
            if (item != null)
            {
                listView.Focus();
                item.IsFocused = true;
            }
        }

        private void ListView_MouseLeftButtonDown(object sender, Alternet.UI.MouseButtonEventArgs e)
        {
            var result = listView.HitTest(e.GetPosition(listView));
            
            string columnHeader;
            var columnIndex = result.Cell?.ColumnIndex;
            if (columnIndex != null)
                columnHeader = listView.Columns[columnIndex.Value].Title;
            else
                columnHeader = "<none>";

            site?.LogEvent($"HitTest result: Item: '{result.Item?.Text ?? "<none>"}, Column: '{columnHeader}, Location: {result.Location}'");

            if (logItemBoundsOnClickCheckBox.IsChecked && result.Item != null)
            {
                int index = result.Item.Index!.Value;

                var entireItemBounds = listView.GetItemBounds(index, ListViewItemBoundsPortion.EntireItem);
                var iconBounds = listView.GetItemBounds(index, ListViewItemBoundsPortion.Icon);
                var labelBounds = listView.GetItemBounds(index, ListViewItemBoundsPortion.Label);
                
                site?.LogEvent($"Item Bounds: {entireItemBounds}, Icon: {iconBounds}, Label: {labelBounds}");
            }
        }

        private void ModifyLastItemButton_Click(object sender, System.EventArgs e)
        {
            var item = GetLastItem();
            if (item != null)
            {
                item.EnsureVisible();
                item.Text += "X";

                var imageIndex = item.ImageIndex + 1;
                if (imageIndex >= listView.SmallImageList!.Images.Count)
                    imageIndex = 0;

                item.ImageIndex = imageIndex;
                item.Cells[1].ImageIndex = imageIndex;
            }
        }

        private void AddLastItemSiblingButton_Click(object sender, System.EventArgs e)
        {
            var item = GetLastItem();
            if (item != null)
            {
                var newItem = new ListViewItem(item.Text + " Sibling", item.ImageIndex ?? 0);
                listView.Items.Insert(listView.Items.IndexOf(item), newItem);
                newItem.EnsureVisible();
            }
        }

        private void ClearButton_Click(object sender, System.EventArgs e)
        {
            listView.Clear();
            InitializeColumns();
        }

        private void AddColumnButton_Click(object sender, System.EventArgs e)
        {
            listView.Columns.Add(new ListViewColumn("New Column"));
        }

        private void ModifyColumnTitleButton_Click(object sender, System.EventArgs e)
        {
            listView.Columns[1].Title += "X";
        }

        private void ColumnHeaderVisibleCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            listView.ColumnHeaderVisible = columnHeaderVisibleCheckBox.IsChecked;
        }
    }
}