using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a composite repeat pattern rule that combines daily, weekly, monthly, yearly
    /// and other repeat pattern rules.
    /// </summary>
    public partial class RepeatPatternRule : OwnedRepeatPatternRule,
        IEquatable<RepeatPatternRule>, IPropertyBagSerialize, IRepeatPatternRuleOwner
    {
        private readonly DailyRepeatPatternRule dailyRule;
        private readonly WeeklyRepeatPatternRule weeklyRule;
        private readonly MonthlyRepeatPatternRule monthlyRule;
        private readonly YearlyRepeatPatternRule yearlyRule;
        private readonly List<BaseRepeatPatternRule> rules = new();
        private ScheduleRepeatPattern kind = ScheduleRepeatPattern.None;
        private DateSelectionManager? excludedDates;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPatternRule"/> class.
        /// </summary>
        public RepeatPatternRule(IRepeatPatternRuleOwner? owner = null)
            : base(owner)
        {
            var now = DateTime.Now;
            ValidFromDate = now.ToDateOnly();
            StartTime = now.ToTimeOnly();
            ValidToDate = ValidFromDate;

            dailyRule = CreateDailyRule();
            weeklyRule = CreateWeeklyRule();
            monthlyRule = CreateMonthlyRule();
            yearlyRule = CreateYearlyRule();

            rules.Add(dailyRule);
            rules.Add(weeklyRule);
            rules.Add(monthlyRule);
            rules.Add(yearlyRule);

            dailyRule.PropertyChanged += OnChildRulePropertyChanged;
            weeklyRule.PropertyChanged += OnChildRulePropertyChanged;
            monthlyRule.PropertyChanged += OnChildRulePropertyChanged;
            yearlyRule.PropertyChanged += OnChildRulePropertyChanged;
        }

        /// <summary>
        /// Gets a value indicating whether the repeat pattern rule is used.
        /// (i.e., not set to <see cref="ScheduleRepeatPattern.None"/>).
        /// </summary>
        public bool IsRepeated => Kind != ScheduleRepeatPattern.None;

        /// <summary>
        /// Gets or sets the kind of repeat pattern represented by this composite rule.
        /// </summary>
        public virtual ScheduleRepeatPattern Kind
        {
            get => kind;
            set
            {
                SetProperty(ref kind, value, nameof(Kind));
            }
        }

        /// <summary>
        /// Gets the <see cref="DailyRepeatPatternRule"/> instance for daily recurrence settings.
        /// </summary>
        public DailyRepeatPatternRule DailyRule => dailyRule;

        /// <summary>
        /// Gets the <see cref="WeeklyRepeatPatternRule"/> instance for weekly recurrence settings. 
        /// </summary>
        public WeeklyRepeatPatternRule WeeklyRule => weeklyRule;

        /// <summary>
        /// Gets the <see cref="MonthlyRepeatPatternRule"/> instance for monthly recurrence settings.
        /// </summary>
        public MonthlyRepeatPatternRule MonthlyRule => monthlyRule;

        /// <summary>
        /// Gets the <see cref="YearlyRepeatPatternRule"/> instance for yearly recurrence settings.
        /// </summary>
        public YearlyRepeatPatternRule YearlyRule => yearlyRule;

        /// <summary>
        /// Gets a read-only list of all the repeat pattern rules contained in this composite rule.
        /// </summary>
        public IReadOnlyList<BaseRepeatPatternRule> Rules => rules;

        /// <summary>
        /// Gets the list of excluded dates in the repeat pattern rule.
        /// </summary>
        public virtual IEnumerable<DateOnly> ExcludedDates
        {
            get
            {
                if (excludedDates != null)
                {
                    try
                    {
                        var result = excludedDates.SelectedDates.ToArray();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        BaseObject.Nop(ex);
                        return [];
                    }
                }

                return Array.Empty<DateOnly>();
            }

            set
            {
                if (value == null || !value.Any())
                {
                    ClearExcludedDates();
                    return;
                }

                if (excludedDates != null)
                {
                    excludedDates.Clear();
                }
                else
                {
                    excludedDates = new DateSelectionManager();
                }

                excludedDates.SelectRange(value);

                RaisePropertyChanged(nameof(ExcludedDates));
            }
        }

        /// <summary>
        /// Loads the state of this instance from the specified <see cref="ISerializableObjectContext"/>.
        /// </summary>
        /// <param name="context">The context containing the property bag
        /// and optional prefix for property names.</param>
        /// <returns><c>true</c> if the state was successfully loaded; otherwise, <c>false</c>.</returns>
        public override bool LoadFrom(ISerializableObjectContext? context)
        {
            if (context == null)
                return false;

            var prefix = context.Prefix;
            var propertyBag = context.PropertyBag;

            if (propertyBag == null)
                return false;

            if (prefix is not null)
                prefix += ".";
            else
                prefix = "";

            return DoInsideSuspendedPropertyChanged(() =>
            {
                Reset();

                if (propertyBag.TryGetEnumProperty(prefix + nameof(Kind), out ScheduleRepeatPattern parsedKind))
                {
                    Kind = parsedKind;
                }

                if (propertyBag.TryGetDateProperty(prefix + nameof(ValidFromDate), out var validFromDate))
                {
                    ValidFromDate = validFromDate;
                }

                if (propertyBag.TryGetDateProperty(prefix + nameof(ValidToDate), out var validToDate))
                {
                    ValidToDate = validToDate;
                }

                if (propertyBag.TryGetTimeProperty(prefix + nameof(StartTime), out var startTime))
                {
                    StartTime = startTime;
                }

                if (propertyBag.TryGetIntProperty(prefix + nameof(OccurrenceCount), out var occurrenceCount))
                {
                    OccurrenceCount = occurrenceCount;
                }

                if (propertyBag.TryGetTimeSpanProperty(prefix + nameof(Duration), out var duration))
                {
                    Duration = duration;
                }

                if (propertyBag.TryGetEnumProperty(
                    prefix + nameof(EndCondition),
                    out RepeatPatternEndCondition endCondition))
                {
                    EndCondition = endCondition;
                }

                switch (Kind)
                {
                    case ScheduleRepeatPattern.Daily:
                        return DailyRule.LoadFrom(
                            new SerializableObjectContext(
                                propertyBag,
                                prefix + nameof(DailyRule)));
                    case ScheduleRepeatPattern.Weekly:
                        return WeeklyRule.LoadFrom(
                            new SerializableObjectContext(
                                propertyBag,
                                prefix + nameof(WeeklyRule)));
                    case ScheduleRepeatPattern.Monthly:
                        return MonthlyRule.LoadFrom(
                            new SerializableObjectContext(
                                propertyBag,
                                prefix + nameof(MonthlyRule)));
                    case ScheduleRepeatPattern.Yearly:
                        return YearlyRule.LoadFrom(
                            new SerializableObjectContext(
                                propertyBag,
                                prefix + nameof(YearlyRule)));
                }

                return true;
            });
        }

        /// <summary>
        /// Saves the current state of this instance
        /// to the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="context">The context to which the state will be saved.</param>
        public override bool SaveTo(ISerializableObjectContext? context)
        {
            if (context == null)
                return false;

            var prefix = context.Prefix;
            var propertyBag = context.PropertyBag;

            if (propertyBag == null)
                return false;

            if (prefix is not null)
                prefix += ".";
            else
                prefix = "";

            propertyBag.SetStringProperty(prefix + nameof(Kind), Kind.ToString());
            propertyBag.SetTimeProperty(prefix + nameof(StartTime), StartTime);
            propertyBag.SetTimeSpanProperty(prefix + nameof(Duration), Duration);
            propertyBag.SetIntProperty(prefix + nameof(OccurrenceCount), OccurrenceCount);
            propertyBag.SetStringProperty(prefix + nameof(EndCondition), EndCondition.ToString());
            propertyBag.SetDateProperty(prefix + nameof(ValidFromDate), ValidFromDate);
            propertyBag.SetDateProperty(prefix + nameof(ValidToDate), ValidToDate);

            switch (Kind)
            {
                case ScheduleRepeatPattern.Daily:
                    DailyRule.SaveTo(new SerializableObjectContext(propertyBag, prefix + nameof(DailyRule)));
                    break;
                case ScheduleRepeatPattern.Weekly:
                    WeeklyRule.SaveTo(new SerializableObjectContext(propertyBag, prefix + nameof(WeeklyRule)));
                    break;
                case ScheduleRepeatPattern.Monthly:
                    MonthlyRule.SaveTo(new SerializableObjectContext(propertyBag, prefix + nameof(MonthlyRule)));
                    break;
                case ScheduleRepeatPattern.Yearly:
                    YearlyRule.SaveTo(new SerializableObjectContext(propertyBag, prefix + nameof(YearlyRule)));
                    break;
            }

            return true;
        }

        /// <summary>
        /// Creates a deep copy of the current instance.
        /// </summary>
        /// <returns>A new instance that is a deep copy of the current instance.</returns>
        public virtual RepeatPatternRule Clone()
        {
            var clone = new RepeatPatternRule();
            clone.Assign(this);
            return clone;
        }

        /// <summary>
        /// Determines whether the specified object is equal
        /// to the current instance of <see cref="RepeatPatternRule"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not RepeatPatternRule other)
            {
                return false;
            }

            return Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(RepeatPatternRule? other)
        {
            if (other == null)
            {
                return false;
            }

            return Kind == other.Kind &&
                   ValidFromDate == other.ValidFromDate &&
                   StartTime == other.StartTime &&
                   Duration == other.Duration &&
                   ValidToDate == other.ValidToDate &&
                   OccurrenceCount == other.OccurrenceCount &&
                   EndCondition == other.EndCondition &&
                   DailyRule.Equals(other.DailyRule) &&
                   WeeklyRule.Equals(other.WeeklyRule) &&
                   MonthlyRule.Equals(other.MonthlyRule) &&
                   YearlyRule.Equals(other.YearlyRule);
        }

        /// <summary>
        /// Resets the current instance to its default state which was set when the instance was created.
        /// </summary>
        public virtual void Reset()
        {
            Assign(null);
        }

        /// <summary>
        /// Assigns the values from another instance to the current instance.
        /// </summary>
        /// <param name="other">The instance from which to copy values.</param>
        /// <returns><c>true</c> if the values were successfully assigned; otherwise, <c>false</c>.
        /// When assigned value equals the current instance, the method returns <c>false</c>.</returns>
        public virtual bool Assign(object? other)
        {
            if (other == null)
            {
                DoInsideSuspendedPropertyChanged(() =>
                {
                    var now = DateTime.Now;
                    ValidFromDate = now.ToDateOnly();
                    StartTime = now.ToTimeOnly();
                    Duration = TimeSpan.Zero;
                    OccurrenceCount = 1;
                    EndCondition = RepeatPatternEndCondition.OnDate;
                    ValidToDate = ValidFromDate;
                    Kind = ScheduleRepeatPattern.None;

                    DailyRule.Assign(null);
                    WeeklyRule.Assign(null);
                    MonthlyRule.Assign(null);
                    YearlyRule.Assign(null);
                });

                return true;
            }

            if (other is RepeatPatternRule otherRule)
            {
                if (Equals(other))
                    return false;

                DoInsideSuspendedPropertyChanged(() =>
                {
                    ValidFromDate = otherRule.ValidFromDate;
                    StartTime = otherRule.StartTime;
                    Duration = otherRule.Duration;
                    ValidToDate = otherRule.ValidToDate;
                    OccurrenceCount = otherRule.OccurrenceCount;
                    EndCondition = otherRule.EndCondition;

                    DailyRule.Assign(otherRule.DailyRule);
                    WeeklyRule.Assign(otherRule.WeeklyRule);
                    MonthlyRule.Assign(otherRule.MonthlyRule);
                    YearlyRule.Assign(otherRule.YearlyRule);

                    Kind = otherRule.Kind;
                });

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the currently selected repeat pattern rule based on the <see cref="Kind"/> property.
        /// </summary>
        /// <returns>The currently selected <see cref="OwnedRepeatPatternRule"/> instance,
        /// or <c>null</c> if no rule is selected.</returns>
        public virtual OwnedRepeatPatternRule? GetSelectedRule()
        {
            switch (Kind)
            {
                case ScheduleRepeatPattern.Daily:
                    return dailyRule;
                case ScheduleRepeatPattern.Weekly:
                    return weeklyRule;
                case ScheduleRepeatPattern.Monthly:
                    return monthlyRule;
                case ScheduleRepeatPattern.Yearly:
                    return yearlyRule;
                default:
                    return null;
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified date is excluded from the repeat pattern.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns><c>true</c> if the date is excluded; otherwise, <c>false</c>.</returns>
        public virtual bool IsDateExcluded(DateOnly date)
        {
            return excludedDates != null && excludedDates.IsSelected(date);
        }

        /// <summary>
        /// Clears all excluded dates from the repeat pattern.
        /// </summary>
        public virtual void ClearExcludedDates()
        {
            if (excludedDates != null)
            {
                excludedDates.Clear();
                RaisePropertyChanged(nameof(ExcludedDates));
            }
        }

        /// <summary>
        /// Determines whether there are any excluded dates in the repeat pattern.
        /// </summary>
        /// <returns><c>true</c> if there are any excluded dates; otherwise, <c>false</c>.</returns>
        public virtual bool HasExcludedDates()
        {
            return excludedDates != null && excludedDates.HasAnySelected();
        }

        /// <summary>
        /// Adds a date to the list of excluded dates in the repeat pattern.
        /// </summary>
        /// <param name="date">The date to add to the list of excluded dates.</param>
        /// <returns><c>true</c> if the exclusion state of the date was changed;
        /// otherwise, <c>false</c>.</returns>
        public virtual bool AddExcludedDate(DateOnly date)
        {
            excludedDates ??= new DateSelectionManager();
            var result = excludedDates.Select(date);

            if (result)
            {
                RaisePropertyChanged(nameof(ExcludedDates));
            }

            return result;
        }

        /// <summary>
        /// Removes a date from the list of excluded dates in the repeat pattern.
        /// </summary>
        /// <param name="date">The date to remove from the list of excluded dates.</param>
        /// <returns><c>true</c> if the exclusion state of the date was changed;
        /// otherwise, <c>false</c>.</returns>
        public virtual bool RemoveExcludedDate(DateOnly date)
        {
            if (excludedDates != null)
            {
                var result = excludedDates.Select(date, false);

                if (result)
                {
                    RaisePropertyChanged(nameof(ExcludedDates));
                }

                return result;
            }

            return false;
        }

        /// <inheritdoc/>
        public override PatternRuleGetDatesResult GetDates(PatternRuleGetDatesParams prm)
        {
            var rule = GetSelectedRule();

            if (rule == null)
            {
                return new PatternRuleGetDatesResult(WithoutExcludedDates(Internal()));
            }

            IEnumerable<DateOnly> WithoutExcludedDates(IEnumerable<DateOnly> dates)
            {
                if (HasExcludedDates())
                {
                    return Internal();
                }
                else
                {
                    return dates;
                }

                IEnumerable<DateOnly> Internal()
                {
                    foreach (var date in dates)
                    {
                        if (!IsDateExcluded(date))
                        {
                            yield return date;
                        }
                    }
                }
            }

            IEnumerable<DateOnly> Internal()
            {
                var minDate = prm.MinDate;
                var maxDate = prm.MaxDate;
                var startDate = ValidFromDate;
                var endDate = ValidToDate;

                DateUtils.Sort(ref minDate, ref maxDate);
                DateUtils.Sort(ref startDate, ref endDate);

                if (minDate > endDate || maxDate < startDate)
                {
                    yield break;
                }

                if (minDate > startDate)
                {
                    startDate = minDate;
                }

                if (maxDate < endDate)
                {
                    endDate = maxDate;
                }

                while (startDate <= endDate)
                {
                    yield return startDate;
                    startDate = startDate.AddDays(1);
                }
            }

            var result = rule.GetDates(prm);

            return new(WithoutExcludedDates(result.Dates));
        }

        /// <summary>
        /// Called when a property of any child rule changes, allowing the composite rule to respond accordingly.
        /// </summary>
        /// <param name="sender">The child rule that raised the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnChildRulePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaiseNullPropertyChanged();
        }

        /// <inheritdoc/>
        protected override bool UseGuidForUniqueId => true;

        /// <inheritdoc/>
        protected override void OnOccurrenceCountChanged()
        {
            base.OnOccurrenceCountChanged();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="YearlyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="YearlyRepeatPatternRule"/> class.</returns>
        protected virtual YearlyRepeatPatternRule CreateYearlyRule()
        {
            return new YearlyRepeatPatternRule(this);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MonthlyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="MonthlyRepeatPatternRule"/> class.</returns>
        protected virtual MonthlyRepeatPatternRule CreateMonthlyRule()
        {
            return new MonthlyRepeatPatternRule(this);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WeeklyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="WeeklyRepeatPatternRule"/> class.</returns>
        protected virtual WeeklyRepeatPatternRule CreateWeeklyRule()
        {
            return new WeeklyRepeatPatternRule(this);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DailyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="DailyRepeatPatternRule"/> class.</returns>
        protected virtual DailyRepeatPatternRule CreateDailyRule()
        {
            return new DailyRepeatPatternRule(this);
        }
    }
}
