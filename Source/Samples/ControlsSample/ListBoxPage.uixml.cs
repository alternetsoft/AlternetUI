using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ListBoxPage : Control
    {
        private IPageSite? site;

        public ListBoxPage()
        {
            InitializeComponent();
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                listBox.Items.Add("One");
                listBox.Items.Add("Two");
                listBox.Items.Add("Three");
                listBox.Items.Add("Four");
                listBox.Items.Add("Five");
                listBox.Items.Add("Six");
                listBox.Items.Add("Seven");
                listBox.Items.Add("Eight");
                listBox.Items.Add("Nine");
                listBox.Items.Add("Ten");
                site = value;
            }
        }

        private void ListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var result = listBox.HitTest(e.GetPosition(listBox));

            site?.LogEvent($"HitTest result: Item: '{(result == null ? "<none>" : listBox.Items[result.Value])}'");
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            int start = listBox.Items.Count + 1;

            listBox.BeginUpdate();
            try
            {
                for (int i = start; i < start + 5000; i++)
                    listBox.Items.Add("Item " + i);
            }
            finally
            {
                listBox.EndUpdate();
            }
        }

        private void ListBox_SelectionChanged(object? sender, EventArgs e)
        {
            string selectedIndicesString = listBox.SelectedIndices.Count > 100 ? "too many indices to display" : string.Join(",", listBox.SelectedIndices);
            site?.LogEvent($"ListBox: SelectionChanged. SelectedIndices: ({selectedIndicesString})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            listBox.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ? ListBoxSelectionMode.Multiple : ListBoxSelectionMode.Single;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            foreach (var item in listBox.SelectedItems.ToArray())
                listBox.Items.Remove(item);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            listBox.Items.Add("Item " + (listBox.Items.Count + 1));
        }

        private void EnsureLastItemVisibleButton_Click(object sender, System.EventArgs e)
        {
            var count = listBox.Items.Count;
            if (count > 0)
                listBox.EnsureVisible(count - 1);
        }

        private void SelectItemAtIndex2Button_Click(object sender, System.EventArgs e)
        {
            int index = 2;
            var count = listBox.Items.Count;
            if (index < count)
                listBox.SelectedIndex = index;
        }

        private void DeselectAllButton_Click(object sender, System.EventArgs e)
        {
            listBox.SelectedItem = null;
        }

        private void SelectItemAtIndices2And4Button_Click(object sender, System.EventArgs e)
        {
            int maxIndex = 4;
            var count = listBox.Items.Count;
            if (maxIndex < count)
                listBox.SelectedIndices = new[] { 2, maxIndex };
        }
    }
}