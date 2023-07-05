using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ListViewPage : Control
    {
        private IPageSite? site;
        private int ignoreRecreate = 1;

        public ListViewPage()
        {
            InitializeComponent();

            //LayoutFactory.SetDebugBackgroundToParents(listView);

            this.LayoutUpdated += ListViewPage_LayoutUpdated;

        }

        private void ListViewPage_LayoutUpdated(object sender, EventArgs e)
        {
            /*var bounds1 = stackPanel1.Bounds;
            var bounds2 = stackPanel2.Bounds;
            var bounds3 = tabPage1.Bounds;
            var bounds4 = tabPage2.Bounds;

            var b = Math.Max(Math.Max(bounds1.Width, bounds2.Width), 
                Math.Max(bounds3.Width, bounds4.Width));

            tabControl.Width = Math.Max(b + 30, tabControl.Width);*/
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                var imageLists = ResourceLoader.LoadImageLists();
                listView.SmallImageList = imageLists.Small;
                listView.LargeImageList = imageLists.Large;

                AddDefaultItems();

                foreach (var item in Enum.GetValues(typeof(ListViewView)))
                    viewComboBox.Items.Add(item ?? throw new Exception());
                viewComboBox.SelectedItem = ListViewView.Details;

                foreach (var item in Enum.GetValues(typeof(ListViewGridLinesDisplayMode)))
                    gridLinesComboBox.Items.Add(item ?? throw new Exception());
                gridLinesComboBox.SelectedIndex = 0;

                foreach (var item in Enum.GetValues(typeof(ListViewColumnWidthMode)))
                    columnWidthModeComboBox.Items.Add(item ?? throw new Exception());
                columnWidthModeComboBox.SelectedIndex = 1;

                listView.Items.ItemInserted += Items_ItemInserted;
                listView.Items.ItemRemoved += Items_ItemRemoved;

                site = value;

                ignoreRecreate--;
            }
        }

        private void InitializeColumns()
        {
            listView?.Columns.Add(new ListViewColumn("Column 1"));
            listView?.Columns.Add(new ListViewColumn("Column 2"));
        }

        private void AddDefaultItems()
        {
            InitializeColumns();
            AddItems(50);
            listView.Columns[1].WidthMode = ListViewColumnWidthMode.AutoSize;
        }

        private void ViewComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            BeginRecreateListView();
            try
            {
                if (listView is not null)
                    listView.View = (ListViewView)(viewComboBox.SelectedItem ?? throw new InvalidOperationException());
            }
            finally
            {
                EndRecreateListView();
            }
        }

        private void BeginRecreateListView()
        {
            if (ignoreRecreate>0)
                return;

            ignoreRecreate++;
            listView?.BeginUpdate();
            listView?.Clear();            
            //listView?.Parent?.BeginUpdate();
            ignoreRecreate--;
        }


        private void EndRecreateListView()
        {
            if (ignoreRecreate>0)
                return;
            ignoreRecreate++;
            AddDefaultItems();
            listView?.EndUpdate();
            //listView?.Parent?.EndUpdate();
            ignoreRecreate--;
        }

        private void GridLinesComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            BeginRecreateListView();
            try
            {
                if (listView is not null)
                    listView.GridLinesDisplayMode = (ListViewGridLinesDisplayMode)(gridLinesComboBox.SelectedItem ?? throw new InvalidOperationException());
            }
            finally
            {
                EndRecreateListView();
            }
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
            if (listView == null)
                return;

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

            if (listView.SelectedItem == null)
                return;
            //var s = listView.SelectedItem.Index;
            //site?.LogEvent(s.ToString());
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            BeginRecreateListView();

            try
            {
                if (listView is not null)
                    listView.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ? ListViewSelectionMode.Multiple : ListViewSelectionMode.Single;
            }
            finally
            {
                EndRecreateListView();
            }
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
            BeginRecreateListView();

            try
            {
                if (listView is not null)
                    listView.AllowLabelEdit = allowLabelEditingCheckBox.IsChecked;
            }
            finally
            {
                EndRecreateListView();
            }

            beginSelectedLabelEditingButton.Enabled = allowLabelEditingCheckBox.IsChecked;
        }

        private void BeginSelectedLabelEditingButton_Click(object sender, EventArgs e)
        {
            listView.SelectedItem?.BeginLabelEdit();
        }

        private ListViewItem? GetLastItem()
        {
            return listView.Items.LastOrDefault();
        }

        private void EnsureLastItemVisibleButton_Click(object sender, System.EventArgs e) => GetLastItem()?.EnsureVisible();

        private void FocusLastItemButton_Click(object sender, System.EventArgs e)
        {
            var item = GetLastItem();
            if (item != null)
            {
                listView.SetFocus();
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
                listView.BeginUpdate();
                var itemIndex = listView.Items.IndexOf(item);
                var newItem = new ListViewItem(item.Text + " Sibling", item.ImageIndex ?? 0);
                listView.Items.Insert(itemIndex, newItem);
                newItem.EnsureVisible();
                listView.EndUpdate();
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
            BeginRecreateListView();

            try
            {
                listView.ColumnHeaderVisible = columnHeaderVisibleCheckBox.IsChecked;
            }
            finally
            {
                EndRecreateListView();
            }
        }

        private void LogItemCount(string s)
        {
            site?.LogEvent($"{s}; UI:{listView.Items.Count}; Native: {listView.NativeItemsCount}");
        }

        private void TestItemButton_Click(object? sender, EventArgs e)
        {
            LogItemCount("TestButton");
        }

        private void Items_ItemRemoved(object sender, Alternet.Base.Collections.CollectionChangeEventArgs<ListViewItem> e)
        {
            //LogItemCount("Items_ItemRemoved");
        }

        private void Items_ItemInserted(object sender, Alternet.Base.Collections.CollectionChangeEventArgs<ListViewItem> e)
        {
            //LogItemCount("Items_ItemInserted");
        }
    }
}