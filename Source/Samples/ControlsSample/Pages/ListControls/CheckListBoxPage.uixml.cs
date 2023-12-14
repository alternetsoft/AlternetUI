using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class CheckListBoxPage : Control
    {
        private int newItemIndex = 0;

        public CheckListBoxPage()
        {
            InitializeComponent();

            checkListBox.Items.Add("One");
            checkListBox.Items.Add("Two");
            checkListBox.Items.Add("Three");
            checkListBox.Items.Add("Four");
            checkListBox.Items.Add("Five");
            checkListBox.Items.Add("Six");
            checkListBox.Items.Add("Seven");
            checkListBox.Items.Add("Eight");
            checkListBox.Items.Add("Nine");
            checkListBox.Items.Add("Ten");

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
            var result = checkListBox.HitTest(e.GetPosition(checkListBox));
            var item = (result == null ? 
                "<none>" : checkListBox.Items[result.Value]);
            Application.Log($"HitTest result: Item: '{item}'");
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
                    checkListBox.Items.Add("Item " + GenItemIndex());
            }
            finally
            {
                checkListBox.EndUpdate();
            }
        }

        private static string IndicesToStr(IReadOnlyList<int> indices)
        {
            string result = indices.Count > 100 ? 
                "too many indices to display" : string.Join(",", indices);
            return result;
        }

        private void CheckListBox_CheckedChanged(object? sender, EventArgs e)
        {
            string checkedIndicesString = IndicesToStr(checkListBox.CheckedIndices);
            Application.Log(
                $"CheckListBox: CheckedChanged. Checked: ({checkedIndicesString})");
        }

        private void CheckListBox_SelectionChanged(object? sender, EventArgs e)
        {
            string selectedIndicesStr = 
                IndicesToStr(checkListBox.SelectedIndices);
            Application.Log(
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
            Application.Log($"Remove items: ({IndicesToStr(items)})");
            checkListBox.RemoveItems(items);
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            RemoveItemsAndLog(checkListBox.SelectedIndicesDescending);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            checkListBox.Items.Add("Item " + GenItemIndex());
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
            checkListBox.CheckItems([2]);
        }

        private void UncheckAllButton_Click(object? sender, EventArgs e)
        {
            checkListBox.ClearChecked();
        }

        private void CheckItemAtIndices2And4Button_Click(
            object? sender, 
            EventArgs e)
        {
            checkListBox.CheckItems([2, 4]);
        }
    }
}