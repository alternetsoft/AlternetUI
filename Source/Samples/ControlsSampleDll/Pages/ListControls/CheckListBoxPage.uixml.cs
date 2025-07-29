using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    [IsCsLocalized(true)]
    internal partial class CheckListBoxPage : Panel
    {
        public CheckListBoxPage()
        {
            InitializeComponent();

            void AddSection(string title)
            {
                ListControlItem section1 = new(title);
                section1.IsBold = true;
                section1.Font = RealFont.IncSize(2);
                section1.CheckBoxVisible = false;
                checkListBox.BaseItems.Add(section1);
            }

            void AddItems()
            {
                for (int i = 0; i < 10; i++)
                {
                    checkListBox.Items.Add("Item " + LogUtils.GenNewId());
                }
            }

            AddSection("Section 1");
            AddItems();

            AddSection("Section 2");

            var item1 = new ListControlItemWithNotify("Radio Button 1");
            item1.IsChecked = true;
            var item2 = new ListControlItemWithNotify("Radio Button 2");

            item1.IsRadioButton = true;
            item2.IsRadioButton = true;

            item1.Group = new();
            item2.Group = item1.Group;

            item1.PropertyChanged += RadioButtonItemChanged;
            item2.PropertyChanged += RadioButtonItemChanged;

            checkListBox.Add(item1);
            checkListBox.Add(item2);

            AddSection("Section 3");
            AddItems();

            checkListBox.SelectionChanged += CheckListBox_SelectionChanged;
            allowMultipleSelectionCheckBox.IsChecked =
                checkListBox.SelectionMode == ListBoxSelectionMode.Multiple;
            App.LogIf($"CheckListBoxDemo: Constructor done", false);

            checkListBox.HasBorder = VirtualListBox.DefaultUseInternalScrollBars;
        }

        private void RadioButtonItemChanged(
            object? sender, 
            PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CheckState")
            {
                var item = (ListControlItemWithNotify)sender!;

                if (item.IsChecked)
                {
                    ListControlItem.UncheckOtherItemsInGroup(checkListBox.BaseItems, item);
                    checkListBox.Refresh();
                }
            }
        }

        private void EditorButton_Click(object? sender, System.EventArgs e)
        {
            checkListBox.EditItemsWithListEditor();
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

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            ListBoxPage.AddManyItems(checkListBox);
        }

        private static string IndicesToStr(IReadOnlyList<int> indices)
        {
            string result = indices.Count > 100 ? 
                GenericStrings.TooManyIndexesToDisplay : string.Join(",", indices);
            return result;
        }

        private void CheckListBox_SizeChanged(object? sender, EventArgs e)
        {
            App.LogIf(
                $"CheckListBox: SizeChanged. InUpdates: {checkListBox.InUpdates}, ClientSize: ({checkListBox.ClientSize}), Size: ({checkListBox.Size})", false);
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
            checkListBox.IsSelectionModeMultiple = allowMultipleSelectionCheckBox.IsChecked;
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
            checkListBox.Items.Add(ListBoxPage.GenItemText());
            checkListBox.SelectLastItemAndScroll();
        }

        private void EnsureLastItemVisibleButton_Click(
            object? sender, 
            EventArgs e)
        {
            checkListBox.EnsureVisible(checkListBox.Count - 1);
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