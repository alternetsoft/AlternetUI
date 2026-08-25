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
        private CultureInfo? culture;

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
        /// Gets or sets the culture used to display month names. If not set, the current culture is used by default.
        /// </summary>
        [Browsable(false)]
        public virtual CultureInfo? Culture
        {
            get => culture;
            set
            {
                if (culture != value)
                {
                    culture = value;
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
        /// Gets the effective culture used for displaying month names.
        /// If the <see cref="Culture"/> property is set, it returns that value; otherwise, it returns the current culture.
        /// </summary>
        /// <returns></returns>
        protected virtual CultureInfo GetEffectiveCulture()
        {
            return culture ?? CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Updates the month labels based on the effective culture.
        /// This method retrieves the month names from the culture's DateTimeFormat
        /// and updates the text of each list item accordingly.
        /// </summary>
        protected virtual void UpdateMonthLabels()
        {
            string[] months = GetEffectiveCulture().DateTimeFormat.MonthNames;

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
