using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to select a day of the week,
    /// including extended options such as "Day", "Weekday", and "Weekend".
    /// </summary>
    [ControlCategory(KnownControlCategory.Date)]
    public partial class DayOfWeekPicker : EnumPicker
    {
        private IFormatProvider? formatProvider;
        private DayNamesKind dayNamesKind = DayNamesKind.Full;

        /// <summary>
        /// Initializes a new instance of the <see cref="DayOfWeekPicker"/> class.
        /// </summary>
        public DayOfWeekPicker()
        {
            EnumType = typeof(ExtendedDayOfWeek);
            UpdateDayLabels();
            SetExtendedItemsVisibility(false);
            Value = ExtendedDayOfWeek.Sunday;
        }

        /// <summary>
        /// Gets or sets the format provider used to display day names. If not set, the current culture is used by default.
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
                    UpdateDayLabels();
                }
            }
        }

        /// <summary>
        /// Gets or sets the kind of day names to display (full or abbreviated).
        /// </summary>
        public virtual DayNamesKind DayNamesKind
        {
            get => dayNamesKind;
            set
            {
                if (dayNamesKind != value)
                {
                    dayNamesKind = value;
                    UpdateDayLabels();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected day.
        /// </summary>
        public new virtual ExtendedDayOfWeek Value
        {
            get => (ExtendedDayOfWeek?)base.Value ?? ExtendedDayOfWeek.Day;
            set => base.Value = value;
        }

        /// <summary>
        /// Sets the visibility of extended day items (Day, Weekday, Weekend).
        /// </summary>
        /// <param name="isVisible">A value indicating whether the extended day items should be visible.</param>
        public virtual void SetExtendedItemsVisibility(bool isVisible)
        {
            SetItemVisibility(ExtendedDayOfWeek.Day, isVisible);
            SetItemVisibility(ExtendedDayOfWeek.Weekday, isVisible);
            SetItemVisibility(ExtendedDayOfWeek.Weekend, isVisible);
        }

        /// <summary>
        /// Sets the visibility of a specific day.
        /// </summary>
        /// <param name="day">The day to set the visibility for.</param>
        /// <param name="isVisible">A value indicating whether the day should be visible.</param>
        /// <returns>True if the item was found and its visibility was set; otherwise, false.</returns>
        public virtual bool SetItemVisibility(ExtendedDayOfWeek day, bool isVisible)
        {
            var item = FindItemWithValue(day);
            if (item != null)
            {
                item.IsVisible = isVisible;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the day labels based on the current culture.
        /// </summary>
        protected virtual void UpdateDayLabels()
        {
            string[] days = DateUtils.GetDayNames(dayNamesKind, formatProvider);

            foreach (var item in ListItems)
            {
                if (item.Value is ExtendedDayOfWeek value)
                {
                    int dayIndex = (int)value - 1;
                    if (dayIndex >= 0 && dayIndex < days.Length)
                    {
                        item.Text = days[dayIndex];
                    }
                }
            }

            Invalidate();
        }
    }
}
