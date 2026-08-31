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
        private DateOnly endDate;
        private int occurrenceCount = 1;
        private EndConditionKind endCondtion = EndConditionKind.OnDate;

        /// <summary>
        /// Defines the types of end conditions for a repeat pattern.
        /// </summary>
        public enum EndConditionKind
        {
            /// <summary>
            /// The repeat pattern has no end date and no occurrence limit.
            /// </summary>
            Never,

            /// <summary>
            /// The repeat pattern ends after a specific number of occurrences.
            /// </summary>
            AfterOccurrence,

            /// <summary>
            /// The repeat pattern ends on a specific date.
            /// </summary>
            OnDate,
        }

        /// <summary>
        /// Gets or sets the start date of the repeat pattern range. 
        /// </summary>
        public virtual DateOnly StartDate
        {
            get => startDate;
            set
            {
                SetProperty(ref startDate, value, OnStartDateChanged);
            }
        }

        /// <summary>
        /// Gets or sets the end condition of the repeat pattern, which determines
        /// how the repeat pattern ends (never, after a number of occurrences, or on a specific date).
        /// </summary>
        public virtual EndConditionKind EndCondition
        {
            get
            {
                return endCondtion;
            }

            set
            {
                SetProperty(ref endCondtion, value);
            }
        }

        /// <summary>
        /// Gets or sets the end date of the repeat pattern range.
        /// </summary>
        public virtual DateOnly EndDate
        {
            get => endDate;
            set
            {
                SetProperty(ref endDate, value, OnEndDateChanged);
            }
        }

        /// <summary>
        /// Gets or sets the number of occurrences within the range.
        /// </summary>
        public virtual int OccurrenceCount
        {
            get => occurrenceCount;
            set
            {
                if (value < 0)
                    value = 0;
                SetProperty(ref occurrenceCount, value, OnOccurrenceCountChanged);
            }
        }

        /// <summary>
        /// Gets the occurrences of the repeat pattern within the specified range and up to the maximum date.
        /// </summary>
        /// <param name="prm">The parameters specifying the minimum and maximum
        /// dates to consider for the occurrences.</param>
        /// <returns>An enumerable of the occurrence dates.</returns>
        public virtual RuleGetDatesResult GetDates(RuleGetDatesParams prm)
        {
            return GetDates(prm, IsDateInPattern, GetNextDate);
        }

        /// <summary>
        /// Determines whether the specified date is part of the repeat pattern.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        protected virtual bool IsDateInPattern(DateOnly date)
        {
            return true;
        }

        /// <summary>
        /// Gets the next date in the repeat pattern after the specified date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        protected virtual DateOnly GetNextDate(DateOnly date)
        {
            return date.AddDays(1);
        }

        /// <summary>
        /// Gets the occurrences of the repeat pattern within the specified range,
        /// filtered by a predicate, and up to the maximum date.
        /// </summary>
        /// <param name="prm"></param>
        /// <param name="predicate"></param>
        /// <param name="nextDate"></param>
        /// <returns></returns>
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
        /// Called when the <see cref="EndDate"/> property changes.
        /// Override this method to implement custom behavior when the end date is updated.
        /// </summary>
        protected virtual void OnEndDateChanged()
        {
        }

        /// <summary>
        /// Called when the <see cref="StartDate"/> property changes.
        /// Override this method to implement custom behavior when the start date is updated.
        /// </summary>
        protected virtual void OnStartDateChanged()
        {
        }

        /// <summary>
        /// Called when the <see cref="OccurrenceCount"/> property changes.
        /// Override this method to implement custom behavior when the occurrence count is updated.
        /// </summary>
        protected virtual void OnOccurrenceCountChanged()
        {
        }

        /// <summary>
        /// Coerces the specified minimum and maximum dates based on the start and end dates of the repeat pattern.
        /// </summary>
        /// <param name="minDate">The minimum date to consider.</param>
        /// <param name="maxDate">The maximum date to consider.</param>
        protected virtual void CoerceMinMaxDate(ref DateOnly minDate, ref DateOnly maxDate)
        {
            if (EndDate < maxDate)
                maxDate = EndDate;

            minDate = StartDate;
        }

        /// <summary>
        /// Defines a structure to hold the result of getting dates from the repeat pattern rule.
        /// </summary>
        public struct RuleGetDatesResult
        {
            /// <summary>
            /// Gets an empty instance of the <see cref="RuleGetDatesResult"/> struct, representing no dates found.
            /// </summary>
            public static readonly RuleGetDatesResult Empty = new(Array.Empty<DateOnly>());

            /// <summary>
            /// Gets the collection of dates that match the repeat pattern within the specified range.
            /// </summary>
            public IEnumerable<DateOnly> Dates { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="RuleGetDatesResult"/> struct
            /// with the specified collection of dates.
            /// </summary>
            /// <param name="dates">The collection of dates that match the repeat pattern within the specified range.</param>
            public RuleGetDatesResult(IEnumerable<DateOnly> dates)
            {
                Dates = dates;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RuleGetDatesResult"/> struct.
            /// </summary>
            public RuleGetDatesResult()
                : this(Array.Empty<DateOnly>())
            {
            }
        }

        /// <summary>
        /// Defines a structure to hold parameters for getting dates from the repeat pattern rule.
        /// </summary>
        public struct RuleGetDatesParams
        {
            /// <summary>
            /// Gets or sets the minimum date to consider.
            /// </summary>
            public DateOnly MinDate { get; set; }

            /// <summary>
            /// Gets or sets the maximum date to consider.
            /// </summary>
            public DateOnly MaxDate { get; set; }
        }

    }
}
