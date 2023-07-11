using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;
using static Alternet.UI.ControlsSampleUtils;

namespace ControlsSample
{
    internal partial class ListViewPage : Control
    {
        private IPageSite? site;
        private bool? slowSettingsEnabled;
        private double maxPanelWidth = 0;

        public ListViewPage()
        {
            InitializeComponent();
            listView.SelectionMode = ListViewSelectionMode.Multiple;
            listView.AllowLabelEdit = true;

            //LayoutFactory.SetDebugBackgroundToParents(listView);

            LayoutUpdated += ListViewPage_LayoutUpdated;
        }

        private void LogLayout(string s)
        {
            Log("================");
            Log(s);
            Log($"stackPanel1.Bounds: {stackPanel1.Bounds}");
            Log($"stackPanel2.Bounds: {stackPanel2.Bounds}");
            Log($"tabPage1.Bounds: {tabPage1.Bounds}");
            Log($"tabPage2.Bounds: {tabPage2.Bounds}");
            Log("================");
        }

        private void ListViewPage_LayoutUpdated(object sender, EventArgs e)
        {
            //LogLayout("ListViewPage");

            maxPanelWidth = Math.Max(maxPanelWidth, stackPanel2.Bounds.Width);
            maxPanelWidth = Math.Max(maxPanelWidth, stackPanel1.Bounds.Width);
            stackPanel1.Width = maxPanelWidth;
            stackPanel2.Width = maxPanelWidth;
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

                AddEnumValues(viewComboBox, typeof(ListViewView), ListViewView.Details);
                AddEnumValues(gridLinesComboBox, typeof(ListViewGridLinesDisplayMode), 
                    ListViewGridLinesDisplayMode.None);
                AddEnumValues(columnWidthModeComboBox, typeof(ListViewColumnWidthMode),
                    ListViewColumnWidthMode.AutoSize);

                listView.Items.ItemInserted += Items_ItemInserted;
                listView.Items.ItemRemoved += Items_ItemRemoved;

                site = value;

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
            foreach (var column in listView!.Columns)
                column.WidthMode = ListViewColumnWidthMode.AutoSize;
        }

        private bool SlowRecreate
        {
            get
            {
                if (listView == null)
                    return true;
                bool result = listView.Items.Count > 1000;
                return result;
            }
        }

        private void UpdateSlowRecreate()
        {
            var fastRecreate = !SlowRecreate;

            if (slowSettingsEnabled != null && fastRecreate == (bool)slowSettingsEnabled)
                return;

            slowSettingsEnabled = fastRecreate;

            viewComboBox.Enabled = fastRecreate;
            gridLinesComboBox.Enabled = fastRecreate;
            allowMultipleSelectionCheckBox.Enabled = fastRecreate;
            allowLabelEditingCheckBox.Enabled = fastRecreate;
            columnHeaderVisibleCheckBox.Enabled = fastRecreate;
            hasBorderButton.Enabled = fastRecreate;
        }

        private void ViewComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (SlowRecreate)
                return;
            if (listView is null)
                return;
            listView.View = (ListViewView)
                (viewComboBox.SelectedItem ?? throw new InvalidOperationException());
        }

