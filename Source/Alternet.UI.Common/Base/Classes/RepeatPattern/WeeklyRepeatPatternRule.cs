using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a weekly repeat pattern rule.
    /// </summary>
    public partial class WeeklyRepeatPatternRule : OwnedRepeatPatternRule
    {
        private int intervalWeeks = 1;
        private DaysOfWeek days;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeeklyRepeatPatternRule"/> class with the specified owner.
        /// </summary>
        /// <param name="owner">The owner <see cref="DateRepeatPatternRule"/> of this rule.</param>
        public WeeklyRepeatPatternRule(DateRepeatPatternRule owner) : base(owner)
        {
        }

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
                SetProperty(ref intervalWeeks, value);
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
                SetProperty(ref days, value);
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance of <see cref="WeeklyRepeatPatternRule"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not WeeklyRepeatPatternRule other)
            {
                return false;
            }

            return IntervalWeeks == other.IntervalWeeks &&
                   WeekDays == other.WeekDays;
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
                IntervalWeeks = 1;
                WeekDays = DaysOfWeek.None;
                ResumePropertyChanged();
                return;
            }

            if (other is WeeklyRepeatPatternRule otherRule)
            {
                if (Equals(other))
                    return;

                SuspendPropertyChanged();
                IntervalWeeks = otherRule.IntervalWeeks;
                WeekDays = otherRule.WeekDays;
                ResumePropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (IntervalWeeks, WeekDays, StartDate, EndDate, OccurrenceCount).GetHashCode();
        }
    }
}
