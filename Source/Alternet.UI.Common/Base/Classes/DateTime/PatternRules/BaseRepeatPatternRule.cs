using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the types of end conditions for a repeat pattern.
    /// </summary>
    public enum RepeatPatternEndCondition
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
    /// Represents a rule for a repeat pattern in scheduling events or tasks.
    /// </summary>
    public abstract partial class BaseRepeatPatternRule : SerializableObject, IDateRepeatPatternRule
    {
        private DateOnly startDate;
        private TimeOnly startTime;
        private TimeSpan duration;
        private DateOnly endDate;
        private int occurrenceCount = 1;
        private RepeatPatternEndCondition endCondtion = RepeatPatternEndCondition.OnDate;

        /// <summary>
        /// Gets or sets the end date and time of the repeat pattern,
        /// combining the <see cref="EndDate"/> and <see cref="EndTime"/> properties.
        /// </summary>
        [Browsable(false)]
        public virtual DateTime End
        {
            get
            {
                return EndDate.ToDateTime(EndTime);
            }

            set
            {
                if (value == End)
                    return;

                DoInsideSuspendedPropertyChanged(() =>
                {
                    EndDate = DateOnly.FromDateTime(value);
                    EndTime = TimeOnly.FromDateTime(value);
                });
            }
        }

        /// <summary>
        /// Gets or sets the start date and time of the repeat pattern,
        /// combining the <see cref="StartDate"/> and <see cref="StartTime"/> properties.
        /// </summary>
        [Browsable(false)]
        public virtual DateTime Start
        {
            get
            {
                return StartDate.ToDateTime(StartTime);
            }

            set
            {
                if (value == Start)
                    return;

                DoInsideSuspendedPropertyChanged(() =>
                {
                    StartDate = DateOnly.FromDateTime(value);
                    StartTime = TimeOnly.FromDateTime(value);
                });
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is an all-day event.
        /// This is calculated based on the <see cref="Duration"/> property
        /// and can be set to coerce the duration to an all-day duration.
        /// </summary>
        public virtual bool IsAllDay
        {
            get => DateUtils.IsValidAllDayDuration(Duration);
            set
            {
                if (value)
                    Duration = DateUtils.CoerceAllDayDuration(Duration);
            }
        }

        /// <summary>
        /// Gets or sets the start date of the repeat pattern range. 
        /// </summary>
        public virtual DateOnly StartDate
        {
            get => startDate;
            set
            {
                if (SetProperty(ref startDate, value, nameof(StartDate)))
                {
                    OnStartDateChanged();
                }
            }
        }

        /// <summary>
        /// Gets the end time of the repeat pattern, calculated as the start time plus the duration.
        /// </summary>
        [Browsable(false)]
        public TimeOnly EndTime
        {
            get
            {
                return StartTime.Add(Duration);
            }

            set
            {
                Duration = value - StartTime;
            }
        }

        /// <summary>
        /// Gets or sets the start time of the repeat pattern.
        /// </summary>
        public virtual TimeOnly StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                SetProperty(ref startTime, value, nameof(StartTime));
            }
        }

        /// <summary>
        /// Gets or sets the duration in time.
        /// </summary>
        public virtual TimeSpan Duration
        {
            get
            {
                return duration;
            }

            set
            {
                if (value < TimeSpan.Zero)
                    value = TimeSpan.Zero;
                SetProperty(ref duration, value, nameof(Duration));
            }
        }

        /// <summary>
        /// Gets or sets the end condition of the repeat pattern, which determines
        /// how the repeat pattern ends (never, after a number of occurrences, or on a specific date).
        /// </summary>
        public virtual RepeatPatternEndCondition EndCondition
        {
            get
            {
                return endCondtion;
            }

            set
            {
                SetProperty(ref endCondtion, value, nameof(EndCondition));
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
                if (SetProperty(ref endDate, value, nameof(EndDate)))
                {
                    OnEndDateChanged();
                }
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
                if (SetProperty(ref occurrenceCount, value, nameof(OccurrenceCount)))
                {
                    OnOccurrenceCountChanged();
                }
            }
        }

        /// <summary>
        /// Gets the occurrences of the repeat pattern within the specified range and up to the maximum date.
        /// </summary>
        /// <param name="prm">The parameters specifying the minimum and maximum
        /// dates to consider for the occurrences.</param>
        /// <returns>An enumerable of the occurrence dates.</returns>
        public virtual PatternRuleGetDatesResult GetDates(PatternRuleGetDatesParams prm)
        {
            return PatternRuleGetDatesResult.Empty;
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
        protected virtual PatternRuleGetDatesResult GetDates(
            PatternRuleGetDatesParams prm,
            Predicate<DateOnly> predicate,
            Func<DateOnly, DateOnly> nextDate)
        {
            switch (EndCondition)
            {
                case RepeatPatternEndCondition.Never:
                    return new(GetDatesEndsOnDate(prm.MaxDate));
                case RepeatPatternEndCondition.AfterOccurrence:
                    return new(GetDatesEndsAfterOccurrence());
                case RepeatPatternEndCondition.OnDate:
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
        /// <returns>A <see cref="PatternRuleGetDatesResult"/> containing the filtered dates.</returns>
        protected virtual PatternRuleGetDatesResult GetDates(
            in PatternRuleGetDatesParams prm,
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
                case RepeatPatternEndCondition.Never:
                case RepeatPatternEndCondition.AfterOccurrence:
                    maxDate = prm.MaxDate;
                    break;
                case RepeatPatternEndCondition.OnDate:
                    maxDate = DateUtils.Min(EndDate, prm.MaxDate);
                    break;
            }

            IEnumerable<DateOnly> GetUnfiltered()
            {
                PatternRuleGetDatesParams unfilteredPrm = new()
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

            if (EndCondition == RepeatPatternEndCondition.AfterOccurrence)
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
