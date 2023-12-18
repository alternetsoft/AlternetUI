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
        internal string workInProgress = "Work in progress...";

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

        public RectD GetPossiblePopupRect()
        {
            return RectD.Empty;
        }

        /*
        $procs.getPossiblePopupRects=function(e,width,height){
            var 
                screen = $procs.getScreenRect(), 
                target = $procs.getBoundingClientRect(e),

                nw = {
                    right:target.right,
                    bottom:target.top,
                    left:target.right-width,
                    top:target.top-height
                },		
                ne = {
                    left:target.left,
                    bottom:target.top,
                    right:screen.right,
                    top:target.top-height			
                },
                sw = {
                    right:target.right,
                    top:target.bottom,
                    left:target.right-width,
                    bottom:screen.bottom
                },
                se = {
                    left:target.left,
                    top:target.bottom,
                    right:screen.right,
                    bottom:screen.bottom
                };

            return { 
                ne:ne,
                nw:nw,
                sw:sw,
                se:se,
                items:[se,sw,ne,nw]
            };
        }
         */
        public ListControlsPopups()
            : base()
        {
            Padding = 5;
            panel.Parent = this;
            showPopupListBoxButton.Parent = panel;
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

        internal void LogPopupListBoxMouseEvent(string eventName, MouseEventArgs e)
        {
            var itemIndex = popupListBox.MainControl.HitTest(e.GetPosition(popupListBox.MainControl));
            var selectedItem = popupListBox.MainControl.Items[itemIndex];
            Application.Log($"Popup: {eventName}. Item: {selectedItem}");
        }

        private void PopupListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(logMouseEvents)
                LogPopupListBoxEvent("DoubleClick");
        }

        private void PopupListBox_VisibleChanged(object? sender, EventArgs e)
        {
            if (popupListBox.Visible)
                return;
            var resultItem = popupListBox.ResultItem ?? "<null>";
            Application.Log($"PopupResult: {popupListBox.PopupResult}, Item: {resultItem}");
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
        }

        private void ShowPopupListBoxButton_Click(object? sender, EventArgs e)
        {
            var useDo = true;

            if (useDo)
            {
                Do();
                return;
            }

            var posDip = showPopupListBoxButton.ClientToScreen((0, 0));
            var pos = PixelFromDip(posDip);
            var szDip = showPopupListBoxButton.Size;
            var sz = PixelFromDip(szDip);

            sz = (0, sz.Height);

            Control.TestPopupWindow(this, pos , sz);

            void Do()
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
}

