using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a daily repeat pattern rule.
    /// </summary>
    public partial class DailyRepeatPatternRule : OwnedRepeatPatternRule
    {
        private int intervalDays = 2;
        private RepeatKind kind = RepeatKind.EveryDay;

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyRepeatPatternRule"/> class with the specified owner.
        /// </summary>
        /// <param name="owner">The owner <see cref="DateRepeatPatternRule"/> of this rule.</param>
        public DailyRepeatPatternRule(DateRepeatPatternRule owner) : base(owner)
        {
        }

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
                SetProperty(ref kind, value);
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
                SetProperty(ref intervalDays, value);
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance of <see cref="DailyRepeatPatternRule"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not DailyRepeatPatternRule other)
            {
                return false;
            }

            return IntervalDays == other.IntervalDays &&
                   Kind == other.Kind;
        }

        /// <inheritdoc/>
        public override RuleGetDatesResult GetDates(RuleGetDatesParams prm)
        {
            return GetDates(prm, IsDateInPattern, GetNextDate);
        }

        /// <summary>
        /// Determines whether the specified date is part of the repeat pattern.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns><c>true</c> if the specified date is part of the repeat pattern; otherwise, <c>false</c>.</returns>
        protected virtual bool IsDateInPattern(DateOnly date)
        {
            return true;
        }

        /// <summary>
        /// Gets the next date in the repeat pattern after the specified date.
        /// </summary>
        /// <param name="date">The date to get the next occurrence for.</param>
        /// <returns>The next date in the repeat pattern after the specified date.</returns>
        protected virtual DateOnly GetNextDate(DateOnly date)
        {
            return date.AddDays(1);
        }

        /// <summary>
        /// Gets the occurrences of the repeat pattern within the specified range,
        /// filtered by a predicate, and up to the maximum date.
        /// </summary>
        /// <param name="prm">The parameters specifying the minimum and maximum
        /// dates to consider for the occurrences.</param>
        /// <param name="predicate">A predicate to filter the dates.</param>
        /// <param name="nextDate">A function to get the next date in the pattern.</param>
        /// <returns>An enumerable of the occurrence dates.</returns>
        protected virtual RuleGetDatesResult GetDates(
            RuleGetDatesParams prm,
            Predicate<DateOnly> predicate,
            Func<DateOnly, DateOnly> nextDate)
        {
            switch (EndCondition)
            {
                case EndConditionKind.Never:
                    return new(GetDatesEndsOnDate(prm.MaxDate));
                case EndConditionKind.AfterOccurrence:
                    return new(GetDatesEndsAfterOccurrence());
                case EndConditionKind.OnDate:
                    return new(GetDatesEndsOnDate(DateUtils.Min(EndDate, prm.MaxDate)));
                default:
                    return new();
            }

            IEnumerable<DateOnly> GetDatesEndsOnDate(DateOnly maxDate)
            {
                var currentDate = StartDate;

                while (currentDate <= maxDate)
                {
                    if (currentDate >= prm.MinDate)
                    {
                        if (predicate(currentDate))
                        {
                            yield return currentDate;
                        }
                    }

                    currentDate = nextDate(currentDate);
                }
            }

            IEnumerable<DateOnly> GetDatesEndsAfterOccurrence()
            {
                if (OccurrenceCount <= 0)
                    yield break;

                var currentDate = StartDate;
                var maxDate = prm.MaxDate;
                var numProcessed = 0;

                while (currentDate <= maxDate)
                {
                    if (predicate(currentDate))
                    {
                        if (currentDate >= prm.MinDate)
                        {
                            yield return currentDate;
                        }

                        numProcessed++;
                        if (numProcessed >= OccurrenceCount)
                            yield break;
                    }

                    currentDate = nextDate(currentDate);
                }
            }
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
                IntervalDays = 1;
                Kind = RepeatKind.EveryDay;
                ResumePropertyChanged();
                return;
            }

            if (other is DailyRepeatPatternRule otherRule)
            {
                if (Equals(other))
                    return;

                SuspendPropertyChanged();
                IntervalDays = otherRule.IntervalDays;
                Kind = otherRule.Kind;
                ResumePropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (IntervalDays, Kind, StartDate, EndDate, OccurrenceCount).GetHashCode();
        }
    }
}
