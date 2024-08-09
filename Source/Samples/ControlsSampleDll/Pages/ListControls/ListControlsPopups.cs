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

        private readonly Button showPopupColorListBoxButton = new()
        {
            Text = "Show Popup ColorListBox",
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        private readonly Button showPopupVListBoxButton = new()
        {
            Text = "Show Popup Virtual ListBox",
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
        private readonly PopupColorListBox popupColorListBox = new();
        private readonly PopupListBox<VirtualListBox> popupVListBox = new();

        static ListControlsPopups()
        {
        }

        public ListControlsPopups()
            : base()
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
            var resultItem = popupListBox.ResultItem ?? "<null>";
            App.Log($"AfterHide PopupResult: {popupListBox.PopupResult}, Item: {resultItem}");
        }

        private void PopupVListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupVListBox.ResultItem ?? "<null>";
            App.Log($"AfterHide PopupResult: {popupVListBox.PopupResult}, Item: {resultItem}");
        }

        private void PopupColorListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupColorListBox.ResultValue?.ToString() ?? "<null>";
            App.Log($"AfterHide PopupResult: {popupColorListBox.PopupResult}, Item: {resultItem}");
        }

        internal void LogPopupListBoxEvent(string eventName)
        {
            var selectedItem = popupListBox.MainControl.SelectedItem ?? "<null>";
            App.Log($"Popup: {eventName}. Selected Item: {selectedItem}");
        }

        internal void LogPopupListBoxMouseEvent(string eventName, MouseEventArgs e)
        {
            var itemIndex = popupListBox.MainControl.HitTest(
                Mouse.GetPosition(popupListBox.MainControl));
            var selectedItem = popupListBox.MainControl[itemIndex];
            App.Log($"Popup: {eventName}. Item: {selectedItem}");
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

