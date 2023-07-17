using System;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class DateTimePage : Control
    {
        private IPageSite? site;

        public DateTimePage()
        {
            InitializeComponent();
            datePicker.Value = DateTime.Now;
            timePicker.Value = DateTime.Now;
        }

        public IPageSite? Site
        {
            get => site;

            set
            {
                site = value;
            }
        }

        private void DatePicker_DateChanged(object? sender, EventArgs e)
        {
            var v = datePicker.Value;
            var s = v.ToLongDateString();
            dateLabel.Text = $"Selected Value: {s}";
        }

        private void TimePicker_Changed(object? sender, EventArgs e)
        {
            var v = timePicker.Value;
            var s = v.ToLongTimeString();
            timeLabel.Text = $"Selected Value: {s}";
        }

    }
}