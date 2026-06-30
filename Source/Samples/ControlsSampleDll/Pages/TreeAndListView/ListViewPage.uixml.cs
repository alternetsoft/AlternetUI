using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;
using static Alternet.UI.ControlsSampleUtils;

namespace ControlsSample
{
    internal partial class ListViewPage : Panel
    {
        private int newItemIndex = 0;
        private int newColIndex = 2;

        private ListControlColumn? nameColumn;
        private ListControlColumn? dataColumn;
        private ListControlColumn? infoColumn;

        static ListViewPage()
        {
        }

        public ListViewPage()
        {
            InitializeComponent();
            
            ControlSet lastItemButtons = new(
                ensureLastItemVisibleButton,
                modifyLastItemButton,
                focusLastItemButton,
                addLastItemSiblingButton);
            lastItemButtons.MinWidthToMaxPreferred();

            ControlSet buttons = new(
                addItemButton,
                removeItemButton,
                addManyItemsButton,
                ClearButton,
                AddColumnButton,
                ModifyColumnTitleButton,
                editItemsButton,
                editColumnsButton,
                beginSelectedLabelEditingButton);
            buttons.MinWidthToMaxPreferred();

            viewComboBox.EnumType = typeof(ListViewView);
            viewComboBox.Value = ListViewView.Details;

            gridLinesComboBox.EnumType = typeof(ListViewGridLinesDisplayMode);
            gridLinesComboBox.Value = ListViewGridLinesDisplayMode.Vertical;

            columnWidthModeComboBox.EnumType = typeof(ListViewColumnWidthMode);

            ListControlUtils.SetTestItemsWithColumns(listView, 100, false);

            nameColumn = listView.ColumnByName("Name");
            dataColumn = listView.ColumnByName("Data");
            infoColumn = listView.ColumnByName("Info");

            listView.SetPreferredColumnWidth();

            listView.ListBox.SelectionMode = ListBoxSelectionMode.Multiple;

            listView.ListBox.ItemTextEdited+=(s,e) =>
            {
                e.Item.SetText(nameColumn, e.NewText);
                App.Log($"Item text edited: {e.NewText}");
            };

            /*
            listView.SelectionMode = ListViewSelectionMode.Multiple;
            listView.AllowLabelEdit = true;
            */
        }

        private void EditItemsButton_Click(object? sender, System.EventArgs e)
        {
            /*DialogFactory.EditItemsWithListEditor(listView);*/
        }

        private void EditColumnsButton_Click(object? sender, System.EventArgs e)
        {
            /*DialogFactory.EditColumnsWithListEditor(listView);*/
        }

        private void ViewComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            /*
            if (listView is null)
                return;
            listView.View = (ListViewView)
                (viewComboBox.Value ?? throw new InvalidOperationException());
            */
        }

