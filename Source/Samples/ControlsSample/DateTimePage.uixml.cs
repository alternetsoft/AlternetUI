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