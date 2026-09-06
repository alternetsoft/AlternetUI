using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a rule for a repeat pattern in scheduling events or tasks.
    /// </summary>
    public abstract partial class DateRepeatPatternRule : BaseObjectWithNotify, IDateRepeatPatternRule
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
        public virtual IDateRepeatPatternRule.RuleGetDatesResult GetDates(IDateRepeatPatternRule.RuleGetDatesParams prm)
        {
            return IDateRepeatPatternRule.RuleGetDatesResult.Empty;
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
        protected virtual IDateRepeatPatternRule.RuleGetDatesResult GetDates(
            IDateRepeatPatternRule.RuleGetDatesParams prm,
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
        /// Gets the occurrences of the repeat pattern within the specified range and up to the maximum date,
        /// taking into account the end condition and occurrence count.
        /// </summary>
        /// <param name="prm">The parameters specifying the date range and maximum date.</param>
        /// <param name="getDatesUnfiltered">A delegate to get unfiltered dates within the specified range.</param>
        /// <returns>A <see cref="IDateRepeatPatternRule.RuleGetDatesResult"/> containing the filtered dates.</returns>
        protected virtual IDateRepeatPatternRule.RuleGetDatesResult GetDates(
            in IDateRepeatPatternRule.RuleGetDatesParams prm,
            GetDatesDelegate getDatesUnfiltered)
        {
            var weekFormat = prm.WeekFormat;
            var prmMinDate = prm.MinDate;
            DateOnly minDate = StartDate;
            DateOnly maxDate;
            var min = DateUtils.Max(StartDate, prmMinDate);

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

            IEnumerable<DateOnly> GetUnfiltered()
            {
                IDateRepeatPatternRule.RuleGetDatesParams unfilteredPrm = new()
                {
                    MinDate = minDate,
                    MaxDate = maxDate,
                    WeekFormat = weekFormat,
                };

                return getDatesUnfiltered(unfilteredPrm).Dates;
            }

            IEnumerable<DateOnly> GetDates()
            {
                foreach (var date in GetUnfiltered())
                {
                    if (date >= min)
                        yield return date;
                }
            }

            IEnumerable<DateOnly> GetDatesEndsAfterOccurrence()
            {
                var count = OccurrenceCount;

                if (count <= 0)
                    yield break;

                var numProcessed = 0;

                foreach (var date in GetUnfiltered())
                {
                    if (date >= min)
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
    }
}
