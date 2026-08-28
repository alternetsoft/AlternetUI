using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a rule for a repeat pattern in scheduling events or tasks.
    /// </summary>
    public abstract partial class DateRepeatPatternRule : BaseObjectWithNotify
    {
        private DateOnly startDate;
        private DateOnly? endDate;
        private int occurrenceCount;

        /// <summary>
        /// Gets or sets the start date of the repeat pattern range. 
        /// </summary>
        public virtual DateOnly StartDate
        {
            get => startDate;
            set
            {
                startDate = GetNewFieldValue(startDate, value);
            }
        }

        /// <summary>
        /// Gets or sets the end date of the repeat pattern range.
        /// If not set, the repeat pattern is considered to have no end date.
        /// </summary>
        public virtual DateOnly? EndDate
        {
            get => endDate;
            set
            {
                endDate = GetNewFieldValue(endDate, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of occurrences within the range.
        /// If 0, the repeat pattern is considered to have no limit on occurrences.
        /// </summary>
        public virtual int OccurrenceCount
        {
            get => occurrenceCount;
            set
            {
                occurrenceCount = GetNewFieldValue(occurrenceCount, value);
            }
        }

        /// <summary>
        /// Coerces the specified minimum and maximum dates based on the start and end dates of the repeat pattern.
        /// </summary>
        /// <param name="minDate">The minimum date to consider.</param>
        /// <param name="maxDate">The maximum date to consider.</param>
        public virtual void CoerceMinMaxDate(ref DateOnly minDate, ref DateOnly maxDate)
        {
            if (EndDate is not null && EndDate < maxDate)
                maxDate = EndDate.Value;

            // Here we can add optimization for some cases and to use minDate as is.

            minDate = StartDate;
        }

        /// <summary>
        /// Gets the occurrences of the repeat pattern within the specified range and up to the maximum date.
        /// </summary>
        /// <param name="minDate">The minimum date to consider for the occurrences.</param>
        /// <param name="maxDate">The maximum date to consider for the occurrences.</param>
        /// <returns>An enumerable of the occurrence dates.</returns>
        public virtual IEnumerable<DateOnly> GetDates(DateOnly minDate, DateOnly maxDate)
        {
            return Array.Empty<DateOnly>();
        }
    }

    /// <summary>
    /// Represents a daily repeat pattern rule.
    /// </summary>
    public partial class DailyRepeatPatternRule : DateRepeatPatternRule
    {
        private int intervalDays = 2;
        private RepeatKind kind = RepeatKind.EveryDay;

        /// <summary>
        /// Represents the kind of daily repeat pattern.
        /// </summary>
        public enum RepeatKind
        {
            /// <summary>
            /// Repeat every day (default behavior).
            /// </summary>
            EveryDay,

            /// <summary>
            /// Repeat only on even days of the month (2, 4, 6, ...).
            /// </summary>
            EvenDays,

            /// <summary>
            /// Repeat only on odd days of the month (1, 3, 5, ...).
            /// </summary>
            OddDays,

            /// <summary>
            /// Repeat every N days (uses <see cref="IntervalDays"/>).
            /// </summary>
            IntervalDays,

            /// <summary>
            /// Repeat only on weekdays (Mon–Fri).
            /// </summary>
            Weekdays,

            /// <summary>
            /// Repeat only on weekends (Sat–Sun).
            /// </summary>
            Weekends,
        }

        /// <summary>
        /// Gets or sets the kind of daily repeat pattern.
        /// </summary>
        public virtual RepeatKind Kind
        {
            get => kind;
            set
            {
                kind = GetNewFieldValue(kind, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of days between occurrences.
        /// Example: 1 means "every day", 2 means "every 2 days".
        /// </summary>
        public virtual int IntervalDays
        {
            get => intervalDays;
            set
            {
                if (value < 1)
                    value = 1;
                intervalDays = GetNewFieldValue(intervalDays, value);
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<DateOnly> GetDates(DateOnly minDate, DateOnly maxDate)
        {
            CoerceMinMaxDate(ref minDate, ref maxDate);

            if (minDate > maxDate)
                return Array.Empty<DateOnly>();

            var currentDate = minDate;

            switch (Kind)
            {
                case RepeatKind.EveryDay:
                    return GetEveryDay();
                case RepeatKind.EvenDays:
                    return GetDays((d) => d.Day % 2 == 0);
                case RepeatKind.OddDays:
                    return GetDays((d) => d.Day % 2 != 0);
                case RepeatKind.IntervalDays:
                    return GetIntervalDays();
                case RepeatKind.Weekends:
                    return GetWeekDays(DaysOfWeek.Weekend);
                case RepeatKind.Weekdays:
                    return GetWeekDays(DaysOfWeek.Weekdays);
                default:
                    return Array.Empty<DateOnly>();
            }

            IEnumerable<DateOnly> GetIntervalDays()
            {
                var returnedCount = 0;
                var occ = OccurrenceCount;

                while (currentDate <= maxDate)
                {
                    yield return currentDate;
                    currentDate = currentDate.AddDays(intervalDays);
                    returnedCount++;
                    if (occ > 0 && returnedCount >= occ)
                        yield break;
                }
            }

            IEnumerable<DateOnly> GetEveryDay()
            {
                while (currentDate <= maxDate)
                {
                    yield return currentDate;
                    currentDate = currentDate.AddDays(1);
                }
            }

            IEnumerable<DateOnly> GetDays(Func<DateOnly, bool> predicate)
            {
                while (currentDate <= maxDate)
                {
                    if (predicate(currentDate))
                        yield return currentDate;
                    currentDate = currentDate.AddDays(1);
                }
            }

            IEnumerable<DateOnly> GetWeekDays(DaysOfWeek days)
            {
                return GetDays((d) => days.HasDay(d.DayOfWeek));
            }
        }
    }

    /// <summary>
    /// Represents a weekly repeat pattern rule.
    /// </summary>
    public partial class WeeklyRepeatPatternRule : DateRepeatPatternRule
    {
        private int intervalWeeks = 1;
        private DaysOfWeek days;

        /// <summary>
        /// Gets or sets the number of weeks between occurrences.
        /// </summary>
        public virtual int IntervalWeeks
        {
            get => intervalWeeks;
            set
            {
                if (value < 1)
                    value = 1;
                intervalWeeks = GetNewFieldValue(intervalWeeks, value);
            }
        }

        /// <summary>
        /// Gets or sets the days of the week on which the event occurs.
        /// </summary>
        public virtual DaysOfWeek WeekDays
        {
            get => days;
            set
            {
                days = GetNewFieldValue(days, value);
            }
        }
    }

    /// <summary>
    /// Represents a monthly repeat pattern rule.
    /// </summary>
    public partial class MonthlyRepeatPatternRule : DateRepeatPatternRule
    {
        private int dayOfMonth = 1;
        private int intervalMonths = 1;
        private RepeatKind kind = RepeatKind.DayOfMonth;
        private ExtendedDayOfWeek dayOfWeek = ExtendedDayOfWeek.Day;
        private RelativeWeekday dayOfWeekIndex = RelativeWeekday.First;

        /// <summary>
        /// Gets or sets the kind of monthly repeat pattern.
        /// </summary>
        public enum RepeatKind
        {
            /// <summary>
            /// Repeat on a fixed day of the month (e.g., 15th of every month).
            /// </summary>
            DayOfMonth,

            /// <summary>
            /// Repeat on a relative weekday occurrence (e.g., First Monday of every month).
            /// </summary>
            RelativeWeekday,
        }

        /// <summary>
        /// Gets or sets the kind of daily repeat pattern.
        /// </summary>
        public virtual RepeatKind Kind
        {
            get => kind;
            set
            {
                kind = GetNewFieldValue(kind, value);
            }
        }

        /// <summary>
        /// Gets or sets the day of the month for recurrence.
        /// Example: 26 means "Day 26 of every month".
        /// </summary>
        public virtual int DayOfMonth
        {
            get => dayOfMonth;
            set
            {
                if (value < 1)
                    value = 1;
                if (value > 31)
                    value = 31;
                dayOfMonth = GetNewFieldValue(dayOfMonth, value);
            }
        }

        /// <summary>
        /// Gets or sets the interval in months between occurrences.
        /// Example: 1 means "every month", 2 means "every 2 months".
        /// </summary>
        public virtual int IntervalMonths
        {
            get => intervalMonths;
            set
            {
                if (value < 1)
                    value = 1;
                intervalMonths = GetNewFieldValue(intervalMonths, value);
            }
        }

        /// <summary>
        /// Gets or sets the day of week for recurrence.
        /// Example: Sunday, Monday, etc.
        /// Used when recurrence is based on a weekday occurrence.
        /// </summary>
        public virtual ExtendedDayOfWeek DayOfWeek
        {
            get => dayOfWeek;
            set
            {
                dayOfWeek = GetNewFieldValue(dayOfWeek, value);
            }
        }

        /// <summary>
        /// Gets or sets the occurrence index within the month
        /// (e.g., First, Second, Third, Last).
        /// Used when recurrence is based on a weekday occurrence.
        /// </summary>
        public virtual RelativeWeekday DayOfWeekIndex
        {
            get => dayOfWeekIndex;
            set
            {
                dayOfWeekIndex = GetNewFieldValue(dayOfWeekIndex, value);
            }
        }
    }

    /// <summary>
    /// Represents a yearly repeat pattern rule.
    /// </summary>
    public partial class YearlyRepeatPatternRule : DateRepeatPatternRule
    {
        private int intervalYears = 1;
        private CalendarMonth month = CalendarMonth.January;
        private ExtendedDayOfWeek dayOfWeek = ExtendedDayOfWeek.Day;
        private int dayOfMonth = 1;
        private RelativeWeekday dayOfWeekIndex = RelativeWeekday.First;
        private RepeatKind kind = RepeatKind.DayOfMonth;

        /// <summary>
        /// Gets or sets the kind of yearly repeat pattern.
        /// </summary>
        public enum RepeatKind
        {
            /// <summary>
            /// Repeat on a fixed day of the month (e.g., August 26th every year).
            /// </summary>
            DayOfMonth,

            /// <summary>
            /// Repeat on a relative weekday occurrence (e.g., First Monday of August every year).
            /// </summary>
            RelativeWeekday,
        }

        /// <summary>
        /// Gets or sets the kind of yearly repeat pattern.
        /// </summary>
        public virtual RepeatKind Kind
        {
            get => kind;
            set
            {
                kind = GetNewFieldValue(kind, value);
            }
        }

        /// <summary>
        /// Gets or sets the interval in years between occurrences.
        /// Example: 1 means "every year", 2 means "every 2 years".
        /// </summary>
        public virtual int IntervalYears
        {
            get => intervalYears;
            set
            {
                if (value < 1)
                    value = 1;
                intervalYears = GetNewFieldValue(intervalYears, value);
            }
        }

        /// <summary>
        /// Gets or sets the month of the recurrence.
        /// Example: August.
        /// </summary>
        public virtual CalendarMonth Month
        {
            get => month;
            set
            {
                month = GetNewFieldValue(month, value);
            }
        }

        /// <summary>
        /// Gets or sets the day of the month for recurrence.
        /// Example: 26 means "On August 26".
        /// Used when recurrence is based on a fixed date.
        /// </summary>
        public virtual int DayOfMonth
        {
            get => dayOfMonth;
            set
            {
                dayOfMonth = GetNewFieldValue(dayOfMonth, value);
            }
        }

        /// <summary>
        /// Gets or sets the occurrence index within the month
        /// (e.g., First, Second, Third, Last).
        /// Used when recurrence is based on a weekday occurrence.
        /// </summary>
        public virtual RelativeWeekday DayOfWeekIndex
        {
            get => dayOfWeekIndex;
            set
            {
                dayOfWeekIndex = GetNewFieldValue(dayOfWeekIndex, value);
            }
        }

        /// <summary>
        /// Gets or sets the day of week for recurrence.
        /// Example: Sunday, Monday, etc.
        /// Used when recurrence is based on a weekday occurrence.
        /// </summary>
        public virtual ExtendedDayOfWeek DayOfWeek
        {
            get => dayOfWeek;
            set
            {
                dayOfWeek = GetNewFieldValue(dayOfWeek, value);
            }
        }
    }
}
