using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class DateTimePage : Panel
    {
        private string?[] dateFormats = new[]
        {
            null,           // Default format
            "M/d/yyyy",     // 4/5/2017
            "M/d/yy",       // 4/5/17
            "MM/dd/yy",     // 04/05/17
            "MM/dd/yyyy",   // 04/05/2017
            "yy/MM/dd",     // 17/04/05
            "yyyy-MM-dd",   // 2017-04-05
            "dd-MMM-yy",     // 05-Apr-17,
        };

        private ContextMenu dateFormatContextMenu;

        public DateTimePage()
        {
            InitializeComponent();

            tabControl2.MinSizeGrowMode = WindowSizeToContentMode.Width;

            datePicker.Value = DateTime.Now;
            timePicker.Value = DateTime.Now;
            
            dateFormatContextMenu = CreateDateFormatContextMenu();
            buttonDateFormats.DropDownMenu = dateFormatContextMenu;

            buttonClearDate.Click += (s, e) =>
            {
                datePicker.Value = null;
            };
        }

        public ContextMenu CreateDateFormatContextMenu()
        {
            var contextMenu = new ContextMenu();
            foreach (var format in dateFormats)
            {
                var menuItem = new MenuItem();
                menuItem.Text = format is null
                    ? "Default" : DateTime.Now.ToString(format);
                menuItem.Tag = format;
                menuItem.Click += (s, e) =>
                {
                    datePicker.Format = format;
                };
                contextMenu.Items.Add(menuItem);
            }
            return contextMenu;
        }

        private void DatePicker_DateChanged(object? sender, EventArgs e)
        {
            dateLabel.Text = $"Selected: {datePicker.Text}";
        }

        private void TimePicker_Changed(object? sender, EventArgs e)
        {
            var v = timePicker.Value;
            var s = v.ToLongTimeString();
            timeLabel.Text = $"Selected: {s}";
        }

        private void SetNow_Click(object? sender, EventArgs e)
        {
            datePicker.Value = DateTime.Now;
            timePicker.Value = DateTime.Now;
        }

        private void SetNowDate_Click(object? sender, EventArgs e)
        {
            datePicker.Value = DateTime.Today;
            timePicker.Value = DateTime.Today;
        }

        private void HasBorderButton_Click(object? sender, EventArgs e)
        {
            //datePicker.HasBorder = !datePicker.HasBorder;
            //timePicker.HasBorder = !timePicker.HasBorder;
        }

        private void RangeAnyDate_Click(object? sender, EventArgs e)
        {
            datePicker.UseMinMaxDate = false;
        }

        private void RangeTomorrow_Click(object? sender, EventArgs e)
        {
            datePicker.UseMinMaxDate = false;
            datePicker.MaxDate = DateTime.Today.AddDays(1);
            datePicker.UseMaxDate = true;
        }

        private void RangeYesterday_Click(object? sender, EventArgs e)
        {
            datePicker.UseMinMaxDate = false;
            datePicker.MinDate = DateTime.Today.AddDays(-1);
            datePicker.UseMinDate = true;
        }

        private void RangeYesterdayTomorrow_Click(object? sender, EventArgs e)
        {
            datePicker.UseMinMaxDate = false;
            datePicker.MaxDate = DateTime.Today.AddDays(1);
            datePicker.MinDate = DateTime.Today.AddDays(-1);
            datePicker.UseMinMaxDate = true;
        }

        private void Popup_CheckedChanged(object? sender, EventArgs e)
        {
            /*
            if (SpinRadioButton.IsChecked)
            {
                datePicker.PopupKind = DateTimePickerPopupKind.Spin;
                return;
            }

            if (DropDownRadioButton.IsChecked)
            {
                datePicker.PopupKind = DateTimePickerPopupKind.DropDown;
                return;
            }

            if (DefaultRadioButton.IsChecked)
            {
                datePicker.PopupKind = DateTimePickerPopupKind.Default;
                return;
            }
            */
        }
    }
}