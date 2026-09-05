using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSample
{
    public class DateTimeOther : Panel
    {
        private readonly ScrollablePanelSettings panel = new()
        {
        };

        private readonly PopupCalendar popupCalendar = new();

        public DateTimeOther()
        {
            panel.Parent = this;

            var propGrid = panel.ScrolledControl;

            propGrid.Padding = 10;

            propGrid.Add<BoldLabel>("DateTimePicker");
            propGrid.Add<DateTimePicker>((c) =>
            {
                c.Kind = DateTimePickerKind.DateTime;
            });
            propGrid.AddHorizontalLine();

            propGrid.Add<BoldLabel>("MonthPicker");
            propGrid.Add<MonthPicker>();
            propGrid.AddHorizontalLine();

            propGrid.Add<BoldLabel>("YearPicker");
            propGrid.Add<YearPicker>();
            propGrid.AddHorizontalLine();

            propGrid.Horizontal(c =>
            {
                c.Add<BoldLabel>("PopupCalendar").WithAlignment(VerticalAlignment.Center);

                var showPopupItem = propGrid.AddButton("Show");

                void ShowPopupCalendar()
                {
                    popupCalendar.ShowPopup(showPopupItem.Editor);
                }

                showPopupItem.SetEditorClick(ShowPopupCalendar);
            });

            propGrid.AddHorizontalLine();

            propGrid.Add<BoldLabel>("DayOfWeekPicker");
            propGrid.Add<DayOfWeekPicker>();
            propGrid.AddHorizontalLine();

            propGrid.Add<BoldLabel>("RelativeWeekdayOfMonthPicker");
            propGrid.Add<RelativeWeekdayOfMonthPicker>();
            propGrid.AddHorizontalLine();

            propGrid.Add<BoldLabel>("MonthAndDayPicker");
            propGrid.Add<MonthAndDayPicker>();
            propGrid.AddHorizontalLine();

            propGrid.Add<BoldLabel>("RelativeWeekdayPicker");
            propGrid.Add<RelativeWeekdayPicker>();
            propGrid.AddHorizontalLine();

            propGrid.Add<BoldLabel>("XCalendar.MonthPickerPanel");
            propGrid.Add<XCalendar.MonthPickerPanel>((c) =>
            {
            });
            propGrid.AddHorizontalLine();

            propGrid.Add<BoldLabel>("SpeedDateButton");
            propGrid.Add<SpeedDateButton>();
            propGrid.AddHorizontalLine();

            propGrid.Add<BoldLabel>("MonthSpeedButton");
            propGrid.Add<MonthSpeedButton>();
            propGrid.AddHorizontalLine();

            propGrid.Add<BoldLabel>("XCalendar.CalendarHeader");
            propGrid.Add<XCalendar.CalendarHeader>(c =>
            {
                c.IgnoreTransparency = true;
            });

            propGrid.AddHorizontalLine();

            popupCalendar.AfterHide += PopupListBox_AfterHide;
        }

        private void PopupListBox_AfterHide(object? sender, EventArgs e)
        {
            var resultItem = popupCalendar.MainControl.Value;
            App.Log($"AfterHide PopupResult: {popupCalendar.PopupResult}, Value: {resultItem}");
        }
    }
}

