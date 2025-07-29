using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class VListBoxSamplePage: Panel
    {
        private readonly NotNullCollection<ListControlItem> items1 = new();
        private readonly NotNullCollection<ListControlItem> items2 = new();
        private bool useItems1 = true;

        private readonly VirtualListBox listBox = new()
        {
            SuggestedWidth = 350,
            Margin = (0,0,0,5),
        };

        public VListBoxSamplePage()
        {
            PropertyGridSample.ObjectInit.AddDefaultOwnerDrawItems(listBox, (s) =>
            {
                items1.Add(s);
            });

            for (int i = 0; i < 1000; i++)
            {
                items2.Add(new ListControlItem($"Items2.Item {i}"));
            }

            InitializeComponent();
            Title = "Virtual";

            findExactCheckBox.BindBoolProp(this, nameof(FindExact));
            findIgnoreCaseCheckBox.BindBoolProp(this, nameof(FindIgnoreCase));
            findText.TextChanged += FindText_TextChanged;
            PropertyGridSample.ObjectInit.InitVListBox(listBox);

            Children.Prepend(listBox);
            listBox.SelectionChanged += ListBox_SelectionChanged;
            listBox.MouseLeftButtonDown += ListBox_MouseLeftButtonDown;
            listBox.Search.UseContains = true;
            listBox.HandleCreated += ListBox_HandleCreated;
            roundSelectionCheckBox.CheckedChanged += RoundSelectionCheckBox_CheckedChanged;
            showCheckBoxesCheckBox.CheckedChanged += ShowCheckBoxesCheckBox_CheckedChanged;
            threeStateCheckBox.BindBoolProp(listBox, nameof(VirtualListBox.CheckBoxThreeState));
            allowAllStatesCheckBox.BindBoolProp(
                listBox,
                nameof(VirtualListBox.CheckBoxAllowAllStatesForUser));
            allowClickCheckCheckBox.BindBoolProp(listBox, nameof(VirtualListBox.CheckOnClick));

            SetSizeToContent();
            listBox.CheckedChanged += ListBox_CheckedChanged;

            otherThemeCheckBox.CheckedChanged += (s, e) =>
            {
                if (otherThemeCheckBox.IsChecked)
                    LightDarkColor.IsDarkOverride = !IsDarkBackground;
                else
                    LightDarkColor.IsDarkOverride = null;
                listBox.Invalidate();
            };

            var contextMenu = new ContextMenuStrip();
            vertPanel2.ContextMenuStrip = contextMenu;

            contextMenu.Add("Toggle items fast", () =>
            {
                if (useItems1)
                {
                    listBox.SetItemsFast(items2, VirtualListBox.SetItemsKind.ChangeField);
                }
                else
                {
                    listBox.SetItemsFast(items1, VirtualListBox.SetItemsKind.ChangeField);
                }

                useItems1 = !useItems1;
            });

            contextMenu.Add("Toggle Draw Debug Corners", () =>
            {
                ListControlItem.DrawDebugCornersOnElements = !ListControlItem.DrawDebugCornersOnElements;
                listBox.Invalidate();
            });

            contextMenu.Add("Next item alignment", () =>
            {
                var item = listBox.GetItem(listBox.SelectedIndex ?? 0);
                if (item is null)
                    return;
                item.Text ??= "Item 0 Text";
                item.DisplayText = null;
                item.MinHeight = 70;
                item.Alignment = item.Alignment.NextValue();
                App.LogNameValueReplace("VListBox.Items[0].Alignment", item.Alignment);
                listBox.Invalidate();
            });

            listBox.HasBorder = VirtualListBox.DefaultUseInternalScrollBars;
        }

        private void ListBox_CheckedChanged(object? sender, EventArgs e)
        {
            string checkedIndicesString = IndexesToStr(listBox.CheckedIndices);
            App.Log(
                $"ListBox.CheckedChanged: ({checkedIndicesString})");
        }

        private void ShowCheckBoxesCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            listBox.CheckBoxVisible = showCheckBoxesCheckBox.IsChecked;
        }

        private void RoundSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (roundSelectionCheckBox.IsChecked)
            {
                BorderSettings border = new();
                border.UniformRadiusIsPercent = false;
                border.UniformCornerRadius = 10;

                listBox.CurrentItemBorder = border;
                listBox.SelectionBorder = border;
            }
            else
            {
                listBox.CurrentItemBorder = null;
                listBox.SelectionBorder = null;
            }
        }

        private void ListBox_HandleCreated(object? sender, EventArgs e)
        {
            App.LogIf("ListBox.HandleCreated", false);
        }

        private void FindText_TextChanged(object? sender, EventArgs e)
        {
            listBox.FindAndSelect(findText.Text, null, FindExact, FindIgnoreCase);
        }

        public bool FindExact { get; set; } = false;

        public bool FindIgnoreCase { get; set; } = true;

        private void EditorButton_Click(object? sender, System.EventArgs e)
        {
            DialogFactory.EditItemsWithListEditor(listBox);
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
            
            var item = (result == null ? "<none>" : listBox.GetItem(result.Value)?.ToString());

            item ??= result?.ToString() ?? string.Empty;

            var splitted = StringUtils.Split(item);

            if(splitted.Length > 1)
                item = $"{splitted[0]}...";

            App.Log($"HitTest result: Item: '{item}'");
        }

        private static string IndexesToStr(IReadOnlyList<int>? indexes)
        {
            if (indexes is null)
                return string.Empty;
            string result = indexes.Count > 50 ?
                "too many indexes to display" : string.Join(",", indexes);
            return result;
        }

        private void ListBox_SelectionChanged(object? sender, EventArgs e)
        {
            string s = IndexesToStr(listBox.SelectedIndexes);
            App.Log($"ListBox: SelectionChanged. SelectedIndexes: ({s})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            listBox.Parent?.BeginUpdate();

            var b = allowMultipleSelectionCheckBox.IsChecked;

            listBox.SelectionMode = b ? ListBoxSelectionMode.Multiple : ListBoxSelectionMode.Single;

            selectItemAtIndices2And4Button.Enabled = b;

            listBox.Parent?.EndUpdate();
        }

        private void EnsureLastItemVisibleButton_Click(
            object? sender, 
            EventArgs e)
        {
            // do not need to check count as EnsureVisible does this.
            listBox.EnsureVisible(listBox.Count - 1);
        }

        private void SelectItemAtIndex2Button_Click(
            object? sender, 
            EventArgs e)
        {
            listBox.SelectItems(2);
        }

        private void DeselectAllButton_Click(object? sender, EventArgs e)
        {
            listBox.SelectedItem = null;
        }

        private void SelectItemAtIndices2And4Button_Click(
            object? sender, 
            EventArgs e)
        {
            listBox.SelectItems(2, 4);
        }
    }
}