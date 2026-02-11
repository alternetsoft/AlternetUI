using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    [IsTextLocalized(true)]
    internal class ListControlsPopups : Panel
    {
        internal bool logMouseEvents = false;

        private readonly VerticalStackPanel panel = new()
        {
            Padding = 10,
        };

        private readonly Button showPopupListBoxButton = new()
        {
            Text = $"{GenericStrings.ShowPopupWith} ListBox",
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly Button showPopupColorListBoxButton = new()
        {
            Text = $"{GenericStrings.ShowPopupWith} ColorListBox",
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly Button showPopupVListBoxButton = new()
        {
            Text = $"{GenericStrings.ShowPopupWith} VirtualListBox",
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly Button showPopupCheckListBoxButton = new()
        {
            Text = $"{GenericStrings.ShowPopupWith} CheckListBox",
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly PopupListBox popupListBox = new();
        private readonly PopupCheckListBox popupCheckListBox = new();
        private readonly PopupColorListBox popupColors = new();
        private readonly PopupListBox<VirtualListBox> popupVListBox = new();

        static ListControlsPopups()
        {
        }

        public ListControlsPopups()
        {
            Padding = 5;
            panel.Parent = this;

            var group = Group(
                showPopupVListBoxButton,
                showPopupListBoxButton,
                showPopupCheckListBoxButton,
                showPopupColorListBoxButton).Parent(panel).Margin(10).MaxWidthOnSizeChanged();

            showPopupListBoxButton.Click += ShowPopupListBoxButton_Click;
            showPopupCheckListBoxButton.Click += ShowPopupCheckListBoxButton_Click;
            showPopupVListBoxButton.Click += ShowPopupVListBoxButton_Click;
            showPopupColorListBoxButton.Click += ShowPopupColorListBoxButton_Click;

            popupListBox.AfterHide += PopupListBox_AfterHide;
            popupCheckListBox.AfterHide += PopupCheckListBox_AfterHide;
            popupVListBox.AfterHide += PopupVListBox_AfterHide;
            popupColors.AfterHide+= PopupColorListBox_AfterHide;

            // These events are handled only for logging purposes.
            // They are normally not needed in order to work with popup windows
            popupListBox.MainControl.MouseLeftButtonUp += PopupListBox_MouseLeftButtonUp;
            popupListBox.MainControl.MouseLeftButtonDown += PopupListBox_MouseLeftButtonDown;
            popupListBox.MainControl.SelectionChanged += PopupListBox_SelectionChanged;
            popupListBox.MainControl.Click += PopupListBox_Click;
            popupListBox.MainControl.MouseDoubleClick += PopupListBox_MouseDoubleClick;
        }

        private void PopupCheckListBox_AfterHide(object? sender, EventArgs e)
        {
            var r = popupCheckListBox.PopupResult;
            var ch = popupCheckListBox.MainControl.CheckedIndices.Count;
            App.Log($"CheckListBoxPopup AfterHide PopupResult: {r}, Checked: {ch}");
        }

        private void PopupListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupListBox.ResultItem?.Text ?? GenericStrings.NoneInsideLessGreater;
            App.Log($"PopupResult: {popupListBox.PopupResult}, {GenericStrings.Item}: {resultItem}");
        }

        private void PopupVListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupVListBox.ResultItem?.Text ?? GenericStrings.NoneInsideLessGreater;
            App.Log($"PopupResult: {popupVListBox.PopupResult}, {GenericStrings.Item}: {resultItem}");
        }

        private void PopupColorListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupColors.ResultValue?.ToString()
                ?? GenericStrings.NoneInsideLessGreater;
            App.Log($"PopupResult: {popupColors.PopupResult}, {GenericStrings.Item}: {resultItem}");
        }

        internal void LogPopupListBoxEvent(string eventName)
        {
            var selectedItem = popupListBox.MainControl.SelectedItem?.ToString()
                ?? GenericStrings.NoneInsideLessGreater;
            App.Log($"Popup: {eventName}. {GenericStrings.SelectedItem}: {selectedItem}");
        }

        internal void LogPopupListBoxMouseEvent(string eventName, MouseEventArgs e)
        {
            var itemIndex = popupListBox.MainControl.HitTest(
                Mouse.GetPosition(popupListBox.MainControl));
            var selectedItem = popupListBox.MainControl[itemIndex];
            App.Log($"Popup: {eventName}. {GenericStrings.Item}: {selectedItem}");
        }

        private void PopupListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(logMouseEvents)
                LogPopupListBoxEvent("DoubleClick");
        }

        private void PopupListBox_SelectionChanged(object? sender, EventArgs e)
        {
        }

        private void PopupListBox_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (logMouseEvents)
                LogPopupListBoxMouseEvent("MouseLeftButtonUp", e);
        }

        private void PopupListBox_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (logMouseEvents)
                LogPopupListBoxMouseEvent("MouseLeftButtonDown", e);
        }

        private void PopupListBox_Click(object? sender, EventArgs e)
        {
            if (logMouseEvents)
                LogPopupListBoxEvent("Click");
        }

        private void AddDefaultItems(VirtualListBox control)
        {
            GenericStrings.AddTenRows((s) =>
            {
                control.Add(new ListControlItem(s));
            });
            control.Add(new ListControlItem(
                GenericStrings.ThisIsLongItemWhichOccupiesMoreSpaceThanOtherItems));
        }

        private void ShowPopupListBoxButton_Click(object? sender, EventArgs e)
        {
            if (popupListBox.MainControl.Items.Count == 0)
            {
                AddDefaultItems(popupListBox.MainControl);
                popupListBox.MainControl.SelectFirstItem();
            }
            popupListBox.ShowPopup(showPopupListBoxButton);
        }

        private void ShowPopupVListBoxButton_Click(object? sender, EventArgs e)
        {
            if (popupVListBox.MainControl.Items.Count == 0)
            {
                PropertyGridSample.ObjectInit.InitVListBox(popupVListBox.MainControl);
                popupVListBox.MainControl.SelectFirstItem();
            }
            popupVListBox.ShowPopup(showPopupVListBoxButton);
        }

        private void ShowPopupColorListBoxButton_Click(object? sender, EventArgs e)
        {
            if(popupColors.MainControl.SelectedItem is null)
                popupColors.MainControl.SelectFirstItem();
            popupColors.ShowPopup(showPopupColorListBoxButton);
        }

        private void ShowPopupCheckListBoxButton_Click(object? sender, EventArgs e)
        {
            var control = popupCheckListBox.MainControl;

            if (control.Items.Count == 0)
            {
                AddDefaultItems(control);
                control.SelectFirstItem();
            }
            popupCheckListBox.ShowPopup(showPopupCheckListBoxButton);
        }
    }
}

