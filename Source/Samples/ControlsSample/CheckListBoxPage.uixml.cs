using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class CheckListBoxPage : Control
    {
        private IPageSite? site;

        public CheckListBoxPage()
        {
            InitializeComponent();
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
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

                // Do not comment this line or items will not be painted properly
                checkListBox.RecreateWindow();

                checkListBox.SelectionChanged += CheckListBox_SelectionChanged;
                allowMultipleSelectionCheckBox.IsChecked = checkListBox.SelectionMode == ListBoxSelectionMode.Multiple;
                site = value;
            }
        }

        private void CheckListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var result = checkListBox.HitTest(e.GetPosition(checkListBox));

            site?.LogEvent($"HitTest result: Item: '{(result == null ? "<none>" : checkListBox.Items[result.Value])}'");
        }

        private void HasBorderButton_Click(object? sender, EventArgs e)
        {
            checkListBox.HasBorder = !checkListBox.HasBorder;
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            int start = checkListBox.Items.Count + 1;

            checkListBox.BeginUpdate();
            try
            {
                for (int i = start; i < start + 5000; i++)
                    checkListBox.Items.Add("Item " + i);
            }
            finally
            {
                checkListBox.EndUpdate();
            }
        }

        private string IndicesToStr(IReadOnlyList<int> indices)
        {
            string result = indices.Count > 100 ? 
                "too many indices to display" : string.Join(",", indices);
            return result;
        }

        private void CheckListBox_CheckedChanged(object? sender, EventArgs e)
        {
            string checkedIndicesString = IndicesToStr(checkListBox.CheckedIndices);
            site?.LogEvent(
                $"CheckListBox: CheckedChanged. Checked: ({checkedIndicesString})");
        }

        private void CheckListBox_SelectionChanged(object? sender, EventArgs e)
        {
            string selectedIndicesString = IndicesToStr(checkListBox.SelectedIndices);
            site?.LogEvent(
                $"CheckListBox: SelectionChanged. Selected: ({selectedIndicesString})");
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
            site?.LogEvent($"Remove items: ({IndicesToStr(items)})");
            checkListBox.RemoveItems(items);
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            RemoveItemsAndLog(checkListBox.SelectedIndicesDescending);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            checkListBox.Items.Add("Item " + (checkListBox.Items.Count + 1));
        }

        private void EnsureLastItemVisibleButton_Click(object sender, System.EventArgs e)
        {
            var count = checkListBox.Items.Count;
            if (count > 0)
                checkListBox.EnsureVisible(count - 1);
        }

        private void CheckItemAtIndex2Button_Click(object sender, System.EventArgs e)
        {
            checkListBox.CheckItems(2);
        }

        private void UncheckAllButton_Click(object sender, System.EventArgs e)
        {
            checkListBox.ClearChecked();
        }

        private void CheckItemAtIndices2And4Button_Click(object sender, System.EventArgs e)
        {
            checkListBox.CheckItems(2,4);
        }
    }
}