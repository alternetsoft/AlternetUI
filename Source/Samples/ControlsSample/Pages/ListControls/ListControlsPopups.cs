using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal class ListControlsPopups : Control
    {
        internal bool logMouseEvents = false;

        private readonly VerticalStackPanel panel = new()
        {
            Padding = 10,
        };

        private readonly Button showPopupListBoxButton = new()
        {
            Text = "Show Popup ListBox",
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly Button showPopupCheckListBoxButton = new()
        {
            Text = "Show Popup CheckListBox",
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly CheckBox modalPopupsCheckBox = new()
        {
            Text = "Modal Popups",
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly PopupListBox popupListBox = new();
        private readonly PopupCheckListBox popupCheckListBox = new();

        static ListControlsPopups()
        {
        }

        public ListControlsPopups()
            : base()
        {
            Padding = 5;
            panel.Parent = this;

            showPopupListBoxButton.Parent = panel;
            showPopupCheckListBoxButton.Parent = panel;
            modalPopupsCheckBox.Parent = panel;
            modalPopupsCheckBox.BindBoolProp(this, nameof(ModalPopups));

            panel.ChildrenSet.Margin(10);

            showPopupListBoxButton.Click += ShowPopupListBoxButton_Click;
            showPopupCheckListBoxButton.Click += ShowPopupCheckListBoxButton_Click;

            popupListBox.AfterHide += PopupListBox_AfterHide;
            popupCheckListBox.AfterHide += PopupCheckListBox_AfterHide;

            // These events are handled only for logging purposes.
            // They are normally not needed in order to work with popup windows
            popupListBox.MainControl.MouseLeftButtonUp += PopupListBox_MouseLeftButtonUp;
            popupListBox.MainControl.MouseLeftButtonDown += PopupListBox_MouseLeftButtonDown;
            popupListBox.MainControl.SelectionChanged += PopupListBox_SelectionChanged;
            popupListBox.MainControl.Click += PopupListBox_Click;
            popupListBox.MainControl.MouseDoubleClick += PopupListBox_MouseDoubleClick;

            Group(showPopupListBoxButton, showPopupCheckListBoxButton).SuggestedWidthToMax();

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
            Application.Log($"CheckListBoxPopup AfterHide PopupResult: {r}, Checked: {ch}");
        }

        private void PopupListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupListBox.ResultItem ?? "<null>";
            Application.Log($"AfterHide PopupResult: {popupListBox.PopupResult}, Item: {resultItem}");
        }

        internal void LogPopupListBoxEvent(string eventName)
        {
            var selectedItem = popupListBox.MainControl.SelectedItem ?? "<null>";
            Application.Log($"Popup: {eventName}. Selected Item: {selectedItem}");
        }

        internal void LogPopupListBoxMouseEvent(string eventName, MouseEventArgs e)
        {
            var itemIndex = popupListBox.MainControl.HitTest(
                e.GetPosition(popupListBox.MainControl));
            var selectedItem = popupListBox.MainControl.Items[itemIndex];
            Application.Log($"Popup: {eventName}. Item: {selectedItem}");
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
            control.Add("This is long item which occupies more space than other items");
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

