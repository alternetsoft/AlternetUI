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

        /// <inheritdoc/>
        public override IEnumerable<DateOnly> GetDates(DateOnly minDate, DateOnly maxDate)
        {
            return base.GetDates(minDate, maxDate);
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
