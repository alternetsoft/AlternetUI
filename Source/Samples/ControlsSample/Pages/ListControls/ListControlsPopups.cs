using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class ListControlsPopups : Control
    {
        internal string workInProgress = "Work in progress. Currently not everything works fine in the popups...";

        private readonly VerticalStackPanel panel = new()
        {
            Padding = 10,
        };

        private readonly Button showPopupListBoxButton = new()
        {
            Text = "Show Popup ListBox",
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly PopupListBox popupListBox = new();


        public ListControlsPopups()
            : base()
        {
            Padding = 5;
            panel.Parent = this;
            showPopupListBoxButton.Parent = this;
            showPopupListBoxButton.Click += ShowPopupListBoxButton_Click;

            popupListBox.MainControl.MouseLeftButtonUp += PopupListBox_MouseLeftButtonUp;
            popupListBox.MainControl.MouseLeftButtonDown += PopupListBox_MouseLeftButtonDown;
            popupListBox.MainControl.SelectionChanged += PopupListBox_SelectionChanged;
            popupListBox.MainControl.Click += PopupListBox_Click;
            popupListBox.MainControl.MouseDoubleClick += PopupListBox_MouseDoubleClick;
            popupListBox.VisibleChanged += PopupListBox_VisibleChanged;

        }

        internal void LogPopupListBoxEvent(string eventName)
        {
            var selectedItem = popupListBox.MainControl.SelectedItem ?? "<null>";
            Application.Log($"Popup: {eventName}. Selected Item: {selectedItem}");
        }

        internal void LogPopupListBoxMouseEvent(string eventName, MouseButtonEventArgs e)
        {
            var itemIndex = popupListBox.MainControl.HitTest(e.GetPosition(popupListBox.MainControl));
            var selectedItem = popupListBox.MainControl.Items[itemIndex];
            Application.Log($"Popup: {eventName}. Item: {selectedItem}");
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
            Application.Log($"PopupResult: {popupListBox.PopupResult}, Item: {resultItem}");
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

        private void AddDefaultItems(ListControl control)
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

        private void ShowPopupListBoxButton_Click(object? sender, EventArgs e)
        {
            if (popupListBox.MainControl.Items.Count == 0)
            {
                popupListBox.MainControl.SuggestedSize = new(150, 300);
                AddDefaultItems(popupListBox.MainControl);
                popupListBox.MainControl.SelectFirstItem();
            }
            Application.Log(" === ShowPopupButton_Click ===");
            popupListBox.ShowPopup(showPopupListBoxButton);
        }
    }
}

