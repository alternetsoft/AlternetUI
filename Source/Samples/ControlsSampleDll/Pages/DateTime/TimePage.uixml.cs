using System;
using System.Linq;

using Alternet.UI;

namespace ControlsSample
{
    internal partial class TimePage : Panel
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

            tabControl2.MinSizeGrowMode = WindowSizeToContentMode.Width;

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
        }

        private void TimePicker_Changed(object? sender, EventArgs e)
        {
            var v = timePicker.Value;
            var s = v.ToLongTimeString();
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