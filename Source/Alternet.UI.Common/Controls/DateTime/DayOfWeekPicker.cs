using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

using Alternet.UI.Localization;

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
            SetDisplayText(ExtendedDayOfWeek.Day, CommonStrings.Default.ExtendedDayOfWeekDay);
            SetDisplayText(ExtendedDayOfWeek.Weekday, CommonStrings.Default.ExtendedDayOfWeekWeekday);
            SetDisplayText(ExtendedDayOfWeek.Weekend, CommonStrings.Default.ExtendedDayOfWeekWeekend);
            SetExtendedItemsVisibility(false);
            Value = ExtendedDayOfWeek.Sunday;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the extended "Day" item is visible in the picker.
        /// </summary>
        public bool ShowDayItem
        {
            get
            {
                return GetItemVisibility(ExtendedDayOfWeek.Day);
            }

            set
            {
                SetItemVisibility(ExtendedDayOfWeek.Day, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the extended "Weekday" item is visible in the picker.
        /// </summary>
        public bool ShowWeekdayItem
        {
            get
            {
                return GetItemVisibility(ExtendedDayOfWeek.Weekday);
            }
            set
            {
                SetItemVisibility(ExtendedDayOfWeek.Weekday, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the extended "Weekend" item is visible in the picker.
        /// </summary>
        public bool ShowWeekendItem
        {
            get
            {
                return GetItemVisibility(ExtendedDayOfWeek.Weekend);
            }
            set
            {
                SetItemVisibility(ExtendedDayOfWeek.Weekend, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected day of the week as a <see cref="DayOfWeek"/> value.
        /// This property returns null if the selected value is one of the extended day options (Day, Weekday, Weekend).
        /// </summary>
        public virtual DayOfWeek? AsDayOfWeek
        {
            get
            {
                if (Value >= ExtendedDayOfWeek.Day)
                    return null;
                return (DayOfWeek)Value;
            }
            set
            {
                if (value == null)
                    Value = ExtendedDayOfWeek.Day;
                else
                    Value = (ExtendedDayOfWeek)value;
            }
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
        /// Gets the visibility of a specific day item.
        /// </summary>
        /// <param name="day">The day to get the visibility for.</param>
        /// <returns>True if the day is visible; otherwise, false.</returns>
        public virtual bool GetItemVisibility(ExtendedDayOfWeek day)
        {
            var item = FindItemWithValue(day);
            return item?.IsVisible ?? false;
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
                    int dayIndex = (int)value;
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