        private void GridLinesComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (SlowRecreate)
                return;
            if (listView is null)
                return;
            listView.GridLinesDisplayMode = (ListViewGridLinesDisplayMode)
                (gridLinesComboBox.SelectedItem ?? throw new InvalidOperationException());
        }

        private void ColumnWidthModeComboBox_SelectedItemChanged(
            object? sender,
            EventArgs e)
        {
            var mode = (ListViewColumnWidthMode)(
                columnWidthModeComboBox.SelectedItem ??
                throw new InvalidOperationException());

            foreach (var column in listView!.Columns)
                column.WidthMode = mode;
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            AddItems(1000);
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
                {
                    listView.Items.Add(
                        new ListViewItem(new[] { 
                            "Item " + i, 
                            "Some Info " + i 
                        }, i % 4));
                }
            }
            finally
            {
                listView.EndUpdate();
            }
        }

        private void Log(string s)
        {
            site?.LogEvent(s);
        }

        private void ListView_SelectionChanged(object? sender, EventArgs e)
        {
            var i = listView.SelectedIndices.Count > 100;
            string s = i ? "too many items" : string.Join(",", listView.SelectedIndices);
            Log($"ListView: SelectedIndices: ({s})");

            if (listView.SelectedItem != null)
                Log($"ListView: SelectedItem.Index: {listView.SelectedItem.Index}");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(
            object? sender, EventArgs e)
        {
            if (SlowRecreate)
                return;
            if (listView is null)
                return;
            listView.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ? 
                ListViewSelectionMode.Multiple : ListViewSelectionMode.Single;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            int selectedIndex = listView.SelectedIndex ?? -1;
            listView.RemoveSelectedItems();
            if(listView.Items.Count > 0 && selectedIndex >= 0)
                listView.SelectedIndex = Math.Min(selectedIndex, listView.Items.Count-1);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            var item = new ListViewItem("Item " + (listView.Items.Count + 1), 1);
            listView.Items.Add(item);
            item.EnsureVisible();
        }

        private void ListView_BeforeLabelEdit(
            object? sender, ListViewItemLabelEditEventArgs e)
        {
            e.Cancel = cancelBeforeLabelEditEventsCheckBox.IsChecked;
            var s = listView.Items[e.ItemIndex].Text;
            var lbl = e.Label ?? "<null>";
            Log($"ListView: BeforeLabelEdit. Item: '{s}', Label: '{lbl}'");
        }

        private void ListView_AfterLabelEdit(object? sender, 
            ListViewItemLabelEditEventArgs e)
        {
            e.Cancel = cancelAfterLabelEditEventsCheckBox.IsChecked;
            var s = listView.Items[e.ItemIndex].Text;
            var lbl = e.Label ?? "<null>";
            Log($"ListView: AfterLabelEdit. Item: '{s}', Label: '{lbl}'");
        }

        private void ListView_ColumnClick(object? sender, ListViewColumnEventArgs e)
        {
            var s = listView.Columns[e.ColumnIndex].Title;
            site?.LogEvent($"ListView: ColumnClick. Column title: '{s}'");
        }

        private void AllowLabelEditingCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (SlowRecreate)
                return;
            if (listView is not null)
                listView.AllowLabelEdit = allowLabelEditingCheckBox.IsChecked;
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

        private void EnsureLastItemVisibleButton_Click(object sender, System.EventArgs e) 
            => GetLastItem()?.EnsureVisible();

        private void HasBorderButton_Click(object sender, System.EventArgs e)
        {
            listView.HasBorder = !listView.HasBorder;
        }

        private void FocusLastItemButton_Click(object sender, System.EventArgs e)
        {
            var item = GetLastItem();
            if (item != null)
            {
                listView.SetFocus();
                item.IsFocused = true;
                listView.SelectedItem = item;
            }
        }

        private void ListView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var result = listView.HitTest(e.GetPosition(listView));

            string columnHeader;
            var columnIndex = result.Cell?.ColumnIndex;
            if (columnIndex != null)
                columnHeader = listView.Columns[columnIndex.Value].Title;
            else
                columnHeader = "<none>";

            var s = result.Item?.Text ?? "<none>";
            Log($"HitTest result: Item: '{s}', Column: '{columnHeader}', " +
                $"Location: '{result.Location}'");

            if (logItemBoundsOnClickCheckBox.IsChecked && result.Item != null)
            {
                int index = result.Item.Index!.Value;

                var entireItemBounds = listView.GetItemBounds(index, 
                    ListViewItemBoundsPortion.EntireItem);
                var iconBounds = listView.GetItemBounds(index, 
                    ListViewItemBoundsPortion.Icon);
                var labelBounds = listView.GetItemBounds(index, 
                    ListViewItemBoundsPortion.Label);

                Log($"Item Bounds: {entireItemBounds}, " +
                    $"Icon: {iconBounds}, Label: {labelBounds}");
            }
        }

        private void ModifyLastItemButton_Click(object sender, System.EventArgs e)
        {
            var item = GetLastItem();
            if (item != null)
            {
                item.EnsureVisible();

                var imageIndex = item.ImageIndex + 1;
                if (imageIndex >= listView.SmallImageList!.Images.Count)
                    imageIndex = 0;

                item.ImageIndex = imageIndex;

                foreach (var cell in item.Cells)
                {
                    cell.Text += cell.ColumnIndex.ToString();
                    cell.ImageIndex = imageIndex;
                }
            }
        }

        private void AddLastItemSiblingButton_Click(object sender, System.EventArgs e)
        {
            var item = GetLastItem();
            if (item != null)
            {
                listView.BeginUpdate();
                var itemIndex = listView.Items.IndexOf(item);
                var imageIndex = item.ImageIndex ?? 0;
                var newItem = new ListViewItem(item.Text + " Sibling", imageIndex);
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
            listView.Columns.Add(
                new ListViewColumn($"New Column {listView.Columns.Count}"));
        }

        private void ModifyColumnTitleButton_Click(object sender, System.EventArgs e)
        {
            foreach(var column in listView.Columns)
                column.Title += column.Index;
        }

        private void ColumnHeaderVisibleCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (SlowRecreate)
                return;
            listView.ColumnHeaderVisible = columnHeaderVisibleCheckBox.IsChecked;
        }

        private void Items_ItemRemoved(object sender,
            CollectionChangeEventArgs<ListViewItem> e)
        {
            UpdateSlowRecreate();
        }

        private void Items_ItemInserted(object sender,
            CollectionChangeEventArgs<ListViewItem> e)
        {
            UpdateSlowRecreate();
        }
    }
}