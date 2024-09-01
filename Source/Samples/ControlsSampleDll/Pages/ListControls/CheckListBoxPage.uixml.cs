using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    [IsCsLocalized(true)]
    internal partial class CheckListBoxPage : Control
    {
        private int newItemIndex = 0;

        public CheckListBoxPage()
        {
            InitializeComponent();

            GenericStrings.AddTenRows(checkListBox.Items.Add);

            checkListBox.SelectionChanged += CheckListBox_SelectionChanged;
            allowMultipleSelectionCheckBox.IsChecked =
                checkListBox.SelectionMode == ListBoxSelectionMode.Multiple;
        }

        private void EditorButton_Click(object? sender, System.EventArgs e)
        {
            DialogFactory.EditItemsWithListEditor(checkListBox);
        }

        private void CheckListBox_MouseLeftButtonDown(
            object? sender, 
            MouseEventArgs e)
        {
            var result = checkListBox.HitTest(Mouse.GetPosition(checkListBox));
            var item = (result == null ? 
                GenericStrings.NoneInsideLessGreater : checkListBox.Items[result.Value]);
            App.Log($"{GenericStrings.HitTestResult}: {GenericStrings.Item}: '{item}'");
        }

        private void HasBorderButton_Click(object? sender, EventArgs e)
        {
            checkListBox.HasBorder = !checkListBox.HasBorder;
        }

        private int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            checkListBox.BeginUpdate();
            try
            {
                for (int i = 0; i < 5000; i++)
                    checkListBox.Items.Add($"{GenericStrings.Item} " + GenItemIndex());
            }
            finally
            {
                checkListBox.EndUpdate();
            }
        }

        private static string IndicesToStr(IReadOnlyList<int> indices)
        {
            string result = indices.Count > 100 ? 
                GenericStrings.TooManyIndexesToDisplay : string.Join(",", indices);
            return result;
        }

        private void CheckListBox_CheckedChanged(object? sender, EventArgs e)
        {
            string checkedIndicesString = IndicesToStr(checkListBox.CheckedIndices);
            App.Log(
                $"CheckListBox: CheckedChanged. Checked: ({checkedIndicesString})");
        }

        private void CheckListBox_SelectionChanged(object? sender, EventArgs e)
        {
            string selectedIndicesStr = 
                IndicesToStr(checkListBox.SelectedIndices);
            App.Log(
                $"CheckListBox: SelectionChanged. Selected: ({selectedIndicesStr})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(
            object? sender, 
            EventArgs e)
        {
            checkListBox.Parent?.BeginUpdate();
            checkListBox.SelectionMode = 
                allowMultipleSelectionCheckBox.IsChecked ? 
                ListBoxSelectionMode.Multiple : ListBoxSelectionMode.Single;
            checkListBox.Parent?.EndUpdate();
        }

        private void RemoveCheckedButton_Click(object? sender, EventArgs e)
        {
            RemoveItemsAndLog(checkListBox.CheckedIndicesDescending);
        }

        private void RemoveItemsAndLog(IReadOnlyList<int> items)
        {
            App.Log($"{GenericStrings.RemoveItems}: ({IndicesToStr(items)})");
            checkListBox.RemoveItems(items);
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            RemoveItemsAndLog(checkListBox.SelectedIndicesDescending);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            checkListBox.Items.Add($"{GenericStrings.Item} " + GenItemIndex());
        }

        private void EnsureLastItemVisibleButton_Click(
            object? sender, 
            EventArgs e)
        {
            var count = checkListBox.Items.Count;
            if (count > 0)
                checkListBox.EnsureVisible(count - 1);
        }

        private void CheckItemAtIndex2Button_Click(
            object? sender, 
            EventArgs e)
        {
            checkListBox.CheckItems(2);
        }

        private void UncheckAllButton_Click(object? sender, EventArgs e)
        {
            checkListBox.ClearChecked();
        }

        private void CheckItemAtIndices2And4Button_Click(
            object? sender, 
            EventArgs e)
        {
            checkListBox.CheckItems(2, 4);
        }
    }
}