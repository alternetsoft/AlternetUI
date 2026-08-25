using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to select a month from the Gregorian calendar.
    /// </summary>
    [ControlCategory(KnownControlCategory.Date)]
    public partial class MonthPicker : EnumPicker
    {
        private IFormatProvider? formatProvider;
        private MonthNamesKind monthNamesKind = MonthNamesKind.Full;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthPicker"/> class.
        /// </summary>
        public MonthPicker()
        {
            EnumType = typeof(CalendarMonth);
            UpdateMonthLabels();
            Value = CalendarMonth.January;
        }

        /// <summary>
        /// Gets or sets the kind of month names to display (full or abbreviated).
        /// </summary>
        public virtual MonthNamesKind MonthNamesKind
        {
            get => monthNamesKind;
            set
            {
                if (monthNamesKind != value)
                {
                    monthNamesKind = value;
                    UpdateMonthLabels();
                }
            }
        }

        /// <summary>
        /// Gets or sets the format provider used to display month names. If not set, the current culture is used by default.
        /// </summary>
        [Browsable(false)]
        public virtual IFormatProvider? FormatProvider
        {
            get => formatProvider;
            set
            {
                if (formatProvider != value)
                {
                    formatProvider = value;
                    UpdateMonthLabels();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected month.
        /// </summary>
        public new virtual CalendarMonth Value
        {
            get => (CalendarMonth?)base.Value ?? CalendarMonth.January;
            set => base.Value = value;
        }

        /// <summary>
        /// Updates the month labels based on the current format provider.
        /// This method retrieves the month names from the format provider's DateTimeFormat
        /// and updates the text of each list item accordingly.
        /// </summary>
        protected virtual void UpdateMonthLabels()
        {
            string[] months = DateUtils.GetMonthNames(MonthNamesKind, formatProvider);

            foreach(var item in ListItems)
            {
                if (item.Value is CalendarMonth month)
                {
                    int monthIndex = (int)month - 1;
                    if (monthIndex >= 0 && monthIndex < months.Length)
                    {
                        item.Text = months[monthIndex];
                    }
                }
            }

            Invalidate();
        }
    }
}
