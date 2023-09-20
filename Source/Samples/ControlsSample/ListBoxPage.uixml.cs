using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class ListBoxPage : Control
    {
        private IPageSite? site;
        private int newItemIndex = 0;

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

        private void EditorButton_Click(object? sender, System.EventArgs e)
        {
            DialogFactory.EditItemsWithListEditor(listBox);
        }

        private int GenItemIndex()
        {
            newItemIndex++;
            return newItemIndex;
        }

        private void HasBorderButton_Click(object? sender, EventArgs e)
        {
            listBox.HasBorder = !listBox.HasBorder;
        }

        private void ListBox_MouseLeftButtonDown(
            object? sender, 
            MouseButtonEventArgs e)
        {
            var result = listBox.HitTest(e.GetPosition(listBox));
            var item = (result == null ? "<none>" : listBox.Items[result.Value]);

            site?.LogEvent($"HitTest result: Item: '{item}'");
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            listBox.BeginUpdate();
            try
            {
                for (int i = 0; i < 5000; i++)
                    listBox.Items.Add("Item " + GenItemIndex());
            }
            finally
            {
                listBox.EndUpdate();
            }
        }

        private static string IndicesToStr(IReadOnlyList<int> indices)
        {
            string result = indices.Count > 100 ?
                "too many indices to display" : string.Join(",", indices);
            return result;
        }

        private void ListBox_SelectionChanged(object? sender, EventArgs e)
        {
            string s = IndicesToStr(listBox.SelectedIndices);
            site?.LogEvent($"ListBox: SelectionChanged. SelectedIndices: ({s})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(
            object? sender, 
            EventArgs e)
        {
            listBox.Parent?.BeginUpdate();

            listBox.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ?
                ListBoxSelectionMode.Multiple : ListBoxSelectionMode.Single;

            listBox.Parent?.EndUpdate();
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            listBox.RemoveSelectedItems();
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            listBox.Items.Add("Item " + GenItemIndex());
        }

        private void EnsureLastItemVisibleButton_Click(
            object? sender, 
            EventArgs e)
        {
            var count = listBox.Items.Count;
            if (count > 0)
                listBox.EnsureVisible(count - 1);
        }

        private void SelectItemAtIndex2Button_Click(
            object? sender, 
            EventArgs e)
        {
            listBox.SelectItems(new int[] { 2 });
        }

        private void DeselectAllButton_Click(object? sender, EventArgs e)
        {
            listBox.SelectedItem = null;
        }

        private void SelectItemAtIndices2And4Button_Click(
            object? sender, 
            EventArgs e)
        {
            listBox.SelectItems(new int[] { 2, 4 });
        }
    }
}