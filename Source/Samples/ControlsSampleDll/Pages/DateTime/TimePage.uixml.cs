using System;
using System.Linq;

using Alternet.UI;

namespace ControlsSample
{
    public partial class TimePage : Panel
    {
        static TimePage()
        {
            var setAmPmDesignators = false;

            if (setAmPmDesignators)
            {
                DateUtils.AmDesignatorOverride = "AM";
                DateUtils.PmDesignatorOverride = "PM";
            }
        }

        public TimePage()
        {
            InitializeComponent();

            tabControl2.HorizontalAlignment = HorizontalAlignment.Fill;

            timePicker.Value = DateTime.Now;

            hourFormatPicker.EnumType = typeof(TimePickerHourFormat);
            hourFormatPicker.Value = TimePickerHourFormat.System;
            hourFormatPicker.ValueChanged += (s,e) =>
            {
                timePicker.HourFormat = (TimePickerHourFormat)hourFormatPicker.Value;
            };

            showSecondsCheckBox.CheckedChanged += (s, e) =>
            {
                timePicker.SecondsVisible = showSecondsCheckBox.IsChecked;
            };

            timePicker.ContextMenuStrip.Add("Set Now", () =>
            {
                timePicker.Value = DateTime.Now;
            });

            timePicker.ContextMenuStrip.Add("Set 12 PM", () =>
            {
                timePicker.Value = DateTime.Today.AddHours(12);
            });

            timePicker.ContextMenuStrip.Add("Set 12 AM", () =>
            {
                timePicker.Value = DateTime.Today;
            });
        }

        private void TimePicker_Changed(object? sender, EventArgs e)
        {
            var v = timePicker.Value;
            var s = v.ToLongTimeString().NormalizeForDrawText();
            timeLabel.Text = $"Selected: {s}";
        }

        private void SetNow_Click(object? sender, EventArgs e)
        {
            timePicker.Value = DateTime.Now;
        }

        private void HasBorderButton_Click(object? sender, EventArgs e)
        {
            timePicker.HasBorder = !timePicker.HasBorder;
        }
    }
}