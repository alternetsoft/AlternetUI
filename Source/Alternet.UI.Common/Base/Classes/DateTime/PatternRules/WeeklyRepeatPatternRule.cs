using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a weekly repeat pattern rule.
    /// </summary>
    public partial class WeeklyRepeatPatternRule : OwnedRepeatPatternRule, IPropertyBagSerialize
    {
        private int intervalWeeks = 1;
        private DaysOfWeek days;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeeklyRepeatPatternRule"/> class with the specified owner.
        /// </summary>
        /// <param name="owner">The owner <see cref="BaseRepeatPatternRule"/> of this rule.</param>
        public WeeklyRepeatPatternRule(BaseRepeatPatternRule owner) : base(owner)
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
        /// Resets the current instance to its default state which was set when the instance was created.
        /// </summary>
        public virtual void Reset()
        {
            Assign(null);
        }

        /// <summary>
        /// Loads the state of this instance from the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="propertyBag">The property bag from which to load the state.</param>
        /// <param name="prefix">An optional prefix to prepend to property names.</param>
        /// <returns><c>true</c> if the state was successfully loaded; otherwise, <c>false</c>.</returns>
        public virtual bool LoadFrom(IPropertyBag? propertyBag, string? prefix = null)
        {
            if (propertyBag == null)
                return false;

            if (prefix is not null)
                prefix += ".";
            else
                prefix = "";

            return DoInsideSuspendedPropertyChanged(() =>
            {
                Reset();

                if (propertyBag.TryGetIntProperty(prefix + nameof(IntervalWeeks), out var intervalWeeks))
                {
                    IntervalWeeks = intervalWeeks;
                }

                if (propertyBag.TryGetStringProperty(prefix + nameof(WeekDays), out var weekDaysString))
                {
                    if (Enum.TryParse<DaysOfWeek>(weekDaysString, out var weekDays))
                        WeekDays = weekDays;
                }

                return true;
            });
        }

        /// <summary>
        /// Saves the current state of this instance
        /// to the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="propertyBag">The property bag to which the state will be saved.</param>
        /// <param name="prefix">An optional prefix to prepend to property names.</param>
        public virtual void SaveTo(IPropertyBag propertyBag, string? prefix = null)
        {
            if (prefix is not null)
                prefix += ".";
            else
                prefix = "";

            propertyBag.SetIntProperty(prefix + nameof(IntervalWeeks), IntervalWeeks);
            propertyBag.SetStringProperty(prefix + nameof(WeekDays), WeekDays.ToString());
        }

        /// <summary>
        /// Determines whether the specified object is equal to the
        /// current instance of <see cref="WeeklyRepeatPatternRule"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance;
        /// otherwise, <c>false</c>.</returns>
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
        /// <param name="other">The instance from which to copy values. If <c>null</c>, default values are assigned.
        /// If the instance is not of type <see cref="WeeklyRepeatPatternRule"/>, no values are copied.</param>
        public virtual void Assign(object? other)
        {
            if (other == null)
            {
                DoInsideSuspendedPropertyChanged(() =>
                {
                    IntervalWeeks = 1;
                    WeekDays = DaysOfWeek.None;
                });
                return;
            }

            if (other is WeeklyRepeatPatternRule otherRule)
            {
                if (Equals(other))
                    return;

                DoInsideSuspendedPropertyChanged(() =>
                {
                    IntervalWeeks = otherRule.IntervalWeeks;
                    WeekDays = otherRule.WeekDays;
                });
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (IntervalWeeks, WeekDays, StartDate, EndDate, OccurrenceCount).GetHashCode();
        }

        /// <inheritdoc/>
        public override IDateRepeatPatternRule.RuleGetDatesResult GetDates(IDateRepeatPatternRule.RuleGetDatesParams prm)
        {
            return GetDates(prm, GetDatesUnfiltered);
        }

        /// <summary>
        /// Determines whether the specified date is included in the repeat pattern based on the defined days of the week.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns><c>true</c> if the date is included in the repeat pattern; otherwise, <c>false</c>.</returns>
        protected virtual bool IsDateInWeekDays(DateOnly date)
        {
            return WeekDays.HasDay(date.DayOfWeek);
        }

        /// <summary>
        /// Gets the occurrences of the repeat pattern within the specified range and up to the maximum date, 
        /// without applying any end condition or occurrence count filtering.
        /// </summary>
        /// <param name="prm">The parameters specifying the date range and maximum date.</param>
        /// <returns>An enumerable collection of unfiltered dates within the specified range.</returns>
        protected virtual IDateRepeatPatternRule.RuleGetDatesResult GetDatesUnfiltered(IDateRepeatPatternRule.RuleGetDatesParams prm)
        {
            return new(Internal());

            IEnumerable<DateOnly> Internal()
            {
                int startWeek = IWeekFormatProvider.EffectiveWeekOfYear(prm.MinDate, prm.WeekFormat);
                var startDate = IWeekFormatProvider.GetStartOfWeek(prm.MinDate.Year, startWeek, prm.WeekFormat);
                var maxDate = prm.MaxDate;

                while (startDate <= maxDate)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        var weekDate = startDate.AddDays(i);

                        if (weekDate < prm.MinDate)
                            continue;
                        if (weekDate > maxDate)
                            break;

                        if (IsDateInWeekDays(weekDate))
                        {
                            yield return weekDate;
                        }
                    }

                    startDate = startDate.AddDays(7 * IntervalWeeks);
                }
            }
        }
    }
}
