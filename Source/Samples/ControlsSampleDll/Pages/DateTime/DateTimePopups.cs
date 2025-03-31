using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    internal class DateTimePopups : Panel
    {
        private readonly VerticalStackPanel panel = new()
        {
            Padding = 10,
        };

        [IsTextLocalized(true)]
        private readonly Button showPopupButton = new("Show Calendar Popup");

        [IsTextLocalized(true)]
        private readonly CheckBox UseGenericCheckBox = new("Use Generic");

        private readonly PopupCalendar popupCalendar = new();

        public DateTimePopups()
        {
            Padding = 5;
            panel.Parent = this;

            showPopupButton.Parent = panel;
            UseGenericCheckBox.Visible = false;
            UseGenericCheckBox.Parent = panel;
            UseGenericCheckBox.BindBoolProp(this, nameof(UseGeneric));

            panel.ChildrenSet.Margin(10).HorizontalAlignment(HorizontalAlignment.Left);

            showPopupButton.Click += ShowPopupButton_Click;

            popupCalendar.AfterHide += PopupListBox_AfterHide;
        }

        public bool UseGeneric
        {
            get
            {
                return popupCalendar.MainControl.UseGeneric;
            }

            set
            {
                popupCalendar.MainControl.UseGeneric = value;
                popupCalendar.SetClientSizeTo();
            }
        }

        private void PopupListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupCalendar.MainControl.Value;
            App.Log($"AfterHide PopupResult: {popupCalendar.PopupResult}, Value: {resultItem}");
        }

        private void ShowPopupButton_Click(object? sender, EventArgs e)
        {
            popupCalendar.ShowPopup(showPopupButton);
        }
    }
}

