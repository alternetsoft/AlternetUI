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
}
