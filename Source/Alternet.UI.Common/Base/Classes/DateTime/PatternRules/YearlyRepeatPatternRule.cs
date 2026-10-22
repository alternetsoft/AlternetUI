using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a yearly repeat pattern rule.
    /// </summary>
    public partial class YearlyRepeatPatternRule : OwnedRepeatPatternRule, IPropertyBagSerialize
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
        /// <param name="owner">The owner <see cref="BaseRepeatPatternRule"/> of this rule.</param>
        public YearlyRepeatPatternRule(BaseRepeatPatternRule owner) : base(owner)
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
        /// Resets the current instance to its default state which was set when the instance was created.
        /// </summary>
        public virtual void Reset()
        {
            Assign(null);
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

            propertyBag.SetStringProperty(prefix + nameof(Kind), Kind.ToString());
            propertyBag.SetStringProperty(prefix + nameof(Month), Month.ToString());
            propertyBag.SetIntProperty(prefix + nameof(IntervalYears), IntervalYears);
            propertyBag.SetIntProperty(prefix + nameof(DayOfMonth), DayOfMonth);
            propertyBag.SetStringProperty(prefix + nameof(DayOfWeekIndex), DayOfWeekIndex.ToString());
            propertyBag.SetStringProperty(prefix + nameof(DayOfWeek), DayOfWeek.ToString());
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

                if (propertyBag.TryGetEnumProperty(prefix + nameof(Kind), out RepeatKind parsedKind))
                {
                    Kind = parsedKind;
                }

                if (propertyBag.TryGetEnumProperty(prefix + nameof(Month), out CalendarMonth parsedMonth))
                {
                    Month = parsedMonth;
                }

                if (propertyBag.TryGetIntProperty(prefix + nameof(IntervalYears), out int parsedIntervalYears))
                {
                    IntervalYears = parsedIntervalYears;
                }

                if (propertyBag.TryGetIntProperty(prefix + nameof(DayOfMonth), out int parsedDayOfMonth))
                {
                    DayOfMonth = parsedDayOfMonth;
                }

                if (propertyBag.TryGetEnumProperty(prefix + nameof(DayOfWeekIndex), out RelativeWeekday parsedDayOfWeekIndex))
                {
                    DayOfWeekIndex = parsedDayOfWeekIndex;
                }

                if (propertyBag.TryGetEnumProperty(prefix + nameof(DayOfWeek), out ExtendedDayOfWeek parsedDayOfWeek))
                {
                    DayOfWeek = parsedDayOfWeek;
                }

                return true;
            });
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current
        /// instance of <see cref="YearlyRepeatPatternRule"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance;
        /// otherwise, <c>false</c>.</returns>
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
        /// <param name="other">The instance from which to copy values. If <c>null</c>, default values are assigned.
        /// If the instance is not of type <see cref="YearlyRepeatPatternRule"/>, no values are copied.</param>
        public virtual void Assign(object? other)
        {
            if (other == null)
            {
                DoInsideSuspendedPropertyChanged(() =>
                {
                    IntervalYears = 1;
                    Month = CalendarMonth.January;
                    DayOfWeek = ExtendedDayOfWeek.Day;
                    DayOfMonth = 1;
                    DayOfWeekIndex = RelativeWeekday.First;
                    Kind = RepeatKind.DayOfMonth;
                });
                return;
            }

            if (other is YearlyRepeatPatternRule otherRule)
            {
                if (Equals(other))
                    return;

                DoInsideSuspendedPropertyChanged(() =>
                {
                    IntervalYears = otherRule.IntervalYears;
                    Month = otherRule.Month;
                    DayOfWeek = otherRule.DayOfWeek;
                    DayOfMonth = otherRule.DayOfMonth;
                    DayOfWeekIndex = otherRule.DayOfWeekIndex;
                    Kind = otherRule.Kind;
                });
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
        /// <returns>The calculated date, or <c>null</c> if the date is invalid.</returns>
        public virtual DateOnly[]? GetDates(int year)
        {
            if (Kind == RepeatKind.DayOfMonth)
            {
                if (DayOfMonth < 1 || DayOfMonth > DateUtils.GetDaysInMonth(year, Month))
                    return null;
                return [new DateOnly(year, (int)Month, DayOfMonth)];
            }
            else if (Kind == RepeatKind.RelativeWeekday)
            {
                var relativeWeekday = new RelativeWeekdayOfMonth(DayOfWeekIndex, DayOfWeek, Month);
                return relativeWeekday.GetDates(year);
            }

            return null;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (IntervalYears, Month, DayOfWeek, DayOfMonth, DayOfWeekIndex, Kind, StartDate, EndDate, OccurrenceCount)
                .GetHashCode();
        }

        /// <summary>
        /// Gets the occurrences of the repeat pattern within the specified range and up to the maximum date, 
        /// without applying any end condition or occurrence count filtering.
        /// </summary>
        /// <returns>An enumerable collection of unfiltered dates within the specified range.</returns>
        protected virtual IDateRepeatPatternRule.RuleGetDatesResult GetDatesUnfiltered(IDateRepeatPatternRule.RuleGetDatesParams prm)
        {
            return new(Internal());

            IEnumerable<DateOnly> Internal()
            {
                int startYear = prm.MinDate.Year;
                int endYear = prm.MaxDate.Year;

                for (int year = startYear; year <= endYear; year += IntervalYears)
                {
                    DateOnly[]? occurrenceDates = GetDates(year);

                    if (occurrenceDates is null)
                        continue;

                    foreach (var d in occurrenceDates)
                    {
                        if (d < prm.MinDate)
                            continue;
                        if (d > prm.MaxDate)
                            break;
                        yield return d;
                    }
                }
            }
        }
    }
}
