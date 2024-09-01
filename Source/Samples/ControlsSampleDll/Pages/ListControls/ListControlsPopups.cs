﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    [IsTextLocalized(true)]
    internal class ListControlsPopups : Control
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

        private readonly CheckBox modalPopupsCheckBox = new()
        {
            Text = GenericStrings.ModalPopups,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly PopupListBox popupListBox = new();
        private readonly PopupCheckListBox popupCheckListBox = new();
        private readonly PopupColorListBox popupColorListBox = new();
        private readonly PopupListBox<VirtualListBox> popupVListBox = new();

        static ListControlsPopups()
        {
        }

        public ListControlsPopups()
        {
            Padding = 5;
            panel.Parent = this;

            Group(
                showPopupVListBoxButton,
                showPopupListBoxButton,
                showPopupCheckListBoxButton,
                showPopupColorListBoxButton,
                modalPopupsCheckBox).Parent(panel).SuggestedWidthToMax().Margin(10);

            modalPopupsCheckBox.BindBoolProp(this, nameof(ModalPopups));

            showPopupListBoxButton.Click += ShowPopupListBoxButton_Click;
            showPopupCheckListBoxButton.Click += ShowPopupCheckListBoxButton_Click;
            showPopupVListBoxButton.Click += ShowPopupVListBoxButton_Click;
            showPopupColorListBoxButton.Click += ShowPopupColorListBoxButton_Click;

            popupListBox.AfterHide += PopupListBox_AfterHide;
            popupCheckListBox.AfterHide += PopupCheckListBox_AfterHide;
            popupVListBox.AfterHide += PopupVListBox_AfterHide;
            popupColorListBox.AfterHide+= PopupColorListBox_AfterHide;

            // These events are handled only for logging purposes.
            // They are normally not needed in order to work with popup windows
            popupListBox.MainControl.MouseLeftButtonUp += PopupListBox_MouseLeftButtonUp;
            popupListBox.MainControl.MouseLeftButtonDown += PopupListBox_MouseLeftButtonDown;
            popupListBox.MainControl.SelectionChanged += PopupListBox_SelectionChanged;
            popupListBox.MainControl.Click += PopupListBox_Click;
            popupListBox.MainControl.MouseDoubleClick += PopupListBox_MouseDoubleClick;
        }

        public bool ModalPopups
        {
            get
            {
                return PopupWindow.ModalPopups;
            }

            set
            {
                PopupWindow.ModalPopups = value;
            }
        }

        private void PopupCheckListBox_AfterHide(object? sender, EventArgs e)
        {
            var r = popupCheckListBox.PopupResult;
            var ch = popupCheckListBox.MainControl.CheckedIndices.Count;
            App.Log($"CheckListBoxPopup AfterHide PopupResult: {r}, Checked: {ch}");
        }

        private void PopupListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupListBox.ResultItem ?? GenericStrings.NoneInsideLessGreater;
            App.Log($"AfterHide PopupResult: {popupListBox.PopupResult}, {GenericStrings.Item}: {resultItem}");
        }

        private void PopupVListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupVListBox.ResultItem ?? GenericStrings.NoneInsideLessGreater;
            App.Log($"AfterHide PopupResult: {popupVListBox.PopupResult}, {GenericStrings.Item}: {resultItem}");
        }

        private void PopupColorListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupColorListBox.ResultValue?.ToString() ?? GenericStrings.NoneInsideLessGreater;
            App.Log($"AfterHide PopupResult: {popupColorListBox.PopupResult}, {GenericStrings.Item}: {resultItem}");
        }

        internal void LogPopupListBoxEvent(string eventName)
        {
            var selectedItem = popupListBox.MainControl.SelectedItem ?? GenericStrings.NoneInsideLessGreater;
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

        private void AddDefaultItems(ListControl control)
        {
            GenericStrings.AddTenRows(ActionUtils.ToAction<string>(control.Add));
            control.Add(GenericStrings.ThisIsLongItemWhichOccupiesMoreSpaceThanOtherItems);
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
            if(popupColorListBox.MainControl.SelectedItem is null)
                popupColorListBox.MainControl.SelectFirstItem();
            popupColorListBox.ShowPopup(showPopupColorListBoxButton);
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

