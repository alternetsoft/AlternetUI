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
        private DateOnly? startDate;
        private DateOnly? endDate;
        private int occurrenceCount;

        /// <summary>
        /// Gets or sets the start date of the repeat pattern range. 
        /// If not set, the repeat pattern is considered to start from the earliest possible date.
        /// </summary>
        public virtual DateOnly? StartDate
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

            if (StartDate is not null && StartDate > minDate)
                minDate = StartDate.Value;
        }

        /// <summary>
        /// Gets the occurrences of the repeat pattern within the specified range and up to the maximum date.
        /// </summary>
        /// <param name="minDate">The minimum date to consider for the occurrences.</param>
        /// <param name="maxDate">The maximum date to consider for the occurrences.</param>
        /// <returns>An enumerable of the occurrence dates.</returns>
        public virtual IEnumerable<DateOnly> GetOccurrences(DateOnly minDate, DateOnly maxDate)
        {
            return Array.Empty<DateOnly>();
        }
    }

    /// <summary>
    /// Represents a daily repeat pattern rule.
    /// </summary>
    public partial class DailyRepeatPatternRule : DateRepeatPatternRule
    {
        private int intervalDays = 1;

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
                intervalDays = GetNewFieldValue<int>(intervalDays, value);
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<DateOnly> GetOccurrences(DateOnly minDate, DateOnly maxDate)
        {
            CoerceMinMaxDate(ref minDate, ref maxDate);

            if (minDate > maxDate)
                yield break;

            var startDate = minDate;
            var returnedCount = 0;
            var occ = OccurrenceCount;

            while (startDate <= maxDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(intervalDays);
                returnedCount++;
                if (occ > 0 && returnedCount >= occ)
                    yield break;
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
        public virtual DaysOfWeek Days
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
        private bool useDayOfMonth = true;
        private int intervalMonths = 1;
        private ExtendedDayOfWeek dayOfWeek = ExtendedDayOfWeek.Day;
        private RelativeWeekday dayOfWeekIndex = RelativeWeekday.First;

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
        /// Gets or sets a value indicating whether the recurrence
        /// is based on a fixed day of the month (true) or a relative
        /// weekday occurrence (false).
        /// </summary>
        public virtual bool UseDayOfMonth
        {
            get => useDayOfMonth;
            set
            {
                useDayOfMonth = GetNewFieldValue(useDayOfMonth, value);
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
        private bool useFixedDate = true;
        private int dayOfMonth = 1;
        private RelativeWeekday dayOfWeekIndex = RelativeWeekday.First;

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
        /// Gets or sets a value indicating whether the recurrence
        /// is based on a fixed date (true) or a relative weekday occurrence (false).
        /// </summary>
        public virtual bool UseFixedDate
        {
            get => useFixedDate;
            set
            {
                useFixedDate = GetNewFieldValue(useFixedDate, value);
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
