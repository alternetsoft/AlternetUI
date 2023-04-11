using Alternet.UI;
using System;
using System.Linq;

namespace ControlsSample
{
    partial class DateTimePage : Control
    {
        private IPageSite? site;

        public DateTimePage()
        {
            InitializeComponent();
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
            dateLabel.Text = string.Format("Selected Date: {0}", datePicker.Value.ToString());
        }
    }
}