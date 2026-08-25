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
        /// <summary>
        /// Gets or sets the default text case rule for the displayed labels.
        /// </summary>
        public static TextCaseRule DefaultTextCase = TextCaseRule.SentenceCase;

        private IFormatProvider? formatProvider;
        private DayNamesKind dayNamesKind = DayNamesKind.Full;
        private bool showExtendedItemsFirst = true;
        private TextCaseRule textCase = DefaultTextCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="DayOfWeekPicker"/> class.
        /// </summary>
        public DayOfWeekPicker()
        {
            EnumType = typeof(ExtendedDayOfWeek);
            UpdateDayLabels();
            SetExtendedItemsVisibility(false);
            ListItems.Sort(ItemsComparison);
            Value = ExtendedDayOfWeek.Sunday;
        }

        /// <summary>
        /// Gets or sets a value indicating whether extended items (day, weekday, weekend)
        /// are displayed before standard weekdays.
        /// </summary>
        public virtual bool ShowExtendedItemsFirst
        {
            get => showExtendedItemsFirst;
            set
            {
                if (showExtendedItemsFirst == value)
                    return;
                showExtendedItemsFirst = value;
                ListItems.Sort(ItemsComparison);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the text case rule for the displayed labels.
        /// </summary>
        public virtual TextCaseRule TextCase
        {
            get => textCase;
            set
            {
                if (textCase != value)
                {
                    textCase = value;
                    UpdateDayLabels();
                    ReassignValue();
                }
            }
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
        /// Compares two <see cref="ListControlItem"/> instances based on their <see cref="ExtendedDayOfWeek"/> values. 
        /// </summary>
        /// <param name="a">The first item to compare.</param>
        /// <param name="b">The second item to compare.</param>
        /// <returns>A signed integer that indicates the relative values of a and b.</returns>
        protected virtual int ItemsComparison(ListControlItem a, ListControlItem b)
        {
            if (a.Value is ExtendedDayOfWeek aValue && b.Value is ExtendedDayOfWeek bValue)
            {
                if (showExtendedItemsFirst)
                {
                    if (aValue >= ExtendedDayOfWeek.Day && bValue < ExtendedDayOfWeek.Day)
                        return -1;
                    if (aValue < ExtendedDayOfWeek.Day && bValue >= ExtendedDayOfWeek.Day)
                        return 1;
                }
                else
                {
                    if (aValue >= ExtendedDayOfWeek.Day && bValue < ExtendedDayOfWeek.Day)
                        return 1;
                    if (aValue < ExtendedDayOfWeek.Day && bValue >= ExtendedDayOfWeek.Day)
                        return -1;
                }

                return aValue.CompareTo(bValue);
            }

            return 0;
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
        /// Formats the label for a given day of the week.
        /// This method can be overridden to customize the display text for each day.
        /// </summary>
        /// <param name="day">The day of the week.</param>
        /// <param name="dayName">The default name of the day.</param>
        /// <returns>The formatted label for the day.</returns>
        protected virtual string FormatDayLabel(ExtendedDayOfWeek day, string dayName)
        {
            return StringUtils.ChangeCase(dayName, textCase) ?? string.Empty;
        }

        /// <summary>
        /// Updates the day labels based on the current <see cref="DayNamesKind"/> and <see cref="FormatProvider"/>.
        /// </summary>
        protected virtual void UpdateDayLabels()
        {
            SetDisplayText(
                ExtendedDayOfWeek.Day,
                FormatDayLabel(ExtendedDayOfWeek.Day, CommonStrings.Default.ExtendedDayOfWeekDay));
            
            SetDisplayText(
                ExtendedDayOfWeek.Weekday,
                FormatDayLabel(ExtendedDayOfWeek.Weekday, CommonStrings.Default.ExtendedDayOfWeekWeekday));
            
            SetDisplayText(
                ExtendedDayOfWeek.Weekend,
                FormatDayLabel(ExtendedDayOfWeek.Weekend, CommonStrings.Default.ExtendedDayOfWeekWeekend));

            string[] days = DateUtils.GetDayNames(DayNamesKind, FormatProvider);

            foreach (var item in ListItems)
            {
                if (item.Value is ExtendedDayOfWeek value)
                {
                    int dayIndex = (int)value;
                    if (dayIndex >= 0 && dayIndex < days.Length)
                    {
                        item.Text = FormatDayLabel(value, days[dayIndex]);
                    }
                }
            }

            Invalidate();
        }
    }
}
