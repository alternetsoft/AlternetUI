using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a yearly repeat pattern rule.
    /// </summary>
    public partial class YearlyRepeatPatternRule : OwnedRepeatPatternRule
    {
        private int intervalYears = 1;
        private CalendarMonth month = CalendarMonth.January;
        private ExtendedDayOfWeek dayOfWeek = ExtendedDayOfWeek.Day;
        private int dayOfMonth = 1;
        private RelativeWeekday dayOfWeekIndex = RelativeWeekday.First;
        private RepeatKind kind = RepeatKind.DayOfMonth;

        /// <summary>
        /// Initializes a new instance of the <see cref="YearlyRepeatPatternRule"/> class with the specified owner.
        /// </summary>
        /// <param name="owner">The owner <see cref="DateRepeatPatternRule"/> of this rule.</param>
        public YearlyRepeatPatternRule(DateRepeatPatternRule owner) : base(owner)
        {
        }

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
                SetProperty(ref kind, value);
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
                SetProperty(ref intervalYears, value);
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
                SetProperty(ref month, value);
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
                SetProperty(ref dayOfMonth, value);
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
        /// Determines whether the specified object is equal to the current instance of <see cref="YearlyRepeatPatternRule"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not YearlyRepeatPatternRule other)
            {
                return false;
            }

            return IntervalYears == other.IntervalYears &&
                   Month == other.Month &&
                   DayOfWeek == other.DayOfWeek &&
                   DayOfMonth == other.DayOfMonth &&
                   DayOfWeekIndex == other.DayOfWeekIndex &&
                   Kind == other.Kind;
        }

        /// <summary>
        /// Assigns the values from another instance to the current instance.
        /// </summary>
        /// <param name="other">The instance from which to copy values.</param>
        public virtual void Assign(object? other)
        {
            if (other == null)
            {
                SuspendPropertyChanged();
                IntervalYears = 1;
                Month = CalendarMonth.January;
                DayOfWeek = ExtendedDayOfWeek.Day;
                DayOfMonth = 1;
                DayOfWeekIndex = RelativeWeekday.First;
                Kind = RepeatKind.DayOfMonth;
                ResumePropertyChanged();
                return;
            }

            if (other is YearlyRepeatPatternRule otherRule)
            {
                if (Equals(other))
                    return;

                SuspendPropertyChanged();
                IntervalYears = otherRule.IntervalYears;
                Month = otherRule.Month;
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
            DateOnly minDate = StartDate;
            DateOnly maxDate;

            switch (EndCondition)
            {
                default:
                case EndConditionKind.Never:
                case EndConditionKind.AfterOccurrence:
                    maxDate = prm.MaxDate;
                    break;
                case EndConditionKind.OnDate:
                    maxDate = DateUtils.Min(EndDate, prm.MaxDate);
                    break;
            }

            int startYear = minDate.Year;
            int endYear = maxDate.Year;

            IEnumerable<DateOnly> GetDatesUnfiltered()
            {
                for (int year = startYear; year <= endYear; year += IntervalYears)
                {
                    DateOnly? occurrenceDate = GetDate(year);

                    if (occurrenceDate is null)
                        continue;

                    var d = occurrenceDate.Value;

                    if (d < minDate)
                        continue;
                    if (d > maxDate)
                        break;
                    yield return d;
                }
            }

            IEnumerable<DateOnly> GetDates()
            {
                foreach (var date in GetDatesUnfiltered())
                {
                    if (date >= prm.MinDate)
                        yield return date;
                }
            }

            IEnumerable<DateOnly> GetDatesEndsAfterOccurrence()
            {
                var count = OccurrenceCount;

                if (count <= 0)
                    yield break;

                var numProcessed = 0;

                foreach (var date in GetDatesUnfiltered())
                {
                    if (date >= prm.MinDate)
                        yield return date;

                    numProcessed++;
                    if (numProcessed >= count)
                        yield break;
                }
            }

            if (EndCondition == EndConditionKind.AfterOccurrence)
            {
                return new(GetDatesEndsAfterOccurrence());
            }
            else
            {
                return new(GetDates());
            }
        }

        /// <summary>
        /// Gets the date for the specified year based on the yearly repeat pattern.
        /// </summary>
        /// <param name="year">The year for which to get the date.</param>
        /// <returns>The calculated date, or <c>null</c> if the date is invalid.</returns>
        public virtual DateOnly? GetDate(int year)
        {
            if (Kind == RepeatKind.DayOfMonth)
            {
                if (DayOfMonth < 1 || DayOfMonth > DateUtils.GetDaysInMonth(year, Month))
                    return null;
                return new DateOnly(year, (int)Month, DayOfMonth);
            }
            else if (Kind == RepeatKind.RelativeWeekday)
            {
                var relativeWeekday = new RelativeWeekdayOfMonth(DayOfWeekIndex, DayOfWeek, Month);
                return relativeWeekday.GetDate(year);
            }

            return null;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (IntervalYears, Month, DayOfWeek, DayOfMonth, DayOfWeekIndex, Kind, StartDate, EndDate, OccurrenceCount)
                .GetHashCode();
        }
    }
}
