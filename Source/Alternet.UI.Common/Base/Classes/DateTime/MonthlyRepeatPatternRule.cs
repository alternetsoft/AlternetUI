using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a monthly repeat pattern rule.
    /// </summary>
    public partial class MonthlyRepeatPatternRule : OwnedRepeatPatternRule
    {
        private int dayOfMonth = 1;
        private int intervalMonths = 1;
        private RepeatKind kind = RepeatKind.DayOfMonth;
        private ExtendedDayOfWeek dayOfWeek = ExtendedDayOfWeek.Day;
        private RelativeWeekday dayOfWeekIndex = RelativeWeekday.First;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthlyRepeatPatternRule"/> class with the specified owner.
        /// </summary>
        /// <param name="owner">The owner <see cref="DateRepeatPatternRule"/> of this rule.</param>
        public MonthlyRepeatPatternRule(DateRepeatPatternRule owner) : base(owner)
        {
        }

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
                SetProperty(ref kind, value);
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
                SetProperty(ref dayOfMonth, value);
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
                SetProperty(ref intervalMonths, value);
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
                SetProperty(ref dayOfWeek, value);
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
                SetProperty(ref dayOfWeekIndex, value);
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance of <see cref="MonthlyRepeatPatternRule"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not MonthlyRepeatPatternRule other)
            {
                return false;
            }

            return IntervalMonths == other.IntervalMonths &&
                   DayOfWeek == other.DayOfWeek &&
                   DayOfMonth == other.DayOfMonth &&
                   DayOfWeekIndex == other.DayOfWeekIndex &&
                   Kind == other.Kind;
        }

        /// <summary>
        /// Assigns the values from another <see cref="DateRepeatPatternRule"/> instance to the current instance.
        /// </summary>
        /// <param name="other"></param>
        public virtual void Assign(object? other)
        {
            if (other == null)
            {
                SuspendPropertyChanged();
                IntervalMonths = 1;
                DayOfWeek = ExtendedDayOfWeek.Day;
                DayOfMonth = 1;
                DayOfWeekIndex = RelativeWeekday.First;
                Kind = RepeatKind.DayOfMonth;
                ResumePropertyChanged();
                return;
            }

            if (other is MonthlyRepeatPatternRule otherRule)
            {
                if (Equals(other))
                    return;

                SuspendPropertyChanged();
                IntervalMonths = otherRule.IntervalMonths;
                DayOfWeek = otherRule.DayOfWeek;
                DayOfMonth = otherRule.DayOfMonth;
                DayOfWeekIndex = otherRule.DayOfWeekIndex;
                Kind = otherRule.Kind;
                ResumePropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override IDateRepeatPatternRule.RuleGetDatesResult GetDates(IDateRepeatPatternRule.RuleGetDatesParams prm)
        {
            return GetDates(prm, GetDatesUnfiltered);
        }

        /// <summary>
        /// Gets the date for the specified year based on the yearly repeat pattern.
        /// </summary>
        /// <param name="year">The year for which to get the date.</param>
        /// <param name="month">The month for which to get the date.</param>
        /// <returns>The calculated date, or <c>null</c> if the date is invalid.</returns>
        public virtual DateOnly[]? GetDates(int year, CalendarMonth month)
        {
            if (Kind == RepeatKind.DayOfMonth)
            {
                if (DayOfMonth < 1 || DayOfMonth > DateUtils.GetDaysInMonth(year, month))
                    return null;
                return [new DateOnly(year, (int)month, DayOfMonth)];
            }
            else if (Kind == RepeatKind.RelativeWeekday)
            {
                var relativeWeekday = new RelativeWeekdayOfMonth(DayOfWeekIndex, DayOfWeek, month);
                return relativeWeekday.GetDates(year);
            }

            return null;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (IntervalMonths, DayOfWeek, DayOfMonth, DayOfWeekIndex, Kind, StartDate, EndDate, OccurrenceCount).GetHashCode();
        }

        /// <summary>
        /// Gets the occurrences of the repeat pattern within the specified range and up to the maximum date, 
        /// without applying any end condition or occurrence count filtering.
        /// </summary>
        /// <param name="minDate">The minimum date for the range.</param>
        /// <param name="maxDate">The maximum date for the range.</param>
        /// <returns>An enumerable collection of unfiltered dates within the specified range.</returns>
        protected virtual IEnumerable<DateOnly> GetDatesUnfiltered(DateOnly minDate, DateOnly maxDate)
        {
            var start = new DateOnly(minDate.Year, minDate.Month, 1);
            var end = maxDate;

            for (var date = start; date <= end; date = date.AddMonths(IntervalMonths))
            {
                DateOnly[]? occurrenceDates = GetDates(date.Year, (CalendarMonth)date.Month);

                if (occurrenceDates is null)
                    continue;

                foreach (var d in occurrenceDates)
                {
                    if (d < minDate)
                        continue;
                    if (d > maxDate)
                        break;
                    yield return d;
                }
            }
        }
    }
}
