using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class ListBoxPage : Panel
    {
        private static int newItemIndex = 0;

        public ListBoxPage()
        {
            InitializeComponent();

            findExactCheckBox.BindBoolProp(this, nameof(FindExact));
            findIgnoreCaseCheckBox.BindBoolProp(this, nameof(FindIgnoreCase));
            findText.TextChanged += FindText_TextChanged;
            AddDefaultItems(listBox);
            listBox.Search.UseContains = true;
            listBox.HasBorder = VirtualListBox.DefaultUseInternalScrollBars;
        }

        private void FindText_TextChanged(object? sender, EventArgs e)
        {
            listBox.FindAndSelect(findText.Text, null, FindExact, FindIgnoreCase);
        }

        public bool FindExact { get; set; } = false;

        public bool FindIgnoreCase { get; set; } = true;

        private void AddDefaultItems(ListBox control)
        {
            GenericStrings.AddTenRows(ActionUtils.ToAction<string>(control.Add));
        }

        private void EditorButton_Click(object? sender, System.EventArgs e)
        {
            listBox.EditItemsWithListEditor();
        }

        public static int GenItemIndex()
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
            var result = listBox.HitTest(Mouse.GetPosition(listBox));
            var item = (result == null ? "<none>" : listBox.Items[result.Value]);

            App.Log($"HitTest result: Item: '{item}'");
        }

        public static string GenItemText()
        {
            return $"{GenericStrings.Item} id({GenItemIndex()})";
        }

        public static void AddManyItems(ListBox listBox)
        {
            listBox.BeginUpdate();
            try
            {
                string[] data = new string[5000];

                var length = data.Length;

                for (int i = 0; i < length; i++)
                    data[i] = GenItemText();

                listBox.Items.AddRange(data);

                App.Log("Added 5000 items");
            }
            finally
            {
                listBox.EndUpdate();
            }

            listBox.SelectLastItemAndScroll();
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            AddManyItems(listBox);
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
            App.Log($"ListBox: SelectionChanged. SelectedIndices: ({s})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(
            object? sender, 
            EventArgs e)
        {
            var b = allowMultipleSelectionCheckBox.IsChecked;
            listBox.IsSelectionModeMultiple = b;
            selectItemAtIndices2And4Button.Enabled = b;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            listBox.RemoveSelectedAndUpdateSelection();
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            listBox.Items.Add(GenItemText());
            listBox.SelectLastItemAndScroll();
        }

        private void EnsureLastItemVisibleButton_Click(
            object? sender, 
            EventArgs e)
        {
            listBox.EnsureVisible(listBox.Count - 1);
        }

        private void SelectItemAtIndex2Button_Click(
            object? sender, 
            EventArgs e)
        {
            listBox.SelectItemsAndScroll(2);
        }

        private void DeselectAllButton_Click(object? sender, EventArgs e)
        {
            listBox.ClearSelected();
        }

        private void SelectItemAtIndices2And4Button_Click(
            object? sender, 
            EventArgs e)
        {
            listBox.SelectItemsAndScroll(2, 4);
        }
    }
}