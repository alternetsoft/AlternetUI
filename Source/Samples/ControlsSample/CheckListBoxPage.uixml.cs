using System;
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
                checkListBox.SelectionMode = ListBoxSelectionMode.Multiple;
                site = value;
            }
        }

        private void CheckListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var result = checkListBox.HitTest(e.GetPosition(checkListBox));

            site?.LogEvent($"HitTest result: Item: '{(result == null ? "<none>" : checkListBox.Items[result.Value])}'");
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

        private void CheckListBox_CheckedChanged(object? sender, EventArgs e)
        {
            string checkedIndicesString = checkListBox.CheckedIndices.Count > 100 ? "too many indices to display" : string.Join(",", checkListBox.CheckedIndices);
            site?.LogEvent($"ListBox: CheckedChanged. CheckedIndices: ({checkedIndicesString})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            checkListBox.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ? ListBoxSelectionMode.Multiple : ListBoxSelectionMode.Single;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            foreach (var item in checkListBox.SelectedItems.ToArray())
                checkListBox.Items.Remove(item);
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
            int index = 2;
            var count = checkListBox.Items.Count;
            if (index < count)
                checkListBox.CheckedIndex = index;
        }

        private void UncheckAllButton_Click(object sender, System.EventArgs e)
        {
            checkListBox.ClearChecked();
        }

        private void CheckItemAtIndices2And4Button_Click(object sender, System.EventArgs e)
        {
            int maxIndex = 4;
            var count = checkListBox.Items.Count;
            if (maxIndex < count)
                checkListBox.CheckedIndices = new[] { 2, maxIndex };
        }
    }
}