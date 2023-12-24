using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class ListBoxPage : Control
    {
        private int newItemIndex = 0;

        public ListBoxPage()
        {
            InitializeComponent();

            findExactCheckBox.BindBoolProp(this, nameof(FindExact));
            findIgnoreCaseCheckBox.BindBoolProp(this, nameof(FindIgnoreCase));
            findText.TextChanged += FindText_TextChanged;
            AddDefaultItems(listBox);
        }

        private void FindText_TextChanged(object? sender, EventArgs e)
        {
            var text = findText.Text;
            if(text is null)
            {
                listBox.SelectedIndex = null;
                return;
            }
            var result = listBox.FindStringEx(text, null, FindExact, FindIgnoreCase);
            listBox.SelectedIndex = result;
        }

        public bool FindExact { get; set; } = false;

        public bool FindIgnoreCase { get; set; } = true;

        private void AddDefaultItems(ListBox control)
        {
            control.Add("One");
            control.Add("Two");
            control.Add("Three");
            control.Add("Four");
            control.Add("Five");
            control.Add("Six");
            control.Add("Seven");
            control.Add("Eight");
            control.Add("Nine");
            control.Add("Ten");
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
            MouseEventArgs e)
        {
            var result = listBox.HitTest(e.GetPosition(listBox));
            var item = (result == null ? "<none>" : listBox.Items[result.Value]);

            Application.Log($"HitTest result: Item: '{item}'");
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
            Application.Log($"ListBox: SelectionChanged. SelectedIndices: ({s})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(
            object? sender, 
            EventArgs e)
        {
            listBox.Parent?.BeginUpdate();

            var b = allowMultipleSelectionCheckBox.IsChecked;

            listBox.SelectionMode = b ? ListBoxSelectionMode.Multiple : ListBoxSelectionMode.Single;

            selectItemAtIndices2And4Button.Enabled = b;

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
            listBox.SelectItems([2]);
        }

        private void DeselectAllButton_Click(object? sender, EventArgs e)
        {
            listBox.SelectedItem = null;
        }

        private void SelectItemAtIndices2And4Button_Click(
            object? sender, 
            EventArgs e)
        {
            listBox.SelectItems([2, 4]);
        }
    }
}