        private void GridLinesComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (listView is null)
                return;
            listView.ListBox.GridLinesDisplayMode = (ListViewGridLinesDisplayMode)
                (gridLinesComboBox.Value ??
                throw new InvalidOperationException());
        }

        private void ColumnWidthModeComboBox_SelectedItemChanged(
            object? sender,
            EventArgs e)
        {
            /*
            var mode = (ListViewColumnWidthMode)(
                columnWidthModeComboBox.Value ??
                throw new InvalidOperationException());

            foreach (var column in listView!.Columns)
                column.WidthMode = mode;
            */
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            ListControlUtils.AddTestItemsWithColumns(listView.RootItem, 1000, false);
        }

        private int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        private int GenColIndex()
        {
            newColIndex++;
            return newColIndex;
        }

        private void Button_SelectionChanged(object? sender, EventArgs e)
        {
            var i = listView.ListBox.SelectedIndices.Count > 100;
            string s = i ? "too many items" :
                string.Join(",", listView.ListBox.SelectedIndices);
            Log($"ListView: SelectedIndices: ({s})");

            if (listView.ListBox.SelectedItem != null)
                Log($"ListView: SelectedItem.Index: {listView.ListBox.SelectedIndex}");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(
            object? sender, EventArgs e)
        {
            if (listView is null)
                return;
            listView.ListBox.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ?
                ListBoxSelectionMode.Multiple : ListBoxSelectionMode.Single;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            var selectedIndex = listView.ListBox.SelectedIndex ?? -1;
            listView.RemoveSelected();
            if (listView.Items.Count > 0 && selectedIndex >= 0)
                listView.ListBox.SelectedIndex =
                    Math.Min(selectedIndex, listView.Items.Count - 1);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            ListControlUtils.AddTestItemsWithColumns(listView.RootItem, 1, false);
            listView.ListBox.SelectLastItemAndScroll();
        }

        /*
        private void ListView_BeforeLabelEdit(
            object? sender, ListViewItemLabelEditEventArgs e)
        {
            e.Cancel = cancelBeforeLabelEditEventsCheckBox.IsChecked;
            var s = listView.Items[e.ItemIndex].Text;
            var lbl = e.Label ?? "<null>";
            Log($"ListView: BeforeLabelEdit. Item: '{s}', Label: '{lbl}'");
        }
        */

        /*
        private void ListView_AfterLabelEdit(object? sender,
            ListViewItemLabelEditEventArgs e)
        {
            e.Cancel = cancelAfterLabelEditEventsCheckBox.IsChecked;
            var s = listView.Items[e.ItemIndex].Text;
            var lbl = e.Label ?? "<null>";
            Log($"ListView: AfterLabelEdit. Item: '{s}', Label: '{lbl}'");
        }
        */

        /*
        private void ListView_ColumnClick(object? sender, ListViewColumnEventArgs e)
        {
            var s = listView.Columns[e.ColumnIndex].Title;
            App.Log($"ListView: ColumnClick. Column title: '{s}'");
        }
        */

        private void AllowLabelEditingCheckBox_CheckedChanged(
            object? sender,
            EventArgs e)
        {
            /*
            if (listView is not null)
                listView.AllowLabelEdit = allowLabelEditingCheckBox.IsChecked;
            beginSelectedLabelEditingButton.Enabled =
                allowLabelEditingCheckBox.IsChecked;
            */
        }

        private void BeginSelectedLabelEditingButton_Click(
            object? sender,
            EventArgs e)
        {
            listView.ListBox.ShowItemEditor();
        }

        private TreeViewItem? GetLastItem()
        {
            return listView.Items.LastOrDefault();
        }

        private void EnsureLastItemVisibleButton_Click(object? sender, System.EventArgs e)
        {
            listView.ListBox.ScrollToLastRow();
        }

        private void HasBorderButton_Click(object sender, System.EventArgs e)
        {
            listView.HasBorder = !listView.HasBorder;
        }

        private void FocusLastItemButton_Click(object? sender, System.EventArgs e)
        {
            var item = GetLastItem();
            if (item != null)
            {
                listView.SetFocusIfPossible();
                listView.SelectedItem = item;
            }
        }

        private void ListView_MouseLeftButtonDown(object? sender, MouseEventArgs e)
        {
            /*
            var result = listView.HitTest(Mouse.GetPosition(listView));

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
                var index = result.Item.Index!.Value;

                var entireItemBounds = listView.GetItemBounds(index,
                    ListViewItemBoundsPortion.EntireItem);
                var iconBounds = listView.GetItemBounds(index,
                    ListViewItemBoundsPortion.Icon);
                var labelBounds = listView.GetItemBounds(index,
                    ListViewItemBoundsPortion.Label);

                Log($"Item Bounds: {entireItemBounds}, " +
                    $"Icon: {iconBounds}, Label: {labelBounds}");
            }
            */
        }

        private void ModifyLastItemButton_Click(object? sender, System.EventArgs e)
        {
            var item = GetLastItem();
            if (item != null)
            {
                item.EnsureVisible();

                var i = 0;
                foreach (var cell in item.Cells)
                {
                    cell.Text += i.ToString();
                    i++;
                }

                listView.Invalidate();
            }
        }

        private void Initialize(TreeViewItem item)
        {
            if (nameColumn == null || dataColumn == null || infoColumn == null)
                return;

            var textCell = item.SafeCell(nameColumn);
            textCell.Text = item.Text;
            textCell.SvgImage = KnownColorSvgImages.ImgLogo;

            var dataCell = item.SafeCell(dataColumn);
            dataCell.Text = "Data " + LogUtils.GenNewId();
            dataCell.HorizontalAlignment = HorizontalAlignment.Right;

            var infoCell = item.SafeCell(infoColumn);
            infoCell.Text = "Info " + LogUtils.GenNewId();
        }

        private void AddLastItemSiblingButton_Click(object? sender, System.EventArgs e)
        {
            var itemIndex = listView.Items.Count - 1;
            if (itemIndex >= 0)
            {
                var newItem = new TreeViewItem();
                newItem.Text = "Item " + LogUtils.GenNewId();
                Initialize(newItem);
                listView.RootItem.Insert(itemIndex, newItem);
                newItem.EnsureVisible();
            }
        }

        private void ClearButton_Click(object? sender, System.EventArgs e)
        {
            listView.Clear();
        }

        private void AddColumnButton_Click(object? sender, System.EventArgs e)
        {
            listView.AddColumn($"Column {GenColIndex()}", 100);
        }

        private void ModifyColumnTitleButton_Click(
            object? sender,
            System.EventArgs e)
        {
            foreach (var column in listView.Header.ColumnControls)
            {
                column.Text += "A";
            }
        }

        private void ColumnHeaderVisibleCheckBox_CheckedChanged(object? sender, System.EventArgs e)
        {
            listView.IsHeaderVisible = columnHeaderVisibleCheckBox.IsChecked;
        }
    }
}