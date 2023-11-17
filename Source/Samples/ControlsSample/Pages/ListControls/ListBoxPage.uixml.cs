using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal partial class ListBoxPage : Control
    {
        private readonly CardPanelHeader panelHeader = new();
        private readonly PopupListBox popupListBox = new();
        private readonly CardPanel cardPanel = new();
        private IPageSite? site;
        private int newItemIndex = 0;

        public ListBoxPage()
        {
            InitializeComponent();

            cardPanel.Visible = false;
            cardPanel.Parent = tabControl;
            var checkListBoxCardIndex = cardPanel.Add(null, CreateCheckListBoxPage);
            var comboBoxCardIndex = cardPanel.Add(null, CreateComboBoxPage);

            panelHeader.CardPanel = cardPanel;
            panelHeader.Add("ListBox", tab1);
            panelHeader.Add("CheckListBox", cardPanel[checkListBoxCardIndex].UniqueId);
            panelHeader.Add("ComboBox", cardPanel[comboBoxCardIndex].UniqueId);
            panelHeader.Add("Popups", tab2);
            tabControl.Children.Prepend(panelHeader);
            panelHeader.SelectFirstTab();

            showPopupButton.Click += ShowPopupButton_Click;

            popupListBox.MainControl.MouseLeftButtonUp += PopupListBox_MouseLeftButtonUp;
            popupListBox.MainControl.MouseLeftButtonDown += PopupListBox_MouseLeftButtonDown;
            popupListBox.MainControl.SelectionChanged += PopupListBox_SelectionChanged;
            popupListBox.MainControl.Click += PopupListBox_Click;
            popupListBox.MainControl.MouseDoubleClick += PopupListBox_MouseDoubleClick;
            popupListBox.VisibleChanged += PopupListBox_VisibleChanged;

            findExactCheckBox.BindBoolProp(this, nameof(FindExact));
            findIgnoreCaseCheckBox.BindBoolProp(this, nameof(FindIgnoreCase));
            findText.TextChanged += FindText_TextChanged;
        }

        private Control CreateCheckListBoxPage()
        {
            return new CheckListBoxPage() { Site = site };
        }

        private Control CreateComboBoxPage()
        {
            return new ComboBoxPage() { Site = site };
        }

        private void FindText_TextChanged(object sender, TextChangedEventArgs e)
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

        internal void LogPopupListBoxEvent(string eventName)
        {
            var selectedItem = popupListBox.MainControl.SelectedItem ?? "<null>";
            site?.LogEvent($"Popup: {eventName}. Selected Item: {selectedItem}");
        }

        internal void LogPopupListBoxMouseEvent(string eventName, MouseButtonEventArgs e)
        {
            var itemIndex = popupListBox.MainControl.HitTest(e.GetPosition(popupListBox.MainControl));
            var selectedItem = popupListBox.MainControl.Items[itemIndex];
            site?.LogEvent($"Popup: {eventName}. Item: {selectedItem}");
        }

        private void PopupListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // LogPopupListBoxEvent("DoubleClick");
        }

        private void PopupListBox_VisibleChanged(object? sender, EventArgs e)
        {
            if (popupListBox.Visible)
                return;
            var resultItem = popupListBox.MainControl[popupListBox.ResultIndex] ?? "<null>";
            site?.LogEvent($"PopupResult: {popupListBox.PopupResult}, Item: {resultItem}");
        }

        private void PopupListBox_SelectionChanged(object? sender, EventArgs e)
        {
        }

        private void PopupListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // LogPopupListBoxMouseEvent("MouseLeftButtonUp", e);
        }

        private void PopupListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // LogPopupListBoxMouseEvent("MouseLeftButtonDown", e);
        }

        private void PopupListBox_Click(object? sender, EventArgs e)
        {
            // LogPopupListBoxEvent("Click");
        }

        private void ShowPopupButton_Click(object? sender, EventArgs e)
        {
            if (popupListBox.MainControl.Items.Count == 0)
            {
                popupListBox.MainControl.SuggestedSize = new Size(150, 300);
                AddDefaultItems(popupListBox.MainControl);
                popupListBox.MainControl.SelectFirstItem();
            }
            site?.LogEvent(" === ShowPopupButton_Click ===");
            popupListBox.ShowPopup(showPopupButton);            
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                AddDefaultItems(listBox);
                site = value;
            }
        }

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